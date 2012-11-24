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
using System.Globalization;

namespace FluorineFx.Json
{
	/// <summary>
	/// Represents a reader that provides fast, non-cached, forward-only access to serialized Json data.
	/// </summary>
	public class JsonReader : IDisposable
	{
		private enum State
		{
			Start,
			Complete,
			Property,
			ObjectStart,
			Object,
			ArrayStart,
			Array,
			Closed,
			PostValue,
			Constructor,
			ConstructorEnd,
			Error,
			Finished
		}

		private TextReader _reader;
		private char _currentChar;

		// current Token data
		private JsonToken _token;
		private object _value;
		private Type _valueType;
		private char _quoteChar;
		private StringBuffer _buffer;
		//private StringBuilder _testBuffer;
		private State _currentState;

		private int _top;
		private ArrayList _stack;//List<JsonType>

		/// <summary>
		/// Gets the quotation mark character used to enclose the value of a string.
		/// </summary>
		public char QuoteChar
		{
			get { return _quoteChar; }
		}

		/// <summary>
		/// Gets the type of the current Json token. 
		/// </summary>
		public JsonToken TokenType
		{
			get { return _token; }
		}

		/// <summary>
		/// Gets the text value of the current Json token.
		/// </summary>
		public object Value
		{
			get { return _value; }
		}

