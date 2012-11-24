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

namespace FluorineFx.Json
{
	public abstract class JsonConverter
	{
		public virtual void WriteJson(JsonWriter writer, object value)
		{
			JsonSerializer serializer = new JsonSerializer();

			serializer.Serialize(writer, value);
		}

		public virtual object ReadJson(JsonReader reader, Type objectType)
		{
			throw new NotImplementedException(string.Format("{0} has not overriden FromJson method.", GetType().Name));
		}

		public abstract bool CanConvert(Type objectType);
	}
}
