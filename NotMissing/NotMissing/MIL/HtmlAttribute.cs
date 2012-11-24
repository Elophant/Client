using System;
using System.Collections;
using System.ComponentModel;

namespace MIL.Html
{
	/// <summary>
	/// The HtmlAttribute object represents a named value associated with an HtmlElement.
	/// </summary>
	public class HtmlAttribute
	{
		protected string mName;
		protected string mValue;

		public HtmlAttribute()
		{
			mName = "Unnamed";
			mValue = "";
		}

		/// <summary>
		/// This constructs an HtmlAttribute object with the given name and value. For wierd
		/// HTML attributes that don't have a value (e.g. "NOWRAP"), specify null as the value.
		/// </summary>
		/// <param name="name">The name of the attribute</param>
		/// <param name="value">The value of the attribute</param>
		public HtmlAttribute(string name,string value)
		{
			mName = name;
			mValue = value;
		}

		/// <summary>
		/// The name of the attribute. e.g. WIDTH
		/// </summary>
		[
			Category("General"),
			Description("The name of the attribute")
		]
		public string Name
		{
			get
			{
				return mName;
			}
			set
			{
				mName = value;
			}
		}

		/// <summary>
		/// The value of the attribute. e.g. 100%
		/// </summary>
		[
			Category("General"),
			Description("The value of the attribute")
		]
		public string Value
		{
			get
			{
				return mValue;
			}
			set
			{
				mValue = value;
			}
		}

		/// <summary>
		/// This will return an HTML-formatted version of this attribute. NB. This is
		/// not SGML or XHTML safe, as it caters for null-value attributes such as "NOWRAP".
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			if( mValue == null )
			{
				return mName;
			}
			else
			{
				return mName + "=\"" + mValue + "\"";
			}
		}

		[
			Category("Output"),
			Description("The HTML to represent this attribute")
		]
		public string HTML
		{
			get
			{
				if( mValue == null )
				{
					return mName;
				}
				else
				{
					return mName + "=\"" + HtmlEncoder.EncodeValue( mValue ) + "\"";
				}
			}
		}

		[
			Category("Output"),
			Description("The XHTML to represent this attribute")
		]
		public string XHTML
		{
			get
			{
				if( mValue == null )
				{
					return mName.ToLower();
				}
				else
				{
					return mName + "=\"" + HtmlEncoder.EncodeValue( mValue.ToLower() ) + "\"";
				}
			}
		}
	}

	/// <summary>
	/// This is a collection of attributes. Typically, this is associated with a particular
	/// element. This collection is searchable by both the index and the name of the attribute.
	/// </summary>
	public class HtmlAttributeCollection: CollectionBase
	{
		HtmlElement mElement;

		public HtmlAttributeCollection()
		{
			mElement = null;
		}

		/// <summary>
		/// This will create an empty collection of attributes.
		/// </summary>
		/// <param name="element"></param>
		internal HtmlAttributeCollection(HtmlElement element)
		{
			mElement = element;
		}

		/// <summary>
		/// This will add an element to the collection.
		/// </summary>
		/// <param name="attribute">The attribute to add.</param>
		/// <returns>The index at which it was added.</returns>
		public int Add(HtmlAttribute attribute)
		{
			return base.List.Add( attribute );
		}

		/// <summary>
		/// This provides direct access to an attribute in the collection by its index.
		/// </summary>
		public HtmlAttribute this [int index]
		{
			get
			{
				return (HtmlAttribute) base.List[ index ];
			}
			set
			{
				base.List[ index ] = value;
			}
		}

		/// <summary>
		/// This will search the collection for the named attribute. If it is not found, this
		/// will return null.
		/// </summary>
		/// <param name="name">The name of the attribute to find.</param>
		/// <returns>The attribute, or null if it wasn't found.</returns>
		public HtmlAttribute FindByName(string name)
		{
			int index = IndexOf( name );
			if( index == -1 )
			{
				return null;
			}
			else
			{
				return this[ IndexOf( name ) ];
			}
		}

		/// <summary>
		/// This will return the index of the attribute with the specified name. If it is
		/// not found, this method will return -1.
		/// </summary>
		/// <param name="name">The name of the attribute to find.</param>
		/// <returns>The zero-based index, or -1.</returns>
		public int IndexOf(string name)
		{
			for( int index = 0 ; index < this.List.Count ; index++ )
			{
				if( this[ index ].Name.ToLower().Equals( name.ToLower() ) )
				{
					return index;
				}
			}
			return -1;
		}

		/// <summary>
		/// This overload allows you to have direct access to an attribute by providing
		/// its name. If the attribute does not exist, null is returned.
		/// </summary>
		public HtmlAttribute this [string name]
		{
			get
			{
				return FindByName( name );
			}
		}

        /// <summary>
        /// Get the value of the attibute with the specified name.
        /// </summary>
        /// <param name="name">Name of attribute to get.</param>
        /// <returns>Value of attribute or string.Empty</returns>
        public string GetAttributeValue(string name)
        {
            for (int index = 0; index < this.List.Count; index++)
            {
                if (this[index].Name.ToLower().Equals(name.ToLower()))
                {
                    return this[index].Value;
                }
            }
            return string.Empty;
        }

	}
}
