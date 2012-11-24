/*
	FluorineFx open source library 
	Copyright (C) 2007 Zoltan Csibi, zoltan@TheSilentGroup.com, FluorineFx.com 
	
	This library is free software; you can redistribute it and/or
	modify it under the terms of the GNU Lesser General Public
	License as published by the Free Software Foundation; either
	version 2.1 of the License, or (at your option) any later version.
	
	This library is distributed in the hope that it will be useful,
	but WITHOUT ANY WARRANTY; without even the implied warranty of
	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
	Lesser General Public License for more details.
	
	You should have received a copy of the GNU Lesser General Public
	License along with this library; if not, write to the Free Software
	Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
*/
using System;
using System.Text;
using System.IO;
using System.Collections;
using System.Reflection;
using System.ComponentModel;
using System.Globalization;
using FluorineFx.Util;

namespace FluorineFx.Json
{
	/// <summary>
	/// Specifies reference loop handling options for the <see cref="JsonWriter"/>.
	/// </summary>
	public enum ReferenceLoopHandling
	{
		/// <summary>
		/// Throw a <see cref="JsonSerializationException"/> when a loop is encountered.
		/// </summary>
		Error = 0,
		/// <summary>
		/// Ignore loop references and do not serialize.
		/// </summary>
		Ignore = 1,
		/// <summary>
		/// Serialize loop references.
		/// </summary>
		Serialize = 2
	}

	/// <summary>
	/// Serializes and deserializes objects into and from the Json format.
	/// The <see cref="JsonSerializer"/> enables you to control how objects are encoded into Json.
	/// </summary>
	public class JsonSerializer
	{
		private ReferenceLoopHandling _referenceLoopHandling;
		private int _level;
		private JsonConverterCollection _converters;
		//Dictionary<Type, MemberMappingCollection>
		private Hashtable _typeMemberMappings;

		/// <summary>
		/// Get or set how reference loops (e.g. a class referencing itself) is handled.
		/// </summary>
		public ReferenceLoopHandling ReferenceLoopHandling
		{
			get { return _referenceLoopHandling; }
			set
			{
				if (value < ReferenceLoopHandling.Error || value > ReferenceLoopHandling.Serialize)
				{
					throw new ArgumentOutOfRangeException("value");
				}
				_referenceLoopHandling = value;
			}
		}

