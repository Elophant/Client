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
using System.Collections;
using System.Text;
using System.IO;
using System.Xml;
using FluorineFx.Util;

namespace FluorineFx.Json
{
	internal enum JsonType
	{
		Object,
		Array,
		None
	}

	/// <summary>
	/// Specifies the state of the <see cref="JsonWriter"/>.
	/// </summary>
	public enum WriteState
	{
		/// <summary>
		/// An exception has been thrown, which has left the <see cref="JsonWriter"/> in an invalid state.
		/// You may call the <see cref="JsonWriter.Close"/> method to put the <see cref="JsonWriter"/> in the <c>Closed</c> state.
		/// Any other <see cref="JsonWriter"/> method calls results in an <see cref="InvalidOperationException"/> being thrown. 
		/// </summary>
		Error,
		/// <summary>
		/// The <see cref="JsonWriter.Close"/> method has been called. 
		/// </summary>
		Closed,
		/// <summary>
		/// An object is being written. 
		/// </summary>
		Object,
		/// <summary>
		/// A array is being written.
		/// </summary>
		Array,
		/// <summary>
		/// A property is being written.
		/// </summary>
		Property,
		/// <summary>
		/// A write method has not been called.
		/// </summary>
		Start
	}

	/// <summary>
	/// Specifies formatting options for the <see cref="JsonWriter"/>.
	/// </summary>
	public enum Formatting
	{
		/// <summary>
		/// No special formatting is applied. This is the default.
		/// </summary>
		None,
		/// <summary>
		/// Causes child objects to be indented according to the <see cref="JsonWriter.Indentation"/> and <see cref="JsonWriter.IndentChar"/> settings.
		/// </summary>
		Indented
	}

	/// <summary>
	/// Represents a writer that provides a fast, non-cached, forward-only way of generating Json data.
	/// </summary>
	public class JsonWriter : IDisposable
	{
		private enum State
		{
			Start,
			Property,
			ObjectStart,
			Object,
			ArrayStart,
			Array,
			Closed,
			Error
		}

		// array that gives a new state based on the current state an the token being written
		private static readonly State[,] stateArray = {
														  //                      Start				PropertyName		ObjectStart			Object			ArrayStart			Array				Closed			Error
														  //						
														  /* None				*/{ State.Error,		State.Error,		State.Error,		State.Error,	State.Error,		State.Error,		State.Error,	State.Error },
														  /* StartObject		*/{ State.ObjectStart,	State.ObjectStart,	State.Error,		State.Error,	State.ObjectStart,	State.ObjectStart,  State.Error,	State.Error },
														  /* StartArray		*/{ State.ArrayStart,	State.ArrayStart,	State.Error,		State.Error,	State.ArrayStart,   State.ArrayStart,   State.Error,	State.Error },
														  /* StartProperty	*/{ State.Error,		State.Error,		State.Property,		State.Property, State.Error,		State.Error,		State.Error,	State.Error },
														  /* Comment			*/{ State.Error,		State.Property,		State.ObjectStart,	State.Object,	State.ArrayStart,	State.Array,		State.Error,	State.Error },
														  /* Value			*/{ State.Error,		State.Object,		State.Error,		State.Error,	State.Array,		State.Array,		State.Error,	State.Error },
		};

		private int _top;
		private ArrayList _stack;
		private ArrayList _serializeStack;
		private TextWriter _writer;
		private Formatting _formatting;
		private char _indentChar;
		private int _indentation;
		private char _quoteChar;
		private bool _quoteName;
		private State _currentState;

		internal ArrayList SerializeStack
		{
			get
			{
				if (_serializeStack == null)
					_serializeStack = new ArrayList();

				return _serializeStack;
			}
		}

		/// <summary>
		/// Gets the state of the writer.
		/// </summary>
		public WriteState WriteState
		{
			get
			{
				switch (_currentState)
				{
					case State.Error:
						return WriteState.Error;
					case State.Closed:
						return WriteState.Closed;
					case State.Object:
					case State.ObjectStart:
						return WriteState.Object;
					case State.Array:
					case State.ArrayStart:
						return WriteState.Array;
					case State.Property:
						return WriteState.Property;
					case State.Start:
						return WriteState.Start;
					default:
						throw new JsonWriterException("Invalid state: " + _currentState);
				}
			}
		}

		/// <summary>
		/// Indicates how the output is formatted.
		/// </summary>
		public Formatting Formatting
		{
			get { return _formatting; }
			set { _formatting = value; }
		}

