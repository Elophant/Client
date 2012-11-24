using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;

namespace System.Web
{
    public sealed class HttpUtil
    {
        private class UrlDecoder
        {
            private int _bufferSize;
            private int _numChars;
            private char[] _charBuffer;
            private int _numBytes;
            private byte[] _byteBuffer;
            private Encoding _encoding;
            private void FlushBytes()
            {
                if (this._numBytes > 0)
                {
                    this._numChars += this._encoding.GetChars(this._byteBuffer, 0, this._numBytes, this._charBuffer, this._numChars);
                    this._numBytes = 0;
                }
            }
            internal UrlDecoder(int bufferSize, Encoding encoding)
            {
                this._bufferSize = bufferSize;
                this._encoding = encoding;
                this._charBuffer = new char[bufferSize];
            }
            internal void AddChar(char ch)
            {
                if (this._numBytes > 0)
                {
                    this.FlushBytes();
                }
                this._charBuffer[this._numChars++] = ch;
            }
            internal void AddByte(byte b)
            {
                if (this._byteBuffer == null)
                {
                    this._byteBuffer = new byte[this._bufferSize];
                }
                this._byteBuffer[this._numBytes++] = b;
            }
            internal string GetString()
            {
                if (this._numBytes > 0)
                {
                    this.FlushBytes();
                }
                if (this._numChars > 0)
                {
                    return new string(this._charBuffer, 0, this._numChars);
                }
                return string.Empty;
            }
        }
        private static char[] s_entityEndingChars = new char[]
		{
			';', 
			'&'
		};
        /// <summary>
        ///               Converts a string that has been HTML-encoded for HTTP transmission into a decoded string.
        ///           </summary>
        /// <returns>
        ///               A decoded string.
        ///           </returns>
        /// <param name="s">
        ///               The string to decode. 
        ///           </param>
        public static string HtmlDecode(string s)
        {
            if (s == null)
            {
                return null;
            }
            if (s.IndexOf('&') < 0)
            {
                return s;
            }
            StringBuilder stringBuilder = new StringBuilder();
            StringWriter output = new StringWriter(stringBuilder);
            HttpUtil.HtmlDecode(s, output);
            return stringBuilder.ToString();
        }
        /// <summary>
        ///               Converts a string that has been HTML-encoded into a decoded string, and sends the decoded string to a <see cref="T:System.IO.TextWriter" /> output stream.
        ///           </summary>
        /// <param name="s">
        ///               The string to decode. 
        ///           </param>
        /// <param name="output">
        ///               A <see cref="T:System.IO.TextWriter" /> stream of output. 
        ///           </param>
        public static void HtmlDecode(string s, TextWriter output)
        {
            if (s == null)
            {
                return;
            }
            if (s.IndexOf('&') < 0)
            {
                output.Write(s);
                return;
            }
            int length = s.Length;
            int i = 0;
            while (i < length)
            {
                char c = s[i];
                if (c != '&')
                {
                    goto IL_FC;
                }
                int num = s.IndexOfAny(HttpUtil.s_entityEndingChars, i + 1);
                if (num <= 0 || s[num] != ';')
                {
                    goto IL_FC;
                }
                string text = s.Substring(i + 1, num - i - 1);
                if (text.Length > 1 && text[0] == '#')
                {
                    try
                    {
                        if (text[1] == 'x' || text[1] == 'X')
                        {
                            c = (char)int.Parse(text.Substring(2), NumberStyles.AllowHexSpecifier);
                        }
                        else
                        {
                            c = (char)int.Parse(text.Substring(1));
                        }
                        i = num;
                        goto IL_FC;
                    }
                    catch (FormatException)
                    {
                        i++;
                        goto IL_FC;
                    }
                    catch (ArgumentException)
                    {
                        i++;
                        goto IL_FC;
                    }
                }
                i = num;
                char c2 = HtmlEntities.Lookup(text);
                if (c2 != '\0')
                {
                    c = c2;
                    goto IL_FC;
                }
                output.Write('&');
                output.Write(text);
                output.Write(';');
            IL_103:
                i++;
                continue;
            IL_FC:
                output.Write(c);
                goto IL_103;
            }
        }
        /// <summary>
        ///               Converts a string to an HTML-encoded string.
        ///           </summary>
        /// <returns>
        ///               An encoded string.
        ///           </returns>
        /// <param name="s">
        ///               The string to encode. 
        ///           </param>
        public static string HtmlEncode(string s)
        {
            if (s == null)
            {
                return null;
            }
            int num = HttpUtil.IndexOfHtmlEncodingChars(s, 0);
            if (num == -1)
            {
                return s;
            }
            StringBuilder stringBuilder = new StringBuilder(s.Length + 5);
            int length = s.Length;
            int num2 = 0;
            do
            {
                if (num > num2)
                {
                    stringBuilder.Append(s, num2, num - num2);
                }
                char c = s[num];
                if (c <= '>')
                {
                    char c2 = c;
                    if (c2 != '"')
                    {
                        if (c2 != '&')
                        {
                            switch (c2)
                            {
                                case '<':
                                    {
                                        stringBuilder.Append("&lt;");
                                        break;
                                    }
                                case '>':
                                    {
                                        stringBuilder.Append("&gt;");
                                        break;
                                    }
                            }
                        }
                        else
                        {
                            stringBuilder.Append("&amp;");
                        }
                    }
                    else
                    {
                        stringBuilder.Append("&quot;");
                    }
                }
                else
                {
                    stringBuilder.Append("&#");
                    StringBuilder arg_C6_0 = stringBuilder;
                    int num3 = (int)c;
                    arg_C6_0.Append(num3.ToString(NumberFormatInfo.InvariantInfo));
                    stringBuilder.Append(';');
                }
                num2 = num + 1;
                if (num2 >= length)
                {
                    goto IL_F8;
                }
                num = HttpUtil.IndexOfHtmlEncodingChars(s, num2);
            }
            while (num != -1);
            stringBuilder.Append(s, num2, length - num2);
        IL_F8:
            return stringBuilder.ToString();
        }
        /// <summary>
        ///               Converts a string into an HTML-encoded string, and returns the output as a <see cref="T:System.IO.TextWriter" /> stream of output.
        ///           </summary>
        /// <param name="s">
        ///               The string to encode 
        ///           </param>
        /// <param name="output">
        ///               A <see cref="T:System.IO.TextWriter" /> output stream. 
        ///           </param>
        public static unsafe void HtmlEncode(string s, TextWriter output)
        {
            if (s != null)
            {
                int num = IndexOfHtmlEncodingChars(s, 0x0);
                if (num == -1)
                {
                    output.Write(s);
                }
                else
                {
                    int num2 = s.Length - num;
                    fixed (char* str = s)
                    {
                        char* chPtr = str;
                        char* chPtr2 = chPtr;
                        while (num-- > 0x0)
                        {
                            chPtr2++;
                            output.Write(chPtr2[0x0]);
                        }
                        while (num2-- > 0x0)
                        {
                            chPtr2++;
                            char ch = chPtr2[0x0];
                            if (ch > '>')
                            {
                                goto Label_00C4;
                            }
                            char ch2 = ch;
                            if (ch2 != '"')
                            {
                                switch (ch2)
                                {
                                    case '<':
                                        {
                                            output.Write("&lt;");
                                            continue;
                                        }
                                    case '=':
                                        goto Label_00BA;

                                    case '>':
                                        {
                                            output.Write("&gt;");
                                            continue;
                                        }
                                    case '&':
                                        goto Label_00AD;
                                }
                                goto Label_00BA;
                            }
                            output.Write("&quot;");
                            continue;
                        Label_00AD:
                            output.Write("&amp;");
                            continue;
                        Label_00BA:
                            output.Write(ch);
                            continue;
                        Label_00C4:
                            if ((ch >= '\x00a0') && (ch < 'Ā'))
                            {
                                output.Write("&#");
                                output.Write(((int)ch).ToString(NumberFormatInfo.InvariantInfo));
                                output.Write(';');
                            }
                            else
                            {
                                output.Write(ch);
                            }
                        }
                    }
                }
            }
        }
        private static unsafe int IndexOfHtmlEncodingChars(string s, int startPos)
        {
            int num = s.Length - startPos;
            fixed (char* str = s)
            {
                char* chPtr = str;
                char* chPtr2 = chPtr + startPos;
                while (num > 0x0)
                {
                    char ch = chPtr2[0x0];
                    if (ch <= '>')
                    {
                        switch (ch)
                        {
                            case '<':
                            case '>':
                            case '"':
                            case '&':
                                return (s.Length - num);

                            case '=':
                                goto Label_007A;
                        }
                    }
                    else if ((ch >= '\x00a0') && (ch < 'Ā'))
                    {
                        return (s.Length - num);
                    }
                Label_007A:
                    chPtr2++;
                    num--;
                }
            }
            return -1;
        }

