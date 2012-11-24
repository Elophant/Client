using System;
using System.Text;
using System.Collections;
using System.ComponentModel;

namespace MIL.Html
{
	/// <summary>
	/// This is the basic HTML document object used to represent a sequence of HTML.
	/// </summary>
	public class HtmlDocument
	{
		HtmlNodeCollection mNodes = new HtmlNodeCollection(null);
		private string mXhtmlHeader = "<!DOCTYPE html PUBLIC \"-//W3C//DTD XHTML 1.0 Strict//EN\" \"http://www.w3.org/TR/xhtml1/DTD/xhtml1-strict.dtd\">";

		/// <summary>
		/// This will create a new document object by parsing the HTML specified.
		/// </summary>
		/// <param name="html">The HTML to parse.</param>
		internal HtmlDocument(string html,bool wantSpaces)
		{
			HtmlParser parser = new HtmlParser();
			parser.RemoveEmptyElementText = !wantSpaces;
			mNodes = parser.Parse( html );
		}

		[
			Category("General"),
			Description("This is the DOCTYPE for XHTML production")
		]
		public string DocTypeXHTML
		{
			get
			{
				return mXhtmlHeader;
			}
			set
			{
				mXhtmlHeader = value;
			}
		}

		/// <summary>
		/// This is the collection of nodes used to represent this document.
		/// </summary>
		public HtmlNodeCollection Nodes
		{
			get
			{
				return mNodes;
			}
		}

		/// <summary>
		/// This will create a new document object by parsing the HTML specified.
		/// </summary>
		/// <param name="html">The HTML to parse.</param>
		/// <returns>An instance of the newly created object.</returns>
		public static HtmlDocument Create(string html)
		{
			return new HtmlDocument( html , false );
		}

		/// <summary>
		/// This will create a new document object by parsing the HTML specified.
		/// </summary>
		/// <param name="html">The HTML to parse.</param>
		/// <param name="wantSpaces">Set this to true if you want to preserve all whitespace from the input HTML</param>
		/// <returns>An instance of the newly created object.</returns>
		public static HtmlDocument Create(string html,bool wantSpaces)
		{
			return new HtmlDocument( html , wantSpaces );
		}

		/// <summary>
		/// This will return the HTML used to represent this document.
		/// </summary>
		[
			Category("Output"),
			Description("The HTML version of this document")
		]
		public string HTML
		{
			get
			{
				StringBuilder html = new StringBuilder();
				foreach( HtmlNode node in Nodes )
				{
					html.Append( node.HTML );
				}
				return html.ToString();
			}
		}

		/// <summary>
		/// This will return the XHTML document used to represent this document.
		/// </summary>
		[
			Category("Output"),
			Description("The XHTML version of this document")
		]
		public string XHTML
		{
			get
			{
				StringBuilder html = new StringBuilder();
				if( mXhtmlHeader != null )
				{
					html.Append( mXhtmlHeader );
					html.Append( "\r\n" );
				}
				foreach( HtmlNode node in Nodes )
				{
					html.Append( node.XHTML );
				}
				return html.ToString();
			}
		}
	}
}