		/// <summary>
		/// Gets or sets how many IndentChars to write for each level in the hierarchy when <paramref name="Formatting"/> is set to <c>Formatting.Indented</c>.
		/// </summary>
		public int Indentation
		{
			get { return _indentation; }
			set
			{
				if (value < 0)
					throw new ArgumentException("Indentation value must be greater than 0.");

				_indentation = value;
			}
		}

		/// <summary>
		/// Gets or sets which character to use to quote attribute values.
		/// </summary>
		public char QuoteChar
		{
			get { return _quoteChar; }
			set
			{
				if (value != '"' && value != '\'')
					throw new ArgumentException(@"Invalid JavaScript string quote character. Valid quote characters are ' and "".");

				_quoteChar = value;
			}
		}

		/// <summary>
		/// Gets or sets which character to use for indenting when <paramref name="Formatting"/> is set to <c>Formatting.Indented</c>.
		/// </summary>
		public char IndentChar
		{
			get { return _indentChar; }
			set { _indentChar = value; }
		}

		/// <summary>
		/// Gets or sets a value indicating whether object names will be surrounded with quotes.
		/// </summary>
		public bool QuoteName
		{
			get { return _quoteName; }
			set { _quoteName = value; }
		}

		/// <summary>
		/// Creates an instance of the <c>JsonWriter</c> class using the specified <see cref="TextWriter"/>. 
		/// </summary>
		/// <param name="textWriter">The <c>TextWriter</c> to write to.</param>
		public JsonWriter(TextWriter textWriter)
		{
			if (textWriter == null)
				throw new ArgumentNullException("textWriter");

			_writer = textWriter;
			_quoteChar = '"';
			_quoteName = true;
			_indentChar = ' ';
			_indentation = 2;
			_formatting = Formatting.None;
			_stack = new ArrayList(1);
			_stack.Add(JsonType.None);
			_currentState = State.Start;
		}

		private void Push(JsonType value)
		{
			_top++;
			if (_stack.Count <= _top)
				_stack.Add(value);
			else
				_stack[_top] = value;
		}

		private JsonType Pop()
		{
			JsonType value = Peek();
			_top--;

			return value;
		}

		private JsonType Peek()
		{
			return (JsonType)_stack[_top];
		}

		/// <summary>
		/// Flushes whatever is in the buffer to the underlying streams and also flushes the underlying stream.
		/// </summary>
		public void Flush()
		{
			_writer.Flush();
		}

		/// <summary>
		/// Closes this stream and the underlying stream.
		/// </summary>
		public void Close()
		{
			AutoCompleteAll();

			_writer.Close();
		}

		/// <summary>
		/// Writes the beginning of a Json object.
		/// </summary>
		public void WriteStartObject()
		{
			AutoComplete(JsonToken.StartObject);

			Push(JsonType.Object);

			_writer.Write("{");
		}

		/// <summary>
		/// Writes the end of a Json object.
		/// </summary>
		public void WriteEndObject()
		{
			AutoCompleteClose(JsonToken.EndObject);
		}

		/// <summary>
		/// Writes the beginning of a Json array.
		/// </summary>
		public void WriteStartArray()
		{
			AutoComplete(JsonToken.StartArray);
			Push(JsonType.Array);
			_writer.Write("[");
		}

		/// <summary>
		/// Writes the end of an array.
		/// </summary>
		public void WriteEndArray()
		{
			AutoCompleteClose(JsonToken.EndArray);
		}

		/// <summary>
		/// Writes the property name of a name/value pair on a Json object.
		/// </summary>
		/// <param name="name"></param>
		public void WritePropertyName(string name)
		{
			//_objectStack.Push(new JsonObjectInfo(JsonType.Property));
			AutoComplete(JsonToken.PropertyName);

			if (_quoteName)
				_writer.Write(_quoteChar);

			_writer.Write(name);

			if (_quoteName)
				_writer.Write(_quoteChar);

			_writer.Write(':');
		}

		/// <summary>
		/// Writes the end of the current Json object or array.
		/// </summary>
		public void WriteEnd()
		{
			WriteEnd(Peek());
		}

		private void WriteEnd(JsonType type)
		{
			switch (type)
			{
				case JsonType.Object:
					WriteEndObject();
					break;
				case JsonType.Array:
					WriteEndArray();
					break;
				default:
					throw new JsonWriterException("Unexpected type when writing end: " + type);
			}
		}

		private void AutoCompleteAll()
		{
			while (_top > 0)
			{
				WriteEnd();
			}
		}

		private JsonType GetTypeForCloseToken(JsonToken token)
		{
			switch (token)
			{
				case JsonToken.EndObject:
					return JsonType.Object;
				case JsonToken.EndArray:
					return JsonType.Array;
				default:
					throw new JsonWriterException("No type for token: " + token);
			}
		}

