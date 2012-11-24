using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;

namespace NotMissing
{
	public static class HttpExt
	{
		public static HttpWebResponse GetResponseNoException(this HttpWebRequest req)
		{
			try
			{
				return (HttpWebResponse)req.GetResponse();
			}
			catch (WebException we)
			{
				var resp = we.Response as HttpWebResponse;
				if (resp == null)
					throw;
				return resp;
			}
		}

	}
}
