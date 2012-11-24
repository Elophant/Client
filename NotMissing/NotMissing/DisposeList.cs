using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;

namespace NotMissing
{
	public class DisposeList : List<IDisposable>, IDisposable
	{
		public void Dispose()
		{
			var exs = new List<Exception>();
			for (int i = 0; i < Count; i++)
			{
				try
				{
					var item = this[i];
					if (item != null)
						item.Dispose();
				}
				catch (Exception ex)
				{
					exs.Add(ex);
				}
			}
			if (exs.Count > 0)
			{
				var ex = new Exception("Multiple exceptions. Check Exception.Data[Exceptions]");
				ex.Data["Exceptions"] = exs;
				throw ex;
			}
		}

		public T Create<T>(T item) where T : IDisposable
		{
			Add(item);
			return item;
		}
	}
}