		private JsonToken GetCloseTokenForType(JsonType type)
		{
			switch (type)
			{
				case JsonType.Object:
					return JsonToken.EndObject;
				case JsonType.Array:
					return JsonToken.EndArray;
				default:
					throw new JsonWriterException("No close token for type: " + type);
			}
		}

		private void AutoCompleteClose(JsonToken tokenBeingClosed)
		{
			// write closing symbol and calculate new state

			int levelsToComplete = 0;

			for (int i = 0; i < _top; i++)
			{
				int currentLevel = _top - i;

				if ((JsonType)_stack[currentLevel] == GetTypeForCloseToken(tokenBeingClosed))
				{
					levelsToComplete = i + 1;
					break;
				}
			}

			if (levelsToComplete == 0)
				throw new JsonWriterException("No token to close.");

			for (int i = 0; i < levelsToComplete; i++)
			{
				JsonToken token = GetCloseTokenForType(Pop());

				if (_currentState != State.ObjectStart && _currentState != State.ArrayStart)
					WriteIndent();

				switch (token)
				{
					case JsonToken.EndObject:
						_writer.Write("}");
						break;
					case JsonToken.EndArray:
						_writer.Write("]");
						break;
					default:
						throw new JsonWriterException("Invalid JsonToken: " + token);
				}
			}

			JsonType currentLevelType = Peek();

			switch (currentLevelType)
			{
				case JsonType.Object:
					_currentState = State.Object;
					break;
				case JsonType.Array:
					_currentState = State.Array;
					break;
				case JsonType.None:
					_currentState = State.Start;
					break;
				default:
					throw new JsonWriterException("Unknown JsonType: " + currentLevelType);
			}
		}

		private void WriteIndent()
		{
			if (_formatting == Formatting.Indented)
			{
				_writer.Write(Environment.NewLine);
				// for each level of object...
				for (int i = 0; i < _top; i++)
				{
					// ...write the indent char the specified number of times
					for (int j = 0; j < _indentation; j++)
					{
						_writer.Write(_indentChar);
					}
				}
			}
		}

		private void AutoComplete(JsonToken tokenBeingWritten)
		{
			int token;

			switch (tokenBeingWritten)
			{
				default:
					token = (int)tokenBeingWritten;
					break;
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.String:
				case JsonToken.Boolean:
				case JsonToken.Null:
				case JsonToken.Undefined:
				case JsonToken.Date:
					// a value is being written
					token = 5;
					break;
			}

			// gets new state based on the current state and what is being written
			State newState = stateArray[token, (int)_currentState];

			if (newState == State.Error)
				throw new JsonWriterException(string.Format("Token {0} in state {1} would result in an invalid JavaScript object.", tokenBeingWritten.ToString(), _currentState.ToString()));

			if ((_currentState == State.Object || _currentState == State.Array) && tokenBeingWritten != JsonToken.Comment)
			{
				_writer.Write(',');
			}
			else if (_currentState == State.Property)
			{
				if (_formatting == Formatting.Indented)
					_writer.Write(' ');
			}

			if (tokenBeingWritten == JsonToken.PropertyName ||
				(WriteState == WriteState.Array))
			{
				WriteIndent();
			}

			_currentState = newState;
		}

		private void WriteValueInternal(string value, JsonToken token)
		{
			AutoComplete(token);

			_writer.Write(value);
		}

		#region WriteValue methods
		/// <summary>
		/// Writes a null value.
		/// </summary>
		public void WriteNull()
		{
			WriteValueInternal(JavaScriptConvert.Null, JsonToken.Null);
		}

		/// <summary>
		/// Writes an undefined value.
		/// </summary>
		public void WriteUndefined()
		{
			WriteValueInternal(JavaScriptConvert.Undefined, JsonToken.Undefined);
		}

		/// <summary>
		/// Writes raw JavaScript manually.
		/// </summary>
		/// <param name="javaScript">The raw JavaScript to write.</param>
		public void WriteRaw(string javaScript)
		{
			// hack. some 'raw' or 'other' token perhaps?
			WriteValueInternal(javaScript, JsonToken.Undefined);
		}

		/// <summary>
		/// Writes a <see cref="String"/> value.
		/// </summary>
		/// <param name="value">The <see cref="String"/> value to write.</param>
		public void WriteValue(string value)
		{
			WriteValueInternal(JavaScriptConvert.ToString(value, _quoteChar), JsonToken.String);
		}

		/// <summary>
		/// Writes a <see cref="Int32"/> value.
		/// </summary>
		/// <param name="value">The <see cref="Int32"/> value to write.</param>
		public void WriteValue(int value)
		{
			WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Integer);
		}

