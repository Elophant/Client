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
using System.Net;
using System.Net.Sockets;

namespace FluorineFx.Util
{
    /// <summary>
    /// Network utility class.
    /// </summary>
    public abstract class NetworkUtils
    {
        private NetworkUtils() { }

        protected static bool SetSocketOption(Socket socket, SocketOptionLevel level, SocketOptionName name, bool value)
        {
            if (((int)socket.GetSocketOption(level, name)) == (value ? 1 : 0))
                return value;
            socket.SetSocketOption(level, name, value);
            if (((int)socket.GetSocketOption(level, name)) != 1)
            {
                return false;
            }
            return true;
        }

        protected static int SetSocketOption(Socket socket, SocketOptionLevel level, SocketOptionName name, int value)
        {
            if (((int)socket.GetSocketOption(level, name)) == value)
                return value;
            socket.SetSocketOption(level, name, value);
            return (int)socket.GetSocketOption(level, name);
        }

 

    }
}
