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
using FluorineFx.Messaging.Api.Messaging;

namespace FluorineFx.Messaging.Api.Stream.Support
{
    /// <summary>
    /// Simple playlist item implementation.
    /// </summary>
    [CLSCompliant(false)]
    public class SimplePlayItem : IPlayItem
    {
        private long _length = -1;
        private string _name;
        /// <summary>
        /// Start mark.
        /// </summary>
        private long _start = -2;
        /// <summary>
        /// Message source
        /// </summary>
        private IMessageInput _msgInput;

        #region IPlayItem Members

        /// <summary>
        /// Gets item name.
        /// </summary>
        public string Name
        {
            get { return _name; }
            set { _name = value; }
        }
        /// <summary>
        /// Gets or sets start position.
        /// </summary>
        public long Start
        {
            get { return _start; }
            set { _start = value; }
        }

        /// <summary>
        /// Gets play item length in milliseconds.
        /// </summary>
        public long Length
        {
            get { return _length; }
            set { _length = value; }
        }
        /// <summary>
        /// Gets or sets the message input source.
        /// </summary>
        public IMessageInput MessageInput
        {
            get { return _msgInput; }
            set { _msgInput = value; }
        }

        #endregion
    }
}