		/// <summary>
		/// Writes a <see cref="UInt32"/> value.
		/// </summary>
		/// <param name="value">The <see cref="UInt32"/> value to write.</param>
		[CLSCompliant(false)]
		public void WriteValue(uint value)
		{
			WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Integer);
		}

		/// <summary>
		/// Writes a <see cref="Int64"/> value.
		/// </summary>
		/// <param name="value">The <see cref="Int64"/> value to write.</param>
		public void WriteValue(long value)
		{
			WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Integer);
		}

		/// <summary>
		/// Writes a <see cref="UInt64"/> value.
		/// </summary>
		/// <param name="value">The <see cref="UInt64"/> value to write.</param>
		[CLSCompliant(false)]
		public void WriteValue(ulong value)
		{
			WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Integer);
		}

		/// <summary>
		/// Writes a <see cref="Single"/> value.
		/// </summary>
		/// <param name="value">The <see cref="Single"/> value to write.</param>
		public void WriteValue(float value)
		{
			WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Float);
		}

		/// <summary>
		/// Writes a <see cref="Double"/> value.
		/// </summary>
		/// <param name="value">The <see cref="Double"/> value to write.</param>
		public void WriteValue(double value)
		{
			WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Float);
		}

		/// <summary>
		/// Writes a <see cref="Boolean"/> value.
		/// </summary>
		/// <param name="value">The <see cref="Boolean"/> value to write.</param>
		public void WriteValue(bool value)
		{
			WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Boolean);
		}

		/// <summary>
		/// Writes a <see cref="Int16"/> value.
		/// </summary>
		/// <param name="value">The <see cref="Int16"/> value to write.</param>
		public void WriteValue(short value)
		{
			WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Integer);
		}

		/// <summary>
		/// Writes a <see cref="UInt16"/> value.
		/// </summary>
		/// <param name="value">The <see cref="UInt16"/> value to write.</param>
		[CLSCompliant(false)]
		public void WriteValue(ushort value)
		{
			WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Integer);
		}

		/// <summary>
		/// Writes a <see cref="Char"/> value.
		/// </summary>
		/// <param name="value">The <see cref="Char"/> value to write.</param>
		public void WriteValue(char value)
		{
			WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Integer);
		}

		/// <summary>
		/// Writes a <see cref="Byte"/> value.
		/// </summary>
		/// <param name="value">The <see cref="Byte"/> value to write.</param>
		public void WriteValue(byte value)
		{
			WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Integer);
		}

		/// <summary>
		/// Writes a <see cref="SByte"/> value.
		/// </summary>
		/// <param name="value">The <see cref="SByte"/> value to write.</param>
		[CLSCompliant(false)]
		public void WriteValue(sbyte value)
		{
			WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Integer);
		}

		/// <summary>
		/// Writes a <see cref="Decimal"/> value.
		/// </summary>
		/// <param name="value">The <see cref="Decimal"/> value to write.</param>
		public void WriteValue(decimal value)
		{
			WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Float);
		}

		/// <summary>
		/// Writes a <see cref="DateTime"/> value.
		/// </summary>
		/// <param name="value">The <see cref="DateTime"/> value to write.</param>
		public void WriteValue(DateTime value)
		{
			WriteValueInternal(JavaScriptConvert.ToString(value), JsonToken.Date);
		}
		#endregion

		/// <summary>
		/// Writes out a comment <code>/*...*/</code> containing the specified text. 
		/// </summary>
		/// <param name="text">Text to place inside the comment.</param>
		public void WriteComment(string text)
		{
			AutoComplete(JsonToken.Comment);

			_writer.Write("/*");
			_writer.Write(text);
			_writer.Write("*/");
		}

		/// <summary>
		/// Writes out the given white space.
		/// </summary>
		/// <param name="ws">The string of white space characters.</param>
		public void WriteWhitespace(string ws)
		{
			if (ws != null)
			{
				if (!StringUtils.IsWhiteSpace(ws))
					throw new JsonWriterException("Only white space characters should be used.");

				_writer.Write(ws);
			}
		}

		void IDisposable.Dispose()
		{
			Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			if (WriteState != WriteState.Closed)
				Close();
		}

        /// <summary>
        /// Writes a JSON array of JSON strings given an enumerable source
        /// of arbitrary <see cref="Object"/> values.
        /// </summary>
        public void WriteStringArray(IEnumerable values)
        {
            if (values == null)
            {
                WriteNull();
            }
            else
            {
                WriteStartArray();

                foreach (object value in values)
                {
                    if (JavaScriptConvert.IsNull(value))
                        WriteNull();
                    else
                        WriteValue(value.ToString());
                }
                WriteEndArray();
            }
        }
	}
}