        /// <summary>
        ///               Minimally converts a string into an HTML-encoded string and sends the encoded string to a <see cref="T:System.IO.TextWriter" /> output stream.
        ///           </summary>
        /// <param name="s">
        ///               The string to encode 
        ///           </param>
        /// <param name="output">
        ///               A <see cref="T:System.IO.TextWriter" /> output stream. 
        ///           </param>
        public static unsafe void HtmlAttributeEncode(string s, TextWriter output)
        {
            if (s != null)
            {
                int num = IndexOfHtmlAttributeEncodingChars(s, 0x0);
                if (num == -1)
                {
                    output.Write(s);
                }
                else
                {
                    int num2 = s.Length - num;
                    fixed (char* str = s)
                    {
                        char* chPtr = str;
                        char* chPtr2 = chPtr;
                        while (num-- > 0x0)
                        {
                            chPtr2++;
                            output.Write(chPtr2[0x0]);
                        }
                        while (num2-- > 0x0)
                        {
                            chPtr2++;
                            char ch = chPtr2[0x0];
                            if (ch > '<')
                            {
                                goto Label_00A2;
                            }
                            char ch2 = ch;
                            if (ch2 != '"')
                            {
                                if (ch2 == '&')
                                {
                                    goto Label_008B;
                                }
                                if (ch2 != '<')
                                {
                                    goto Label_0098;
                                }
                                output.Write("&lt;");
                            }
                            else
                            {
                                output.Write("&quot;");
                            }
                            continue;
                        Label_008B:
                            output.Write("&amp;");
                            continue;
                        Label_0098:
                            output.Write(ch);
                            continue;
                        Label_00A2:
                            output.Write(ch);
                        }
                    }
                }
            }
        }
        private static unsafe int IndexOfHtmlAttributeEncodingChars(string s, int startPos)
        {
            int num = s.Length - startPos;
            fixed (char* str = s)
            {
                char* chPtr = str;
                char* chPtr2 = chPtr + startPos;
                while (num > 0x0)
                {
                    char ch = chPtr2[0x0];
                    if (ch <= '<')
                    {
                        switch (ch)
                        {
                            case '"':
                            case '&':
                            case '<':
                                return (s.Length - num);
                        }
                    }
                    chPtr2++;
                    num--;
                }
            }
            return -1;
        }
        internal static string FormatPlainTextSpacesAsHtml(string s)
        {
            if (s == null)
            {
                return null;
            }
            StringBuilder stringBuilder = new StringBuilder();
            StringWriter stringWriter = new StringWriter(stringBuilder);
            int length = s.Length;
            for (int i = 0; i < length; i++)
            {
                char c = s[i];
                if (c == ' ')
                {
                    stringWriter.Write("&nbsp;");
                }
                else
                {
                    stringWriter.Write(c);
                }
            }
            return stringBuilder.ToString();
        }
        internal static string FormatPlainTextAsHtml(string s)
        {
            if (s == null)
            {
                return null;
            }
            StringBuilder stringBuilder = new StringBuilder();
            StringWriter output = new StringWriter(stringBuilder);
            HttpUtil.FormatPlainTextAsHtml(s, output);
            return stringBuilder.ToString();
        }
        internal static void FormatPlainTextAsHtml(string s, TextWriter output)
        {
            if (s == null)
            {
                return;
            }
            int length = s.Length;
            char c = '\0';
            int i = 0;
            while (i < length)
            {
                char c2 = s[i];
                char c3 = c2;
                if (c3 <= '\r')
                {
                    if (c3 != '\n')
                    {
                        if (c3 != '\r')
                        {
                            goto IL_D2;
                        }
                    }
                    else
                    {
                        output.Write("<br>");
                    }
                }
                else
                {
                    switch (c3)
                    {
                        case ' ':
                            {
                                if (c == ' ')
                                {
                                    output.Write("&nbsp;");
                                }
                                else
                                {
                                    output.Write(c2);
                                }
                                break;
                            }
                        case '!':
                            {
                                goto IL_D2;
                            }
                        case '"':
                            {
                                output.Write("&quot;");
                                break;
                            }
                        default:
                            {
                                if (c3 != '&')
                                {
                                    switch (c3)
                                    {
                                        case '<':
                                            {
                                                output.Write("&lt;");
                                                break;
                                            }
                                        case '=':
                                            {
                                                goto IL_D2;
                                            }
                                        case '>':
                                            {
                                                output.Write("&gt;");
                                                break;
                                            }
                                        default:
                                            {
                                                goto IL_D2;
                                            }
                                    }
                                }
                                else
                                {
                                    output.Write("&amp;");
                                }
                                break;
                            }
                    }
                }
            IL_113:
                c = c2;
                i++;
                continue;
            IL_D2:
                if (c2 >= '\u00a0' && c2 < 'Ā')
                {
                    output.Write("&#");
                    int num = (int)c2;
                    output.Write(num.ToString(NumberFormatInfo.InvariantInfo));
                    output.Write(';');
                    goto IL_113;
                }
                output.Write(c2);
                goto IL_113;
            }
        }
        /// <summary>
        ///               Parses a query string into a <see cref="T:System.Collections.Specialized.NameValueCollection" /> using <see cref="P:System.Text.Encoding.UTF8" /> encoding.
        ///           </summary>
        /// <returns>
        ///               A <see cref="T:System.Collections.Specialized.NameValueCollection" /> of query parameters and values.
        ///           </returns>
        /// <param name="query">
        ///               The query string to parse.
        ///           </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="query" /> is null. 
        ///           </exception>
        public static NameValueCollection ParseQueryString(string query)
        {
            return HttpUtil.ParseQueryString(query, Encoding.UTF8);
        }
        /// <summary>
        ///               Parses a query string into a <see cref="T:System.Collections.Specialized.NameValueCollection" /> using the specified <see cref="T:System.Text.Encoding" />. 
        ///           </summary>
        /// <returns>
        ///               A <see cref="T:System.Collections.Specialized.NameValueCollection" /> of query parameters and values.
        ///           </returns>
        /// <param name="query">
        ///               The query string to parse.
        ///           </param>
        /// <param name="encoding">
        ///               The <see cref="T:System.Text.Encoding" /> to use.
        ///           </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="query" /> is null.
        ///
        ///               - or -
        ///           <paramref name="encoding" /> is null.
        ///           </exception>
        public static NameValueCollection ParseQueryString(string query, Encoding encoding)
        {
            if (query == null)
            {
                throw new ArgumentNullException("query");
            }
            if (encoding == null)
            {
                throw new ArgumentNullException("encoding");
            }
            if (query.Length > 0 && query[0] == '?')
            {
                query = query.Substring(1);
            }
            return new HttpValueCollection(query, false, true, encoding);
        }
        /// <summary>
        ///               Encodes a URL string.
        ///           </summary>
        /// <returns>
        ///               An encoded string.
        ///           </returns>
        /// <param name="str">
        ///               The text to encode. 
        ///           </param>
        public static string UrlEncode(string str)
        {
            if (str == null)
            {
                return null;
            }
            return HttpUtil.UrlEncode(str, Encoding.UTF8);
        }
        /// <summary>
        ///               Encodes the path portion of a URL string for reliable HTTP transmission from the Web server to a client.
        ///           </summary>
        /// <returns>
        ///               The URL-encoded text.
        ///           </returns>
        /// <param name="str">
        ///               The text to URL-encode. 
        ///           </param>
        public static string UrlPathEncode(string str)
        {
            if (str == null)
            {
                return null;
            }
            int num = str.IndexOf('?');
            if (num >= 0)
            {
                return HttpUtil.UrlPathEncode(str.Substring(0, num)) + str.Substring(num);
            }
            return HttpUtil.UrlEncodeSpaces(HttpUtil.UrlEncodeNonAscii(str, Encoding.UTF8));
        }
        internal static string AspCompatUrlEncode(string s)
        {
            s = HttpUtil.UrlEncode(s);
            s = s.Replace("!", "%21");
            s = s.Replace("*", "%2A");
            s = s.Replace("(", "%28");
            s = s.Replace(")", "%29");
            s = s.Replace("-", "%2D");
            s = s.Replace(".", "%2E");
            s = s.Replace("_", "%5F");
            s = s.Replace("\\", "%5C");
            return s;
        }
        /// <summary>
        ///               Encodes a URL string using the specified encoding object.
        ///           </summary>
        /// <returns>
        ///               An encoded string.
        ///           </returns>
        /// <param name="str">
        ///               The text to encode. 
        ///           </param>
        /// <param name="e">
        ///               The <see cref="T:System.Text.Encoding" /> object that specifies the encoding scheme. 
        ///           </param>
        public static string UrlEncode(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            return Encoding.ASCII.GetString(HttpUtil.UrlEncodeToBytes(str, e));
        }
        internal static string UrlEncodeNonAscii(string str, Encoding e)
        {
            if (string.IsNullOrEmpty(str))
            {
                return str;
            }
            if (e == null)
            {
                e = Encoding.UTF8;
            }
            byte[] array = e.GetBytes(str);
            array = HttpUtil.UrlEncodeBytesToBytesInternalNonAscii(array, 0, array.Length, false);
            return Encoding.ASCII.GetString(array);
        }
        internal static string UrlEncodeSpaces(string str)
        {
            if (str != null && str.IndexOf(' ') >= 0)
            {
                str = str.Replace(" ", "%20");
            }
            return str;
        }
        /// <summary>
        ///               Converts a byte array into an encoded URL string.
        ///           </summary>
        /// <returns>
        ///               An encoded string.
        ///           </returns>
        /// <param name="bytes">
        ///               The array of bytes to encode. 
        ///           </param>
        public static string UrlEncode(byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }
            return Encoding.ASCII.GetString(HttpUtil.UrlEncodeToBytes(bytes));
        }
        /// <summary>
        ///               Converts a byte array into a URL-encoded string, starting at the specified position in the array and continuing for the specified number of bytes.
        ///           </summary>
        /// <returns>
        ///               An encoded string.
        ///           </returns>
        /// <param name="bytes">
        ///               The array of bytes to encode. 
        ///           </param>
        /// <param name="offset">
        ///               The position in the byte array at which to begin encoding. 
        ///           </param>
        /// <param name="count">
        ///               The number of bytes to encode. 
        ///           </param>
        public static string UrlEncode(byte[] bytes, int offset, int count)
        {
            if (bytes == null)
            {
                return null;
            }
            return Encoding.ASCII.GetString(HttpUtil.UrlEncodeToBytes(bytes, offset, count));
        }
        /// <summary>
        ///               Converts a string into a URL-encoded array of bytes.
        ///           </summary>
        /// <returns>
        ///               An encoded array of bytes.
        ///           </returns>
        /// <param name="str">
        ///               The string to encode. 
        ///           </param>
        public static byte[] UrlEncodeToBytes(string str)
        {
            if (str == null)
            {
                return null;
            }
            return HttpUtil.UrlEncodeToBytes(str, Encoding.UTF8);
        }
        /// <summary>
        ///               Converts a string into a URL-encoded array of bytes using the specified encoding object.
        ///           </summary>
        /// <returns>
        ///               An encoded array of bytes.
        ///           </returns>
        /// <param name="str">
        ///               The string to encode 
        ///           </param>
        /// <param name="e">
        ///               The <see cref="T:System.Text.Encoding" /> that specifies the encoding scheme. 
        ///           </param>
        public static byte[] UrlEncodeToBytes(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            byte[] bytes = e.GetBytes(str);
            return HttpUtil.UrlEncodeBytesToBytesInternal(bytes, 0, bytes.Length, false);
        }
        /// <summary>
        ///               Converts an array of bytes into a URL-encoded array of bytes.
        ///           </summary>
        /// <returns>
        ///               An encoded array of bytes.
        ///           </returns>
        /// <param name="bytes">
        ///               The array of bytes to encode. 
        ///           </param>
        public static byte[] UrlEncodeToBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }
            return HttpUtil.UrlEncodeToBytes(bytes, 0, bytes.Length);
        }
        /// <summary>
        ///               Converts an array of bytes into a URL-encoded array of bytes, starting at the specified position in the array and continuing for the specified number of bytes.
        ///           </summary>
        /// <returns>
        ///               An encoded array of bytes.
        ///           </returns>
        /// <param name="bytes">
        ///               The array of bytes to encode. 
        ///           </param>
        /// <param name="offset">
        ///               The position in the byte array at which to begin encoding. 
        ///           </param>
        /// <param name="count">
        ///               The number of bytes to encode. 
        ///           </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="bytes" /> is null, but <paramref name="count" /> does not equal 0.
        ///           </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="offset" /> is less than 0 or greater than the length of the <paramref name="bytes" /> array.
        ///
        ///               - or -
        ///           <paramref name="count" /> is less than 0, or <paramref name="count" /> + <paramref name="offset" /> is greater than the length of the <paramref name="bytes" /> array.
        ///           </exception>
        public static byte[] UrlEncodeToBytes(byte[] bytes, int offset, int count)
        {
            if (bytes == null && count == 0)
            {
                return null;
            }
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if (offset < 0 || offset > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (count < 0 || offset + count > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return HttpUtil.UrlEncodeBytesToBytesInternal(bytes, offset, count, true);
        }
        /// <summary>
        ///               Converts a string into a Unicode string.
        ///           </summary>
        /// <returns>
        ///               A Unicode string in %<paramref name="UnicodeValue" /> notation.
        ///           </returns>
        /// <param name="str">
        ///               The string to convert. 
        ///           </param>
        public static string UrlEncodeUnicode(string str)
        {
            if (str == null)
            {
                return null;
            }
            return HttpUtil.UrlEncodeUnicodeStringToStringInternal(str, false);
        }
        /// <summary>
        ///               Converts a Unicode string into an array of bytes.
        ///           </summary>
        /// <returns>
        ///               A byte array.
        ///           </returns>
        /// <param name="str">
        ///               The string to convert. 
        ///           </param>
        public static byte[] UrlEncodeUnicodeToBytes(string str)
        {
            if (str == null)
            {
                return null;
            }
            return Encoding.ASCII.GetBytes(HttpUtil.UrlEncodeUnicode(str));
        }
        /// <summary>
        ///               Converts a string that has been encoded for transmission in a URL into a decoded string.
        ///           </summary>
        /// <returns>
        ///               A decoded string.
        ///           </returns>
        /// <param name="str">
        ///               The string to decode. 
        ///           </param>
        public static string UrlDecode(string str)
        {
            if (str == null)
            {
                return null;
            }
            return HttpUtil.UrlDecode(str, Encoding.UTF8);
        }
        /// <summary>
        ///               Converts a URL-encoded string into a decoded string, using the specified encoding object.
        ///           </summary>
        /// <returns>
        ///               A decoded string.
        ///           </returns>
        /// <param name="str">
        ///               The string to decode. 
        ///           </param>
        /// <param name="e">
        ///               The <see cref="T:System.Text.Encoding" /> that specifies the decoding scheme. 
        ///           </param>
        public static string UrlDecode(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            return HttpUtil.UrlDecodeStringFromStringInternal(str, e);
        }
        /// <summary>
        ///               Converts a URL-encoded byte array into a decoded string using the specified decoding object.
        ///           </summary>
        /// <returns>
        ///               A decoded string.
        ///           </returns>
        /// <param name="bytes">
        ///               The array of bytes to decode. 
        ///           </param>
        /// <param name="e">
        ///               The <see cref="T:System.Text.Encoding" /> that specifies the decoding scheme. 
        ///           </param>
        public static string UrlDecode(byte[] bytes, Encoding e)
        {
            if (bytes == null)
            {
                return null;
            }
            return HttpUtil.UrlDecode(bytes, 0, bytes.Length, e);
        }
        /// <summary>
        ///               Converts a URL-encoded byte array into a decoded string using the specified encoding object, starting at the specified position in the array, and continuing for the specified number of bytes.
        ///           </summary>
        /// <returns>
        ///               A decoded string.
        ///           </returns>
        /// <param name="bytes">
        ///               The array of bytes to decode. 
        ///           </param>
        /// <param name="offset">
        ///               The position in the byte to begin decoding. 
        ///           </param>
        /// <param name="count">
        ///               The number of bytes to decode. 
        ///           </param>
        /// <param name="e">
        ///               The <see cref="T:System.Text.Encoding" /> object that specifies the decoding scheme. 
        ///           </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="bytes" /> is null, but <paramref name="count" /> does not equal 0.
        ///           </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="offset" /> is less than 0 or greater than the length of the <paramref name="bytes" /> array.
        ///
        ///               - or -
        ///           <paramref name="count" /> is less than 0, or <paramref name="count" /> + <paramref name="offset" /> is greater than the length of the <paramref name="bytes" /> array.
        ///           </exception>
        public static string UrlDecode(byte[] bytes, int offset, int count, Encoding e)
        {
            if (bytes == null && count == 0)
            {
                return null;
            }
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if (offset < 0 || offset > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (count < 0 || offset + count > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return HttpUtil.UrlDecodeStringFromBytesInternal(bytes, offset, count, e);
        }
        /// <summary>
        ///               Converts a URL-encoded string into a decoded array of bytes.
        ///           </summary>
        /// <returns>
        ///               A decoded array of bytes.
        ///           </returns>
        /// <param name="str">
        ///               The string to decode. 
        ///           </param>
        public static byte[] UrlDecodeToBytes(string str)
        {
            if (str == null)
            {
                return null;
            }
            return HttpUtil.UrlDecodeToBytes(str, Encoding.UTF8);
        }
        /// <summary>
        ///               Converts a URL-encoded string into a decoded array of bytes using the specified decoding object.
        ///           </summary>
        /// <returns>
        ///               A decoded array of bytes.
        ///           </returns>
        /// <param name="str">
        ///               The string to decode. 
        ///           </param>
        /// <param name="e">
        ///               The <see cref="T:System.Text.Encoding" /> object that specifies the decoding scheme. 
        ///           </param>
        public static byte[] UrlDecodeToBytes(string str, Encoding e)
        {
            if (str == null)
            {
                return null;
            }
            return HttpUtil.UrlDecodeToBytes(e.GetBytes(str));
        }
        /// <summary>
        ///               Converts a URL-encoded array of bytes into a decoded array of bytes.
        ///           </summary>
        /// <returns>
        ///               A decoded array of bytes.
        ///           </returns>
        /// <param name="bytes">
        ///               The array of bytes to decode. 
        ///           </param>
        public static byte[] UrlDecodeToBytes(byte[] bytes)
        {
            if (bytes == null)
            {
                return null;
            }
            return HttpUtil.UrlDecodeToBytes(bytes, 0, (bytes != null) ? bytes.Length : 0);
        }
        /// <summary>
        ///               Converts a URL-encoded array of bytes into a decoded array of bytes, starting at the specified position in the array and continuing for the specified number of bytes.
        ///           </summary>
        /// <returns>
        ///               A decoded array of bytes.
        ///           </returns>
        /// <param name="bytes">
        ///               The array of bytes to decode. 
        ///           </param>
        /// <param name="offset">
        ///               The position in the byte array at which to begin decoding. 
        ///           </param>
        /// <param name="count">
        ///               The number of bytes to decode. 
        ///           </param>
        /// <exception cref="T:System.ArgumentNullException">
        ///   <paramref name="bytes" /> is null, but <paramref name="count" /> does not equal 0.
        ///           </exception>
        /// <exception cref="T:System.ArgumentOutOfRangeException">
        ///   <paramref name="offset" /> is less than 0 or greater than the length of the <paramref name="bytes" /> array.
        ///
        ///               - or -
        ///           <paramref name="count" /> is less than 0, or <paramref name="count" /> + <paramref name="offset" /> is greater than the length of the <paramref name="bytes" /> array.
        ///           </exception>
        public static byte[] UrlDecodeToBytes(byte[] bytes, int offset, int count)
        {
            if (bytes == null && count == 0)
            {
                return null;
            }
            if (bytes == null)
            {
                throw new ArgumentNullException("bytes");
            }
            if (offset < 0 || offset > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("offset");
            }
            if (count < 0 || offset + count > bytes.Length)
            {
                throw new ArgumentOutOfRangeException("count");
            }
            return HttpUtil.UrlDecodeBytesFromBytesInternal(bytes, offset, count);
        }
        private static byte[] UrlEncodeBytesToBytesInternal(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
        {
            int num = 0;
            int num2 = 0;
            for (int i = 0; i < count; i++)
            {
                char c = (char)bytes[offset + i];
                if (c == ' ')
                {
                    num++;
                }
                else
                {
                    if (!HttpUtil.IsSafe(c))
                    {
                        num2++;
                    }
                }
            }
            if (!alwaysCreateReturnValue && num == 0 && num2 == 0)
            {
                return bytes;
            }
            byte[] array = new byte[count + num2 * 2];
            int num3 = 0;
            for (int j = 0; j < count; j++)
            {
                byte b = bytes[offset + j];
                char c2 = (char)b;
                if (HttpUtil.IsSafe(c2))
                {
                    array[num3++] = b;
                }
                else
                {
                    if (c2 == ' ')
                    {
                        array[num3++] = 43;
                    }
                    else
                    {
                        array[num3++] = 37;
                        array[num3++] = (byte)HttpUtil.IntToHex(b >> 4 & 15);
                        array[num3++] = (byte)HttpUtil.IntToHex((int)(b & 15));
                    }
                }
            }
            return array;
        }
        private static bool IsNonAsciiByte(byte b)
        {
            return b >= 127 || b < 32;
        }
        private static byte[] UrlEncodeBytesToBytesInternalNonAscii(byte[] bytes, int offset, int count, bool alwaysCreateReturnValue)
        {
            int num = 0;
            for (int i = 0; i < count; i++)
            {
                if (HttpUtil.IsNonAsciiByte(bytes[offset + i]))
                {
                    num++;
                }
            }
            if (!alwaysCreateReturnValue && num == 0)
            {
                return bytes;
            }
            byte[] array = new byte[count + num * 2];
            int num2 = 0;
            for (int j = 0; j < count; j++)
            {
                byte b = bytes[offset + j];
                if (HttpUtil.IsNonAsciiByte(b))
                {
                    array[num2++] = 37;
                    array[num2++] = (byte)HttpUtil.IntToHex(b >> 4 & 15);
                    array[num2++] = (byte)HttpUtil.IntToHex((int)(b & 15));
                }
                else
                {
                    array[num2++] = b;
                }
            }
            return array;
        }
        private static string UrlEncodeUnicodeStringToStringInternal(string s, bool ignoreAscii)
        {
            int length = s.Length;
            StringBuilder stringBuilder = new StringBuilder(length);
            for (int i = 0; i < length; i++)
            {
                char c = s[i];
                if ((c & 'ﾀ') == '\0')
                {
                    if (ignoreAscii || HttpUtil.IsSafe(c))
                    {
                        stringBuilder.Append(c);
                    }
                    else
                    {
                        if (c == ' ')
                        {
                            stringBuilder.Append('+');
                        }
                        else
                        {
                            stringBuilder.Append('%');
                            stringBuilder.Append(HttpUtil.IntToHex((int)(c >> 4 & '\u000f')));
                            stringBuilder.Append(HttpUtil.IntToHex((int)(c & '\u000f')));
                        }
                    }
                }
                else
                {
                    stringBuilder.Append("%u");
                    stringBuilder.Append(HttpUtil.IntToHex((int)(c >> 12 & '\u000f')));
                    stringBuilder.Append(HttpUtil.IntToHex((int)(c >> 8 & '\u000f')));
                    stringBuilder.Append(HttpUtil.IntToHex((int)(c >> 4 & '\u000f')));
                    stringBuilder.Append(HttpUtil.IntToHex((int)(c & '\u000f')));
                }
            }
            return stringBuilder.ToString();
        }
        internal static string CollapsePercentUFromStringInternal(string s, Encoding e)
        {
            int length = s.Length;
            HttpUtil.UrlDecoder urlDecoder = new HttpUtil.UrlDecoder(length, e);
            int num = s.IndexOf("%u", StringComparison.Ordinal);
            if (num == -1)
            {
                return s;
            }
            int i = 0;
            while (i < length)
            {
                char c = s[i];
                if (c != '%' || i >= length - 5 || s[i + 1] != 'u')
                {
                    goto IL_C8;
                }
                int num2 = HttpUtil.HexToInt(s[i + 2]);
                int num3 = HttpUtil.HexToInt(s[i + 3]);
                int num4 = HttpUtil.HexToInt(s[i + 4]);
                int num5 = HttpUtil.HexToInt(s[i + 5]);
                if (num2 < 0 || num3 < 0 || num4 < 0 || num5 < 0)
                {
                    goto IL_C8;
                }
                c = (char)(num2 << 12 | num3 << 8 | num4 << 4 | num5);
                i += 5;
                urlDecoder.AddChar(c);
            IL_E5:
                i++;
                continue;
            IL_C8:
                if ((c & 'ﾀ') == '\0')
                {
                    urlDecoder.AddByte((byte)c);
                    goto IL_E5;
                }
                urlDecoder.AddChar(c);
                goto IL_E5;
            }
            return urlDecoder.GetString();
        }
        private static string UrlDecodeStringFromStringInternal(string s, Encoding e)
        {
            int length = s.Length;
            HttpUtil.UrlDecoder urlDecoder = new HttpUtil.UrlDecoder(length, e);
            int i = 0;
            while (i < length)
            {
                char c = s[i];
                if (c == '+')
                {
                    c = ' ';
                    goto IL_106;
                }
                if (c != '%' || i >= length - 2)
                {
                    goto IL_106;
                }
                if (s[i + 1] == 'u' && i < length - 5)
                {
                    int num = HttpUtil.HexToInt(s[i + 2]);
                    int num2 = HttpUtil.HexToInt(s[i + 3]);
                    int num3 = HttpUtil.HexToInt(s[i + 4]);
                    int num4 = HttpUtil.HexToInt(s[i + 5]);
                    if (num < 0 || num2 < 0 || num3 < 0 || num4 < 0)
                    {
                        goto IL_106;
                    }
                    c = (char)(num << 12 | num2 << 8 | num3 << 4 | num4);
                    i += 5;
                    urlDecoder.AddChar(c);
                }
                else
                {
                    int num5 = HttpUtil.HexToInt(s[i + 1]);
                    int num6 = HttpUtil.HexToInt(s[i + 2]);
                    if (num5 < 0 || num6 < 0)
                    {
                        goto IL_106;
                    }
                    byte b = (byte)(num5 << 4 | num6);
                    i += 2;
                    urlDecoder.AddByte(b);
                }
            IL_120:
                i++;
                continue;
            IL_106:
                if ((c & 'ﾀ') == '\0')
                {
                    urlDecoder.AddByte((byte)c);
                    goto IL_120;
                }
                urlDecoder.AddChar(c);
                goto IL_120;
            }
            return urlDecoder.GetString();
        }
        private static string UrlDecodeStringFromBytesInternal(byte[] buf, int offset, int count, Encoding e)
        {
            HttpUtil.UrlDecoder urlDecoder = new HttpUtil.UrlDecoder(count, e);
            int i = 0;
            while (i < count)
            {
                int num = offset + i;
                byte b = buf[num];
                if (b == 43)
                {
                    b = 32;
                    goto IL_DA;
                }
                if (b != 37 || i >= count - 2)
                {
                    goto IL_DA;
                }
                if (buf[num + 1] == 117 && i < count - 5)
                {
                    int num2 = HttpUtil.HexToInt((char)buf[num + 2]);
                    int num3 = HttpUtil.HexToInt((char)buf[num + 3]);
                    int num4 = HttpUtil.HexToInt((char)buf[num + 4]);
                    int num5 = HttpUtil.HexToInt((char)buf[num + 5]);
                    if (num2 < 0 || num3 < 0 || num4 < 0 || num5 < 0)
                    {
                        goto IL_DA;
                    }
                    char ch = (char)(num2 << 12 | num3 << 8 | num4 << 4 | num5);
                    i += 5;
                    urlDecoder.AddChar(ch);
                }
                else
                {
                    int num6 = HttpUtil.HexToInt((char)buf[num + 1]);
                    int num7 = HttpUtil.HexToInt((char)buf[num + 2]);
                    if (num6 >= 0 && num7 >= 0)
                    {
                        b = (byte)(num6 << 4 | num7);
                        i += 2;
                        goto IL_DA;
                    }
                    goto IL_DA;
                }
            IL_E1:
                i++;
                continue;
            IL_DA:
                urlDecoder.AddByte(b);
                goto IL_E1;
            }
            return urlDecoder.GetString();
        }
        private static byte[] UrlDecodeBytesFromBytesInternal(byte[] buf, int offset, int count)
        {
            int num = 0;
            byte[] array = new byte[count];
            for (int i = 0; i < count; i++)
            {
                int num2 = offset + i;
                byte b = buf[num2];
                if (b == 43)
                {
                    b = 32;
                }
                else
                {
                    if (b == 37 && i < count - 2)
                    {
                        int num3 = HttpUtil.HexToInt((char)buf[num2 + 1]);
                        int num4 = HttpUtil.HexToInt((char)buf[num2 + 2]);
                        if (num3 >= 0 && num4 >= 0)
                        {
                            b = (byte)(num3 << 4 | num4);
                            i += 2;
                        }
                    }
                }
                array[num++] = b;
            }
            if (num < array.Length)
            {
                byte[] array2 = new byte[num];
                Array.Copy(array, array2, num);
                array = array2;
            }
            return array;
        }
        private static int HexToInt(char h)
        {
            if (h >= '0' && h <= '9')
            {
                return (int)(h - '0');
            }
            if (h >= 'a' && h <= 'f')
            {
                return (int)(h - 'a' + '\n');
            }
            if (h < 'A' || h > 'F')
            {
                return -1;
            }
            return (int)(h - 'A' + '\n');
        }
        internal static char IntToHex(int n)
        {
            if (n <= 9)
            {
                return (char)(n + 48);
            }
            return (char)(n - 10 + 97);
        }
        internal static bool IsSafe(char ch)
        {
            if ((ch >= 'a' && ch <= 'z') || (ch >= 'A' && ch <= 'Z') || (ch >= '0' && ch <= '9'))
            {
                return true;
            }
            if (ch != '!')
            {
                switch (ch)
                {
                    case '\'':
                    case '(':
                    case ')':
                    case '*':
                    case '-':
                    case '.':
                        {
                            return true;
                        }
                    case '+':
                    case ',':
                        {
                            break;
                        }
                    default:
                        {
                            if (ch == '_')
                            {
                                return true;
                            }
                            break;
                        }
                }
                return false;
            }
            return true;
        }
        internal static string FormatHttpDateTime(DateTime dt)
        {
            if (dt < DateTime.MaxValue.AddDays(-1.0) && dt > DateTime.MinValue.AddDays(1.0))
            {
                dt = dt.ToUniversalTime();
            }
            return dt.ToString("R", DateTimeFormatInfo.InvariantInfo);
        }
        internal static string FormatHttpDateTimeUtc(DateTime dt)
        {
            return dt.ToString("R", DateTimeFormatInfo.InvariantInfo);
        }
        internal static string FormatHttpCookieDateTime(DateTime dt)
        {
            if (dt < DateTime.MaxValue.AddDays(-1.0) && dt > DateTime.MinValue.AddDays(1.0))
            {
                dt = dt.ToUniversalTime();
            }
            return dt.ToString("ddd, dd-MMM-yyyy HH':'mm':'ss 'GMT'", DateTimeFormatInfo.InvariantInfo);
        }

        internal class HtmlEntities
        {
            // Fields
            private static string[] _entitiesList = new string[] { 
        "\"-quot", "&-amp", "<-lt", ">-gt", "\x00a0-nbsp", "\x00a1-iexcl", "\x00a2-cent", "\x00a3-pound", "\x00a4-curren", "\x00a5-yen", "\x00a6-brvbar", "\x00a7-sect", "\x00a8-uml", "\x00a9-copy", "\x00aa-ordf", "\x00ab-laquo", 
        "\x00ac-not", "\x00ad-shy", "\x00ae-reg", "\x00af-macr", "\x00b0-deg", "\x00b1-plusmn", "\x00b2-sup2", "\x00b3-sup3", "\x00b4-acute", "\x00b5-micro", "\x00b6-para", "\x00b7-middot", "\x00b8-cedil", "\x00b9-sup1", "\x00ba-ordm", "\x00bb-raquo", 
        "\x00bc-frac14", "\x00bd-frac12", "\x00be-frac34", "\x00bf-iquest", "\x00c0-Agrave", "\x00c1-Aacute", "\x00c2-Acirc", "\x00c3-Atilde", "\x00c4-Auml", "\x00c5-Aring", "\x00c6-AElig", "\x00c7-Ccedil", "\x00c8-Egrave", "\x00c9-Eacute", "\x00ca-Ecirc", "\x00cb-Euml", 
        "\x00cc-Igrave", "\x00cd-Iacute", "\x00ce-Icirc", "\x00cf-Iuml", "\x00d0-ETH", "\x00d1-Ntilde", "\x00d2-Ograve", "\x00d3-Oacute", "\x00d4-Ocirc", "\x00d5-Otilde", "\x00d6-Ouml", "\x00d7-times", "\x00d8-Oslash", "\x00d9-Ugrave", "\x00da-Uacute", "\x00db-Ucirc", 
        "\x00dc-Uuml", "\x00dd-Yacute", "\x00de-THORN", "\x00df-szlig", "\x00e0-agrave", "\x00e1-aacute", "\x00e2-acirc", "\x00e3-atilde", "\x00e4-auml", "\x00e5-aring", "\x00e6-aelig", "\x00e7-ccedil", "\x00e8-egrave", "\x00e9-eacute", "\x00ea-ecirc", "\x00eb-euml", 
        "\x00ec-igrave", "\x00ed-iacute", "\x00ee-icirc", "\x00ef-iuml", "\x00f0-eth", "\x00f1-ntilde", "\x00f2-ograve", "\x00f3-oacute", "\x00f4-ocirc", "\x00f5-otilde", "\x00f6-ouml", "\x00f7-divide", "\x00f8-oslash", "\x00f9-ugrave", "\x00fa-uacute", "\x00fb-ucirc", 
        "\x00fc-uuml", "\x00fd-yacute", "\x00fe-thorn", "\x00ff-yuml", "Œ-OElig", "œ-oelig", "Š-Scaron", "š-scaron", "Ÿ-Yuml", "ƒ-fnof", "ˆ-circ", "˜-tilde", "Α-Alpha", "Β-Beta", "Γ-Gamma", "Δ-Delta", 
        "Ε-Epsilon", "Ζ-Zeta", "Η-Eta", "Θ-Theta", "Ι-Iota", "Κ-Kappa", "Λ-Lambda", "Μ-Mu", "Ν-Nu", "Ξ-Xi", "Ο-Omicron", "Π-Pi", "Ρ-Rho", "Σ-Sigma", "Τ-Tau", "Υ-Upsilon", 
        "Φ-Phi", "Χ-Chi", "Ψ-Psi", "Ω-Omega", "α-alpha", "β-beta", "γ-gamma", "δ-delta", "ε-epsilon", "ζ-zeta", "η-eta", "θ-theta", "ι-iota", "κ-kappa", "λ-lambda", "μ-mu", 
        "ν-nu", "ξ-xi", "ο-omicron", "π-pi", "ρ-rho", "ς-sigmaf", "σ-sigma", "τ-tau", "υ-upsilon", "φ-phi", "χ-chi", "ψ-psi", "ω-omega", "ϑ-thetasym", "ϒ-upsih", "ϖ-piv", 
        " -ensp", " -emsp", " -thinsp", "‌-zwnj", "‍-zwj", "‎-lrm", "‏-rlm", "–-ndash", "—-mdash", "‘-lsquo", "’-rsquo", "‚-sbquo", "“-ldquo", "”-rdquo", "„-bdquo", "†-dagger", 
        "‡-Dagger", "•-bull", "…-hellip", "‰-permil", "′-prime", "″-Prime", "‹-lsaquo", "›-rsaquo", "‾-oline", "⁄-frasl", "€-euro", "ℑ-image", "℘-weierp", "ℜ-real", "™-trade", "ℵ-alefsym", 
        "←-larr", "↑-uarr", "→-rarr", "↓-darr", "↔-harr", "↵-crarr", "⇐-lArr", "⇑-uArr", "⇒-rArr", "⇓-dArr", "⇔-hArr", "∀-forall", "∂-part", "∃-exist", "∅-empty", "∇-nabla", 
        "∈-isin", "∉-notin", "∋-ni", "∏-prod", "∑-sum", "−-minus", "∗-lowast", "√-radic", "∝-prop", "∞-infin", "∠-ang", "∧-and", "∨-or", "∩-cap", "∪-cup", "∫-int", 
        "∴-there4", "∼-sim", "≅-cong", "≈-asymp", "≠-ne", "≡-equiv", "≤-le", "≥-ge", "⊂-sub", "⊃-sup", "⊄-nsub", "⊆-sube", "⊇-supe", "⊕-oplus", "⊗-otimes", "⊥-perp", 
        "⋅-sdot", "⌈-lceil", "⌉-rceil", "⌊-lfloor", "⌋-rfloor", "〈-lang", "〉-rang", "◊-loz", "♠-spades", "♣-clubs", "♥-hearts", "♦-diams"
     };
            private static Hashtable _entitiesLookupTable;
            private static object _lookupLockObject = new object();

            // Methods
            private HtmlEntities()
            {
            }

            internal static char Lookup(string entity)
            {
                if (_entitiesLookupTable == null)
                {
                    lock (_lookupLockObject)
                    {
                        if (_entitiesLookupTable == null)
                        {
                            Hashtable hashtable = new Hashtable();
                            foreach (string str in _entitiesList)
                            {
                                hashtable[str.Substring(0x2)] = str[0x0];
                            }
                            _entitiesLookupTable = hashtable;
                        }
                    }
                }
                object obj2 = _entitiesLookupTable[entity];
                if (obj2 != null)
                {
                    return (char)obj2;
                }
                return '\0';
            }
        }


        [Serializable]
        internal class HttpValueCollection : NameValueCollection
        {
            // Methods
            internal HttpValueCollection()
                : base(StringComparer.OrdinalIgnoreCase)
            {
            }

            internal HttpValueCollection(int capacity)
                : base(capacity, (IEqualityComparer)StringComparer.OrdinalIgnoreCase)
            {
            }

            protected HttpValueCollection(SerializationInfo info, StreamingContext context)
                : base(info, context)
            {
            }

            internal HttpValueCollection(string str, bool readOnly, bool urlencoded, Encoding encoding)
                : base(StringComparer.OrdinalIgnoreCase)
            {
                if (!string.IsNullOrEmpty(str))
                {
                    this.FillFromString(str, urlencoded, encoding);
                }
                base.IsReadOnly = readOnly;
            }

            internal void FillFromEncodedBytes(byte[] bytes, Encoding encoding)
            {
                int num = (bytes != null) ? bytes.Length : 0x0;
                for (int i = 0x0; i < num; i++)
                {
                    string str;
                    string str2;
                    int offset = i;
                    int num4 = -1;
                    while (i < num)
                    {
                        byte num5 = bytes[i];
                        if (num5 == 0x3d)
                        {
                            if (num4 < 0x0)
                            {
                                num4 = i;
                            }
                        }
                        else if (num5 == 0x26)
                        {
                            break;
                        }
                        i++;
                    }
                    if (num4 >= 0x0)
                    {
                        str = UrlDecode(bytes, offset, num4 - offset, encoding);
                        str2 = UrlDecode(bytes, num4 + 0x1, (i - num4) - 0x1, encoding);
                    }
                    else
                    {
                        str = null;
                        str2 = UrlDecode(bytes, offset, i - offset, encoding);
                    }
                    base.Add(str, str2);
                    if ((i == (num - 0x1)) && (bytes[i] == 0x26))
                    {
                        base.Add(null, string.Empty);
                    }
                }
            }

            internal void FillFromString(string s)
            {
                this.FillFromString(s, false, null);
            }

            internal void FillFromString(string s, bool urlencoded, Encoding encoding)
            {
                int num = (s != null) ? s.Length : 0x0;
                for (int i = 0x0; i < num; i++)
                {
                    int startIndex = i;
                    int num4 = -1;
                    while (i < num)
                    {
                        char ch = s[i];
                        if (ch == '=')
                        {
                            if (num4 < 0x0)
                            {
                                num4 = i;
                            }
                        }
                        else if (ch == '&')
                        {
                            break;
                        }
                        i++;
                    }
                    string str = null;
                    string str2 = null;
                    if (num4 >= 0x0)
                    {
                        str = s.Substring(startIndex, num4 - startIndex);
                        str2 = s.Substring(num4 + 0x1, (i - num4) - 0x1);
                    }
                    else
                    {
                        str2 = s.Substring(startIndex, i - startIndex);
                    }
                    if (urlencoded)
                    {
                        base.Add(UrlDecode(str, encoding), UrlDecode(str2, encoding));
                    }
                    else
                    {
                        base.Add(str, str2);
                    }
                    if ((i == (num - 0x1)) && (s[i] == '&'))
                    {
                        base.Add(null, string.Empty);
                    }
                }
            }

            internal void MakeReadOnly()
            {
                base.IsReadOnly = true;
            }

            internal void MakeReadWrite()
            {
                base.IsReadOnly = false;
            }

            internal void Reset()
            {
                base.Clear();
            }

            public override string ToString()
            {
                return this.ToString(true);
            }

            internal virtual string ToString(bool urlencoded)
            {
                return this.ToString(urlencoded, null);
            }

            internal virtual string ToString(bool urlencoded, IDictionary excludeKeys)
            {
                int count = this.Count;
                if (count == 0x0)
                {
                    return string.Empty;
                }
                StringBuilder builder = new StringBuilder();
                bool flag = (excludeKeys != null) && (excludeKeys["__VIEWSTATE"] != null);
                for (int i = 0x0; i < count; i++)
                {
                    string key = this.GetKey(i);
                    if (((!flag || (key == null)) || !key.StartsWith("__VIEWSTATE", StringComparison.Ordinal)) && (((excludeKeys == null) || (key == null)) || (excludeKeys[key] == null)))
                    {
                        string str3;
                        if (urlencoded)
                        {
                            key = UrlEncodeUnicode(key);
                        }
                        string str2 = !string.IsNullOrEmpty(key) ? (key + "=") : string.Empty;
                        ArrayList list = (ArrayList)base.BaseGet(i);
                        int num3 = (list != null) ? list.Count : 0x0;
                        if (builder.Length > 0x0)
                        {
                            builder.Append('&');
                        }
                        if (num3 == 0x1)
                        {
                            builder.Append(str2);
                            str3 = (string)list[0x0];
                            if (urlencoded)
                            {
                                str3 = UrlEncodeUnicode(str3);
                            }
                            builder.Append(str3);
                        }
                        else if (num3 == 0x0)
                        {
                            builder.Append(str2);
                        }
                        else
                        {
                            for (int j = 0x0; j < num3; j++)
                            {
                                if (j > 0x0)
                                {
                                    builder.Append('&');
                                }
                                builder.Append(str2);
                                str3 = (string)list[j];
                                if (urlencoded)
                                {
                                    str3 = UrlEncodeUnicode(str3);
                                }
                                builder.Append(str3);
                            }
                        }
                    }
                }
                return builder.ToString();
            }
        }
    }


}
