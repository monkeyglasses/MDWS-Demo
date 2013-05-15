using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.IO;

namespace MdwsDemo.dao.rest
{
    public class HttpClient
    {
        public String BaseUri { get; set; }

        public HttpClient(String baseUri)
        {
            this.BaseUri = baseUri;
        }

        public String makeRequest(String request)
        {
            return makeRequest(HttpRequestType.GET, request);
        }

        public String makeRequest(HttpRequestType type, String request)
        {
            if (type != HttpRequestType.GET && type != HttpRequestType.DELETE)
            {
                throw new ArgumentException("Must be a GET or DELETE");
            }
            WebRequest wr = WebRequest.Create(String.Concat(this.BaseUri, request));
            wr.Method = Enum.GetName(typeof(HttpRequestType), type);
           // HttpWebResponse response = (HttpWebResponse)wr.GetResponse();

            using (HttpWebResponse response = (HttpWebResponse)wr.GetResponse())
            {
                using (StreamReader rdr = new StreamReader(response.GetResponseStream()))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        String responseBody = rdr.ReadToEnd();
                        return responseBody;
                    }
                    else
                    {
                        return response.StatusCode.ToString();
                        // TODO - handle error
                    }

                }
            }
        }

        public String postOrPut(HttpRequestType type, String body)
        {
            if (type != HttpRequestType.POST && type != HttpRequestType.PUT)
            {
                throw new ArgumentException("Must be a POST or PUT");
            }
            WebRequest wr = WebRequest.Create(this.BaseUri);
            wr.Method = Enum.GetName(typeof(HttpRequestType), type);
            byte[] bytesToSend = System.Text.Encoding.UTF8.GetBytes(body);
            wr.ContentLength = bytesToSend.Length;
            wr.ContentType = "application/x-www-form-urlencoded";

            using (Stream writer = wr.GetRequestStream())
            {
                writer.Write(bytesToSend, 0, bytesToSend.Length);
            }

            using (HttpWebResponse response = (HttpWebResponse)wr.GetResponse())
            {
                using (StreamReader rdr = new StreamReader(response.GetResponseStream()))
                {
                    if (response.StatusCode == HttpStatusCode.OK)
                    {
                        String responseBody = rdr.ReadToEnd();
                        return responseBody;
                    }
                    else
                    {
                        return response.StatusCode.ToString();
                        // TODO - handle error
                    }

                }
            }
        }
    }

    public enum HttpRequestType
    {
        POST,
        PUT,
        GET,
        DELETE
    }
}