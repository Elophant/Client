using System;
using System.Collections.Generic;
using System.Text;
using System.Net;
using System.IO;

namespace System.Net
{
    public class HttpRequest
    {
        private string m_useragent = "Mozilla/5.0 (Windows; U; Windows NT 6.1; en-US; rv:1.9.2.8) Gecko/20100722 Firefox/3.6.8";
        public string UserAgent
        {
            get
            {
                return m_useragent;
            }
            set
            {
                m_useragent = value;
                if (m_request != null)
                    m_request.UserAgent = value;
            }
        }

        private CookieContainer m_cookies = new CookieContainer();
        public CookieContainer Cookies
        {
            get { return m_cookies; }
            set
            {
                m_cookies = value;
                if (m_request != null)
                    m_request.CookieContainer = value;
            }
        }

        private WebProxy m_proxy = null;
        public WebProxy Proxy
        {
            get { return m_proxy; }
            set
            {
                m_proxy = value;
                if (m_request != null)
                    m_request.Proxy = value;
            }
        }

        public HttpRequest()
        {
        }

        public HttpRequest(string url)
        {
            NewRequest(url);
        }

        public void NewRequest(string url)
        {
            m_request = HttpWebRequest.Create(url) as HttpWebRequest;
            m_request.UserAgent = UserAgent;
            m_request.CookieContainer = Cookies;
            m_request.Proxy = Proxy;
            m_request.ServicePoint.Expect100Continue = false;
        }
        public void NewRequest()
        {
            NewRequest(m_request.Address.OriginalString);
        }

        private HttpWebRequest m_request;
        public HttpWebRequest Request
        {
            get { return m_request; }
            set { m_request = value; }
        }
        private HttpWebResponse m_response;
        public HttpWebResponse Response
        {
            get { return m_response; }
            set { m_response = value; }
        }

        public string Get()
        {
            m_request.Method = "GET";

            m_response = m_request.GetResponse() as HttpWebResponse;

            StreamReader sr = new StreamReader(m_response.GetResponseStream());
            string ret = sr.ReadToEnd();
            sr.Close();

            m_cookies.Add(m_response.Cookies);

            m_response.Close();

            return ret;
        }
        public byte[] GetBytes()
        {
            m_request.Method = "GET";

            m_response = m_request.GetResponse() as HttpWebResponse;

            Stream s = m_response.GetResponseStream();
            var ret = new MemoryStream();
            byte[] t = new byte[2000];
            int read = 0;
            while ((read = s.Read(t, 0, t.Length)) > 0)
            {
                ret.Write(t, 0, read);
            }
            s.Close();

            m_cookies.Add(m_response.Cookies);

            m_response.Close();

            return ret.ToArray();
        }
        public string PostForm(string data)
        {
            m_request.Method = "POST";

            m_request.ContentType = "application/x-www-form-urlencoded";

            Stream s = m_request.GetRequestStream();
            s.Write(Encoding.ASCII.GetBytes(data), 0, data.Length);
            s.Close();

            m_response = m_request.GetResponse() as HttpWebResponse;

            StreamReader sr = new StreamReader(m_response.GetResponseStream());
            string ret = sr.ReadToEnd();
            sr.Close();

            m_cookies.Add(m_response.Cookies);

            m_response.Close();

            return ret;
        }
        public byte[] PostFormBytes(string data)
        {
            m_request.Method = "POST";

            m_request.ContentType = "application/x-www-form-urlencoded";

            Stream s = m_request.GetRequestStream();
            s.Write(Encoding.ASCII.GetBytes(data), 0, data.Length);
            s.Close();

            m_response = m_request.GetResponse() as HttpWebResponse;

            s = m_response.GetResponseStream();
            var ret = new MemoryStream();
            byte[] t = new byte[2000];
            int read = 0;
            while ((read = s.Read(t, 0, t.Length)) > 0)
            {
                ret.Write(t, 0, read);
            }
            s.Close();

            m_cookies.Add(m_response.Cookies);

            m_response.Close();

            return ret.ToArray();
        }
        public string PostMulti(string boundary,byte[] data)
        {
            m_request.Method = "POST";

            m_request.ContentType = "multipart/form-data; boundary=" + boundary;

            Stream s = m_request.GetRequestStream();
            s.Write(data, 0, data.Length);
            s.Close();

            m_response = m_request.GetResponse() as HttpWebResponse;

            StreamReader sr = new StreamReader(m_response.GetResponseStream());
            string ret = sr.ReadToEnd();
            sr.Close();

            m_cookies.Add(m_response.Cookies);

            m_response.Close();

            return ret;
        }
    }
}