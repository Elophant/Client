using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Text;

namespace NotMissing
{
	/// <summary>
	/// Console methods with color parameters.
	/// </summary>
	public static class CConsole
	{
		public static void Write(ConsoleColor fore, String format, params Object[] arg)
		{
			var f = Console.ForegroundColor;
			Console.ForegroundColor = fore;

			Console.Write(format, arg);

			Console.ForegroundColor = f;
		}
		public static void Write(ConsoleColor fore, ConsoleColor back, String format, params Object[] arg)
		{
			var f = Console.ForegroundColor;
			var b = Console.BackgroundColor;
			Console.ForegroundColor = fore;
			Console.BackgroundColor = back;

			Console.Write(format, arg);

			Console.ForegroundColor = f;
			Console.BackgroundColor = b;
		}
		public static void WriteLine(ConsoleColor fore, String format, params Object[] arg)
		{
			var f = Console.ForegroundColor;
			Console.ForegroundColor = fore;

			Console.WriteLine(format, arg);

			Console.ForegroundColor = f;
		}
		public static void WriteLine(ConsoleColor fore, ConsoleColor back, String format, params Object[] arg)
		{
			var f = Console.ForegroundColor;
			var b = Console.BackgroundColor;
			Console.ForegroundColor = fore;
			Console.BackgroundColor = back;

			Console.WriteLine(format, arg);

			Console.ForegroundColor = f;
			Console.BackgroundColor = b;
		}
 
	}
}