		/// <summary>
		/// Gets The Common Language Runtime (CLR) type for the current Json token.
		/// </summary>
		public Type ValueType
		{
			get { return _valueType; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="JsonReader"/> class with the specified <see cref="TextReader"/>.
		/// </summary>
		/// <param name="reader">The <c>TextReader</c> containing the XML data to read.</param>
		public JsonReader(TextReader reader)
		{
			if (reader == null)
				throw new ArgumentNullException("reader");

			_reader = reader;
			_buffer = new StringBuffer(4096);
			//_testBuffer = new StringBuilder();
			_currentState = State.Start;
			_stack = new ArrayList();
			_top = 0;
			Push(JsonType.None);
		}

		private void Push(JsonType value)
		{
			_stack.Add(value);
			_top++;
		}

		private JsonType Pop()
		{
			JsonType value = Peek();
			_stack.RemoveAt(_stack.Count - 1);
			_top--;

			return value;
		}

		private JsonType Peek()
		{
			return (JsonType)_stack[_top - 1];
		}

		private void ParseString(char quote)
		{
			bool stringTerminated = false;

			while (!stringTerminated && MoveNext())
			{
				switch (_currentChar)
				{
						//case 0:
						//case 0x0A:
						//case 0x0D:
						//    throw new JsonReaderException("Unterminated string");
					case '\\':
						if (MoveNext())
						{
							switch (_currentChar)
							{
								case 'b':
									_buffer.Append('\b');
									break;
								case 't':
									_buffer.Append('\t');
									break;
								case 'n':
									_buffer.Append('\n');
									break;
								case 'f':
									_buffer.Append('\f');
									break;
								case 'r':
									_buffer.Append('\r');
									break;
								case 'u':
									//_buffer.Append((char) Integer.parseInt(next(4), 16));
									break;
								case 'x':
									//_buffer.Append((char) Integer.parseInt(next(2), 16));
									break;
								default:
									_buffer.Append(_currentChar);
									break;
							}
						}
						else
						{
							throw new JsonReaderException("Unterminated string. Expected delimiter: " + quote);
						}
						break;
					case '"':
					case '\'':
						if (_currentChar == quote)
							stringTerminated = true;
						else
							goto default;
						break;
					default:
						_buffer.Append(_currentChar);
						break;
				}
			}

			if (!stringTerminated)
				throw new JsonReaderException("Unterminated string. Expected delimiter: " + quote);

			ClearCurrentChar();
			_currentState = State.PostValue;
			_token = JsonToken.String;
			_value = _buffer.ToString();
			_buffer.Position = 0;
			_valueType = typeof(string);
			_quoteChar = quote;
		}

		private bool MoveNext()
		{
			int value = _reader.Read();

			if (value != -1)
			{
				_currentChar = (char)value;
				//_testBuffer.Append(_currentChar);
				return true;
			}
			else
			{
				return false;
			}
		}

		private bool HasNext()
		{
			return (_reader.Peek() != -1);
		}

		private char PeekNext()
		{
			return (char)_reader.Peek();
		}

		private void ClearCurrentChar()
		{
			_currentChar = '\0';
		}

		private bool MoveTo(char value)
		{
			while (MoveNext())
			{
				if (_currentChar == value)
					return true;
			}
			return false;
		}

		/// <summary>
		/// Reads the next Json token from the stream.
		/// </summary>
		/// <returns></returns>
		public bool Read()
		{
			while (true)
			{
				if (_currentChar == '\0')
				{
					if (!MoveNext())
						return false;
				}

				switch (_currentState)
				{
					case State.Start:
					case State.Property:
					case State.Array:
					case State.ArrayStart:
						return ParseValue();
					case State.Complete:
						break;
					case State.Object:
					case State.ObjectStart:
						return ParseObject();
					case State.PostValue:
						// returns true if it hits
						// end of object or array
						if (ParsePostValue())
							return true;
						break;
					case State.Closed:
						break;
					case State.Error:
						break;
					default:
						throw new JsonReaderException("Unexpected state: " + _currentState);
				}
			}
		}

		private bool ParsePostValue()
		{
			do
			{
				switch (_currentChar)
				{
					case '}':
						SetToken(JsonToken.EndObject);
						ClearCurrentChar();
						return true;
					case ']':
						SetToken(JsonToken.EndArray);
						ClearCurrentChar();
						return true;
					case '/':
						ParseComment();
						return true;
					case ',':
						// finished paring
						SetStateBasedOnCurrent();
						ClearCurrentChar();
						return false;
					default:
						if (char.IsWhiteSpace(_currentChar))
						{
							// eat whitespace
							ClearCurrentChar();
						}
						else
						{
							throw new JsonReaderException("After parsing a value an unexpected character was encoutered: " + _currentChar);
						}
						break;
				}
			} while (MoveNext());

			return false;
		}

		private bool ParseObject()
		{
			do
			{
				switch (_currentChar)
				{
					case '}':
						SetToken(JsonToken.EndObject);
						return true;
					case '/':
						ParseComment();
						return true;
					case ',':
						SetToken(JsonToken.Undefined);
						return true;
					default:
						if (char.IsWhiteSpace(_currentChar))
						{
							// eat
						}
						else
						{
							return ParseProperty();
						}
						break;
				}
			} while (MoveNext());

			return false;
		}

		private bool ParseProperty()
		{
			if (ValidIdentifierChar(_currentChar))
			{
				ParseUnquotedProperty();
			}
			else if (_currentChar == '"' || _currentChar == '\'')
			{
				ParseQuotedProperty(_currentChar);
			}
			else
			{
				throw new JsonReaderException("Invalid property identifier character: " + _currentChar);
			}

			// finished property. move to colon
			if (_currentChar != ':')
			{
				MoveTo(':');
			}

			SetToken(JsonToken.PropertyName, _buffer.ToString());
			_buffer.Position = 0;

			return true;
		}

		private void ParseQuotedProperty(char quoteChar)
		{
			// parse property name until quoted char is hit
			while (MoveNext())
			{
				if (_currentChar == quoteChar)
				{
					return;
				}
				else
				{
					_buffer.Append(_currentChar);
				}
			}

			throw new JsonReaderException("Unclosed quoted property. Expected: " + quoteChar);
		}

		private bool ValidIdentifierChar(char value)
		{
			return (char.IsLetterOrDigit(_currentChar) || _currentChar == '_' || _currentChar == '$');
		}

		private void ParseUnquotedProperty()
		{
			// parse unquoted property name until whitespace or colon
			_buffer.Append(_currentChar);

			while (MoveNext())
			{
				if (char.IsWhiteSpace(_currentChar) || _currentChar == ':')
				{
					break;
				}
				else if (ValidIdentifierChar(_currentChar))
				{
					_buffer.Append(_currentChar);
				}
				else
				{
					throw new JsonReaderException("Invalid JavaScript property identifier character: " + _currentChar);
				}
			}
		}

		private void SetToken(JsonToken newToken)
		{
			SetToken(newToken, null);
		}

		private void SetToken(JsonToken newToken, object value)
		{
			_token = newToken;

			switch (newToken)
			{
				case JsonToken.StartObject:
					_currentState = State.ObjectStart;
					Push(JsonType.Object);
					ClearCurrentChar();
					break;
				case JsonToken.StartArray:
					_currentState = State.ArrayStart;
					Push(JsonType.Array);
					ClearCurrentChar();
					break;
				case JsonToken.EndObject:
					ValidateEnd(JsonToken.EndObject);
					ClearCurrentChar();
					_currentState = State.PostValue;
					break;
				case JsonToken.EndArray:
					ValidateEnd(JsonToken.EndArray);
					ClearCurrentChar();
					_currentState = State.PostValue;
					break;
				case JsonToken.PropertyName:
					_currentState = State.Property;
					ClearCurrentChar();
					break;
				case JsonToken.Undefined:
				case JsonToken.Integer:
				case JsonToken.Float:
				case JsonToken.Boolean:
				case JsonToken.Null:
				case JsonToken.Constructor:
				case JsonToken.Date:
					_currentState = State.PostValue;
					break;
			}

			if (value != null)
			{
				_value = value;
				_valueType = value.GetType();
			}
			else
			{
				_value = null;
				_valueType = null;
			}
		}

		private bool ParseValue()
		{
			do
			{
				switch (_currentChar)
				{
					case '"':
					case '\'':
						ParseString(_currentChar);
						return true;
					case 't':
						ParseTrue();
						return true;
					case 'f':
						ParseFalse();
						return true;
					case 'n':
						if (HasNext())
						{
							char next = PeekNext();

							if (next == 'u')
								ParseNull();
							else if (next == 'e')
								ParseConstructor();
							else
								throw new JsonReaderException("Unexpected character encountered while parsing value: " + _currentChar);
						}
						else
						{
							throw new JsonReaderException("Unexpected end");
						}
						return true;
					case '/':
						ParseComment();
						return true;
					case 'u':
						ParseUndefined();
						return true;
					case '{':
						SetToken(JsonToken.StartObject);
						return true;
					case '[':
						SetToken(JsonToken.StartArray);
						return true;
					case '}':
						SetToken(JsonToken.EndObject);
						return true;
					case ']':
						SetToken(JsonToken.EndArray);
						return true;
					case ',':
						SetToken(JsonToken.Undefined);
						//ClearCurrentChar();
						return true;
					case ')':
						if (_currentState == State.Constructor)
						{
							_currentState = State.ConstructorEnd;
							return false;
						}
						else
						{
							throw new JsonReaderException("Unexpected character encountered while parsing value: " + _currentChar);
						}
					default:
						if (char.IsWhiteSpace(_currentChar))
						{
							// eat
						}
						else if (char.IsNumber(_currentChar) || _currentChar == '-' || _currentChar == '.')
						{
							ParseNumber();
							return true;
						}
						else
						{
							throw new JsonReaderException("Unexpected character encountered while parsing value: " + _currentChar);
						}
						break;
				}
			} while (MoveNext());

			return false;
		}

		private bool EatWhitespace(bool oneOrMore)
		{
			bool whitespace = false;
			while (char.IsWhiteSpace(_currentChar))
			{
				whitespace = true;
				MoveNext();
			}

			return (!oneOrMore || whitespace);
		}

		private void ParseConstructor()
		{
			if (MatchValue("new", true))
			{
				if (EatWhitespace(true))
				{
					while (char.IsLetter(_currentChar))
					{
						_buffer.Append(_currentChar);
						MoveNext();
					}

					string constructorName = _buffer.ToString();
					_buffer.Position = 0;

					ArrayList parameters = new ArrayList();

					EatWhitespace(false);

					if (_currentChar == '(' && MoveNext())
					{
						_currentState = State.Constructor;

						while (ParseValue())
						{
							parameters.Add(_value);
							_currentState = State.Constructor;
						}

						if (string.CompareOrdinal(constructorName, "Date") == 0)
						{
							long javaScriptTicks = Convert.ToInt64(parameters[0]);

							DateTime date = JavaScriptConvert.ConvertJavaScriptTicksToDateTime(javaScriptTicks);

							SetToken(JsonToken.Date, date);
						}
						else
						{
							JavaScriptConstructor constructor = new JavaScriptConstructor(constructorName, new JavaScriptParameters(parameters));

							if (_currentState == State.ConstructorEnd)
							{
								SetToken(JsonToken.Constructor, constructor);
							}
						}

						// move past ')'
						MoveNext();
					}
				}
			}
		}

		private void ParseNumber()
		{
			// parse until seperator character or end
			bool end = false;
			do
			{
				if (CurrentIsSeperator())
					end = true;
				else
					_buffer.Append(_currentChar);

			} while (!end && MoveNext());

			string number = _buffer.ToString();
			object numberValue;
			JsonToken numberType;

			if (number.IndexOf('.') == -1)
			{
				numberValue = Convert.ToInt64(_buffer.ToString(), CultureInfo.InvariantCulture);
				numberType = JsonToken.Integer;
			}
			else
			{
				numberValue = Convert.ToDouble(_buffer.ToString(), CultureInfo.InvariantCulture);
				numberType = JsonToken.Float;
			}

			_buffer.Position = 0;

			SetToken(numberType, numberValue);
		}

		private void ValidateEnd(JsonToken endToken)
		{
			JsonType currentObject = Pop();

			if (GetTypeForCloseToken(endToken) != currentObject)
				throw new JsonReaderException(string.Format("JsonToken {0} is not valid for closing JsonType {1}.", endToken, currentObject));
		}

		private void SetStateBasedOnCurrent()
		{
			JsonType currentObject = Peek();

			switch (currentObject)
			{
				case JsonType.Object:
					_currentState = State.Object;
					break;
				case JsonType.Array:
					_currentState = State.Array;
					break;
				case JsonType.None:
					_currentState = State.Finished;
					break;
				default:
					throw new JsonReaderException("While setting the reader state back to current object an unexpected JsonType was encountered: " + currentObject);
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
					throw new JsonReaderException("Not a valid close JsonToken: " + token);
			}
		}

		private void ParseComment()
		{
			// should have already parsed / character before reaching this method

			MoveNext();

			if (_currentChar == '*')
			{
				while (MoveNext())
				{
					if (_currentChar == '*')
					{
						if (MoveNext())
						{
							if (_currentChar == '/')
							{
								break;
							}
							else
							{
								_buffer.Append('*');
								_buffer.Append(_currentChar);
							}
						}
					}
					else
					{
						_buffer.Append(_currentChar);
					}
				}
			}
			else
			{
				throw new JsonReaderException("Error parsing comment. Expected: *");
			}

			SetToken(JsonToken.Comment, _buffer.ToString());

			_buffer.Position = 0;

			ClearCurrentChar();
		}

		private bool MatchValue(string value)
		{
			int i = 0;
			do
			{
				if (_currentChar != value[i])
				{
					break;
				}
				i++;
			}
			while (i < value.Length && MoveNext());

			return (i == value.Length);
		}

		private bool MatchValue(string value, bool noTrailingNonSeperatorCharacters)
		{
			// will match value and then move to the next character, checking that it is a seperator character
			bool match = MatchValue(value);

			if (!noTrailingNonSeperatorCharacters)
				return match;
			else
				return (match && (!MoveNext() || CurrentIsSeperator()));
		}

		private bool CurrentIsSeperator()
		{
			switch (_currentChar)
			{
				case '}':
				case ']':
				case ',':
					return true;
				case '/':
					// check next character to see if start of a comment
					return (HasNext() && PeekNext() == '*');
				case ')':
					if (_currentState == State.Constructor)
						return true;
					break;
				default:
					if (char.IsWhiteSpace(_currentChar))
						return true;
					break;
			}

			return false;
		}

		private void ParseTrue()
		{
			// check characters equal 'true'
			// and that it is followed by either a seperator character
			// or the text ends
			if (MatchValue(JavaScriptConvert.True, true))
			{
				SetToken(JsonToken.Boolean, true);
			}
			else
			{
				throw new JsonReaderException("Error parsing boolean value.");
			}
		}

		private void ParseNull()
		{
			if (MatchValue(JavaScriptConvert.Null, true))
			{
				SetToken(JsonToken.Null);
			}
			else
			{
				throw new JsonReaderException("Error parsing null value.");
			}
		}

		private void ParseUndefined()
		{
			if (MatchValue(JavaScriptConvert.Undefined, true))
			{
				SetToken(JsonToken.Undefined);
			}
			else
			{
				throw new JsonReaderException("Error parsing undefined value.");
			}
		}

		private void ParseFalse()
		{
			if (MatchValue(JavaScriptConvert.False, true))
			{
				SetToken(JsonToken.Boolean, false);
			}
			else
			{
				throw new JsonReaderException("Error parsing boolean value.");
			}
		}

		void IDisposable.Dispose()
		{
			Dispose(true);
		}

		private void Dispose(bool disposing)
		{
			if (_currentState != State.Closed && disposing)
				Close();
		}

		/// <summary>
		/// Changes the <see cref="State"/> to Closed. 
		/// </summary>
		public void Close()
		{
			_currentState = State.Closed;
			_token = JsonToken.None;
			_value = null;
			_valueType = null;

			if (_reader != null)
				_reader.Close();

			if (_buffer != null)
				_buffer.Clear();
		}
	}
}
