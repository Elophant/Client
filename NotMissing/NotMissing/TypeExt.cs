using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NotMissing
{
	public static class TypeExt
	{
		public static T GetAttribute<T>(this MemberInfo member) where T : Attribute
		{
			return member.GetCustomAttributes(typeof(T), false).FirstOrDefault() as T;
		}
	}
}