		public JsonConverterCollection Converters
		{
			get
			{
				if (_converters == null)
					_converters = new JsonConverterCollection();

				return _converters;
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonSerializer"/> class.
		/// </summary>
		public JsonSerializer()
		{
			_referenceLoopHandling = ReferenceLoopHandling.Error;
		}

		#region Deserialize
		/// <summary>
		/// Deserializes the Json structure contained by the specified <see cref="JsonReader"/>.
		/// </summary>
		/// <param name="reader">The <see cref="JsonReader"/> that contains the Json structure to deserialize.</param>
		/// <returns>The <see cref="Object"/> being deserialized.</returns>
		public object Deserialize(JsonReader reader)
		{
			return Deserialize(reader, null);
		}

		/// <summary>
		/// Deserializes the Json structure contained by the specified <see cref="JsonReader"/>
		/// into an instance of the specified type.
		/// </summary>
		/// <param name="reader">The type of object to create.</param>
		/// <param name="objectType">The <see cref="Type"/> of object being deserialized.</param>
		/// <returns>The instance of <paramref name="objectType"/> being deserialized.</returns>
		public object Deserialize(JsonReader reader, Type objectType)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");

			if (!reader.Read())
				return null;

			return GetObject(reader, objectType);
		}

		private JavaScriptArray PopulateJavaScriptArray(JsonReader reader)
		{
			JavaScriptArray jsArray = new JavaScriptArray();

			while (reader.Read())
			{
				switch (reader.TokenType)
				{
					case JsonToken.EndArray:
						return jsArray;
					case JsonToken.Comment:
						break;
					default:
						object value = GetObject(reader, null);

						jsArray.Add(value);
						break;
				}
			}

			throw new JsonSerializationException("Unexpected end while deserializing array.");
		}

		private JavaScriptObject PopulateJavaScriptObject(JsonReader reader)
		{
			JavaScriptObject jsObject = new JavaScriptObject();

			while (reader.Read())
			{
				switch (reader.TokenType)
				{
					case JsonToken.PropertyName:
						string memberName = reader.Value.ToString();

						// move to the value token. skip comments
						do
						{
							if (!reader.Read())
								throw new JsonSerializationException("Unexpected end while deserializing object.");
						} while (reader.TokenType == JsonToken.Comment);

						object value = GetObject(reader, null);

						jsObject[memberName] = value;
						break;
					case JsonToken.EndObject:
						return jsObject;
					case JsonToken.Comment:
						break;
					default:
						throw new JsonSerializationException("Unexpected token while deserializing object: " + reader.TokenType);
				}
			}

			throw new JsonSerializationException("Unexpected end while deserializing object.");
		}

		private object GetObject(JsonReader reader, Type objectType)
		{
			_level++;

			object value;
			JsonConverter converter;

			if (HasMatchingConverter(objectType, out converter))
			{
				return converter.ReadJson(reader, objectType);
			}
			else
			{
				switch (reader.TokenType)
				{
						// populate a typed object or generic dictionary/array
						// depending upon whether an objectType was supplied
					case JsonToken.StartObject:
						value = (objectType != null) ? PopulateObject(reader, objectType) : PopulateJavaScriptObject(reader);
						break;
					case JsonToken.StartArray:
						value = (objectType != null) ? PopulateList(reader, objectType) : PopulateJavaScriptArray(reader);
						break;
					case JsonToken.Integer:
					case JsonToken.Float:
					case JsonToken.String:
					case JsonToken.Boolean:
					case JsonToken.Date:
						value = EnsureType(reader.Value, objectType);
						break;
					case JsonToken.Constructor:
						value = reader.Value.ToString();
						break;
					case JsonToken.Null:
					case JsonToken.Undefined:
						value = null;
						break;
					default:
						throw new JsonSerializationException("Unexpected token whil deserializing object: " + reader.TokenType);
				}
			}

			_level--;

			return value;
		}

		private object EnsureType(object value, Type targetType)
		{
			// do something about null value when the targetType is a valuetype?
			if (value == null)
				return null;

			if (targetType == null)
				return value;

			Type valueType = value.GetType();

			// type of value and type of target don't match
			// attempt to convert value's type to target's type
			if (valueType != targetType)
			{
				TypeConverter targetConverter = TypeDescriptor.GetConverter(targetType);

				if (!targetConverter.CanConvertFrom(valueType))
				{
					if (targetConverter.CanConvertFrom(typeof(string)))
					{
						string valueString = TypeDescriptor.GetConverter(value).ConvertToInvariantString(value);

						return targetConverter.ConvertFromInvariantString(valueString);
					}

					if (!targetType.IsAssignableFrom(valueType))
						throw new InvalidOperationException(string.Format("Cannot convert object of type '{0}' to type '{1}'", value.GetType(), targetType));

					return value;
				}
        
				return targetConverter.ConvertFrom(null, CultureInfo.InvariantCulture, value);
			}
			else
			{
				return value;
			}
		}

		private MemberMappingCollection GetMemberMappings(Type objectType)
		{
			if (_typeMemberMappings == null)
				_typeMemberMappings = new Hashtable();
			MemberMappingCollection memberMappings;

			if (_typeMemberMappings.Contains(objectType))
				return _typeMemberMappings[objectType] as MemberMappingCollection;

			memberMappings = CreateMemberMappings(objectType);
			_typeMemberMappings[objectType] = memberMappings;

			return memberMappings;
		}

		private MemberMappingCollection CreateMemberMappings(Type objectType)
		{
			MemberInfo[] members = ReflectionUtils.GetFieldsAndProperties(objectType, BindingFlags.Public | BindingFlags.Instance);
			MemberMappingCollection memberMappings = new MemberMappingCollection();

			foreach (MemberInfo member in members)
			{
				string mappedName;

				JsonPropertyAttribute propertyAttribute = ReflectionUtils.GetAttribute(typeof(JsonPropertyAttribute), member, true) as JsonPropertyAttribute;
				if (propertyAttribute != null)
					mappedName = propertyAttribute.PropertyName;
				else
					mappedName = member.Name;

				bool ignored = member.IsDefined(typeof (JsonIgnoreAttribute), true);
				bool readable = ReflectionUtils.CanReadMemberValue(member);
				bool writable = ReflectionUtils.CanSetMemberValue(member);
				MemberMapping memberMapping = new MemberMapping(mappedName, member, ignored, readable, writable);

				memberMappings.Add(memberMapping);
			}

			return memberMappings;
		}

		private void SetObjectMember(JsonReader reader, object target, Type targetType, string memberName)
		{
			if (!reader.Read())
				throw new JsonSerializationException(string.Format("Unexpected end when setting {0}'s value.", memberName));

			MemberMappingCollection memberMappings = GetMemberMappings(targetType);
			Type memberType;
			object value;

			// test if a member with memberName exists on the type
			// otherwise test if target is a dictionary and assign value with the key if it is
			if (memberMappings.Contains(memberName))
			{
				MemberMapping memberMapping = memberMappings[memberName];

				if (memberMapping.Ignored)
					return;

				// ignore member if it is readonly
				if (!memberMapping.Writable)
					return;

				// get the member's underlying type
				memberType = ReflectionUtils.GetMemberUnderlyingType(memberMapping.Member);

				value = GetObject(reader, memberType);

				ReflectionUtils.SetMemberValue(memberMapping.Member, target, value);
			}
			else if (typeof(IDictionary).IsAssignableFrom(targetType))
			{
				// attempt to get the IDictionary's type
				memberType = ReflectionUtils.GetDictionaryValueType(target.GetType());

				value = GetObject(reader, memberType);

				((IDictionary)target).Add(memberName, value);
			}
			else
			{
                if( memberName != "__type" )
				    throw new JsonSerializationException(string.Format("Could not find member '{0}' on object of type '{1}'", memberName, targetType.GetType().Name));
			}
		}

		private object PopulateList(JsonReader reader, Type objectType)
		{
			Type elementType = ReflectionUtils.GetListItemType(objectType);

			IList populatedList = CollectionUtils.CreateList(objectType);
			while (reader.Read())
			{
				switch (reader.TokenType)
				{
					case JsonToken.EndArray:
						return populatedList;
					case JsonToken.Comment:
						break;
					default:
						object value = GetObject(reader, elementType);
						populatedList.Add(value);
						break;
				}
			}
			throw new JsonSerializationException("Unexpected end when deserializing array.");
		}

		private object PopulateObject(JsonReader reader, Type objectType)
		{
			object newObject = Activator.CreateInstance(objectType);

			while (reader.Read())
			{
				switch (reader.TokenType)
				{
					case JsonToken.PropertyName:
						string memberName = reader.Value.ToString();

						SetObjectMember(reader, newObject, objectType, memberName);
						break;
					case JsonToken.EndObject:
						return newObject;
					default:
						throw new JsonSerializationException("Unexpected token when deserializing object: " + reader.TokenType);
				}
			}

			throw new JsonSerializationException("Unexpected end when deserializing object.");
		}
		#endregion

		#region Serialize
		/// <summary>
		/// Serializes the specified <see cref="Object"/> and writes the Json structure
		/// to a <c>Stream</c> using the specified <see cref="TextWriter"/>. 
		/// </summary>
		/// <param name="textWriter">The <see cref="TextWriter"/> used to write the Json structure.</param>
		/// <param name="value">The <see cref="Object"/> to serialize.</param>
		public void Serialize(TextWriter textWriter, object value)
		{
			Serialize(new JsonWriter(textWriter), value);
		}

		/// <summary>
		/// Serializes the specified <see cref="Object"/> and writes the Json structure
		/// to a <c>Stream</c> using the specified <see cref="JsonWriter"/>. 
		/// </summary>
		/// <param name="jsonWriter">The <see cref="JsonWriter"/> used to write the Json structure.</param>
		/// <param name="value">The <see cref="Object"/> to serialize.</param>
		public void Serialize(JsonWriter jsonWriter, object value)
		{
			if (jsonWriter == null)
				throw new ArgumentNullException("jsonWriter");

			if (value == null)
				throw new ArgumentNullException("value");

			SerializeValue(jsonWriter, value);
		}


		private void SerializeValue(JsonWriter writer, object value)
		{
			JsonConverter converter;

			if (value == null)
			{
				writer.WriteNull();
			}
			else if (HasMatchingConverter(value.GetType(), out converter))
			{
				converter.WriteJson(writer, value);
			}
			else if (value is IConvertible)
			{
				IConvertible convertible = value as IConvertible;

				switch (convertible.GetTypeCode())
				{
					case TypeCode.String:
						writer.WriteValue((string)convertible);
						break;
					case TypeCode.Char:
						writer.WriteValue((char)convertible);
						break;
					case TypeCode.Boolean:
						writer.WriteValue((bool)convertible);
						break;
					case TypeCode.SByte:
						writer.WriteValue((sbyte)convertible);
						break;
					case TypeCode.Int16:
						writer.WriteValue((short)convertible);
						break;
					case TypeCode.UInt16:
						writer.WriteValue((ushort)convertible);
						break;
					case TypeCode.Int32:
						writer.WriteValue((int)convertible);
						break;
					case TypeCode.Byte:
						writer.WriteValue((byte)convertible);
						break;
					case TypeCode.UInt32:
						writer.WriteValue((uint)convertible);
						break;
					case TypeCode.Int64:
						writer.WriteValue((long)convertible);
						break;
					case TypeCode.UInt64:
						writer.WriteValue((ulong)convertible);
						break;
					case TypeCode.Single:
						writer.WriteValue((float)convertible);
						break;
					case TypeCode.Double:
						writer.WriteValue((double)convertible);
						break;
					case TypeCode.DateTime:
						writer.WriteValue((DateTime)convertible);
						break;
					case TypeCode.Decimal:
						writer.WriteValue((decimal)convertible);
						break;
					default:
						SerializeObject(writer, value);
						break;
				}
			}
			else if (value is IList)
			{
				SerializeList(writer, (IList)value);
			}
			else if (value is IDictionary)
			{
				SerializeDictionary(writer, (IDictionary)value);
			}
			else if (value is ICollection)
			{
				SerializeCollection(writer, (ICollection)value);
			}
			else if (value is Identifier)
			{
				writer.WriteRaw(value.ToString());
			}
			else
			{
				SerializeObject(writer, value);
			}
		}

		private bool HasMatchingConverter(Type type, out JsonConverter matchingConverter)
		{
			if (_converters != null)
			{
				for (int i = 0; i < _converters.Count; i++)
				{
					JsonConverter converter = _converters[i];

					if (converter.CanConvert(type))
					{
						matchingConverter = converter;
						return true;
					}
				}
			}

			matchingConverter = null;
			return false;
		}

		private void WriteMemberInfoProperty(JsonWriter writer, object value, MemberInfo member, string propertyName)
		{
			if (!ReflectionUtils.IsIndexedProperty(member))
			{
				object memberValue = ReflectionUtils.GetMemberValue(member, value);

				if (writer.SerializeStack.IndexOf(memberValue) != -1)
				{
					switch (_referenceLoopHandling)
					{
						case ReferenceLoopHandling.Error:
							throw new JsonSerializationException("Self referencing loop");
						case ReferenceLoopHandling.Ignore:
							// return from method
							return;
						case ReferenceLoopHandling.Serialize:
							// continue
							break;
						default:
							throw new InvalidOperationException(string.Format("Unexpected ReferenceLoopHandling value: '{0}'", _referenceLoopHandling));
					}
				}

				writer.WritePropertyName(propertyName != null ? propertyName : member.Name);
				SerializeValue(writer, memberValue);
			}
		}

		private void SerializeObject(JsonWriter writer, object value)
		{
			Type objectType = value.GetType();

			TypeConverter converter = TypeDescriptor.GetConverter(objectType);

			// use the objectType's TypeConverter if it has one and can convert to a string
			if (converter != null && !(converter is ComponentConverter) && converter.GetType() != typeof(TypeConverter))
			{
				if (converter.CanConvertTo(typeof(string)))
				{
					writer.WriteValue(converter.ConvertToInvariantString(value));
					return;
				}
			}

			writer.SerializeStack.Add(value);

			writer.WriteStartObject();

			MemberMappingCollection memberMappings = GetMemberMappings(objectType);

			foreach (MemberMapping memberMapping in memberMappings)
			{
				if (!memberMapping.Ignored && memberMapping.Readable)
					WriteMemberInfoProperty(writer, value, memberMapping.Member, memberMapping.MappingName);
			}

			writer.WriteEndObject();

			writer.SerializeStack.Remove(value);
		}

		private void SerializeCollection(JsonWriter writer, ICollection values)
		{
			object[] collectionValues = new object[values.Count];
			values.CopyTo(collectionValues, 0);

			SerializeList(writer, collectionValues);
		}

		private void SerializeList(JsonWriter writer, IList values)
		{
			writer.WriteStartArray();

			for (int i = 0; i < values.Count; i++)
			{
				SerializeValue(writer, values[i]);
			}

			writer.WriteEndArray();
		}

		private void SerializeDictionary(JsonWriter writer, IDictionary values)
		{
			writer.WriteStartObject();

			foreach (DictionaryEntry entry in values)
			{
				writer.WritePropertyName(entry.Key.ToString());
				SerializeValue(writer, entry.Value);
			}

			writer.WriteEndObject();
		}
		#endregion
	}
}
