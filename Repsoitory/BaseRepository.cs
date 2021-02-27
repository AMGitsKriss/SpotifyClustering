using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace Repsoitory
{

    public abstract class BaseRepository
    {
        internal enum RequestType { GET, POST, PATCH, DELETE };
        internal string _apiKey;
        internal const string _json = "application/json";
        internal const string _form = "application/x-www-form-urlencoded";
        internal string _contentType = _json;
        private IConfiguration _config;

        public BaseRepository(IConfiguration config)
        {
            _config = config;
        }

        private void SetAuthorization(string apiKey)
        {
            _apiKey = _apiKey;
        }

        internal JToken MakeRequest(RequestType method, string uri, string body)
        {
            return Query(NewRequest(method, uri, body));
        }

        internal JToken MakeRequest(RequestType method, string uri)
        {
            return Query(NewRequest(method, uri));
        }

        internal JToken Query(HttpWebRequest request)
        {
            HttpWebResponse response = MakeRequest(request);
            JToken result = ParseResult(response);
            return result;
        }

        /// <summary>
        /// Get a proper response object
        /// </summary>
        internal HttpWebResponse MakeRequest(HttpWebRequest request)
        {
            try
            {
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                return response;
            }
            catch (WebException ex)
            {
                // Server response stuff
                HttpWebResponse response = (HttpWebResponse)ex.Response;
                return response;
            }
        }

        /// <summary>
        /// Make a request without a body.
        /// </summary>
        internal HttpWebRequest NewRequest(RequestType method, string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request = SetLoginDetails(request);
            request.Method = method.ToString();
            request.Accept = "application/json";//application/vnd.api+json
            return request;
        }

        /// <summary>
        /// Add a body to a request.
        /// </summary>
        internal HttpWebRequest NewRequest(RequestType method, string uri, string body)
        {
            HttpWebRequest request = NewRequest(method, uri);
            request.ContentType = _contentType;
            request.ContentLength = body.Length;

            try
            {
                using (var streamWriter = new StreamWriter(request.GetRequestStream()))
                {
                    streamWriter.Write(body);
                    streamWriter.Flush();
                }
            }
            catch (WebException ex)
            {
                throw ex;
            }
            return request;
        }

        /// <summary>
        /// Assign the token to a header
        /// </summary>
        private HttpWebRequest SetLoginDetails(HttpWebRequest request)
        {
            request.Headers.Add(HttpRequestHeader.Authorization, _apiKey);
            return request;
        }

        static public string EncodeTo64(string toEncode)
        {
            byte[] toEncodeAsBytes = System.Text.ASCIIEncoding.ASCII.GetBytes(toEncode);

            string returnValue = System.Convert.ToBase64String(toEncodeAsBytes);

            return returnValue;
        }

        internal JToken ParseResult(HttpWebResponse response)
        {
            JToken parsedResults = null;
            if (response == null)
            {
                // We should have thrown the error already.
            }
            else if (response.StatusCode != HttpStatusCode.OK && response.StatusCode != HttpStatusCode.NoContent && response.StatusCode != HttpStatusCode.Created)
            {
                parsedResults = JToken.Parse(ReadResponseString(response));
            }
            else if (response.StatusCode != HttpStatusCode.NoContent)
            {
                parsedResults = JToken.Parse(ReadResponseString(response));
            }

            response.Close();
            return parsedResults;
        }


        private string ReadResponseString(HttpWebResponse response)
        {
            string responseString = string.Empty;
            if (response.StatusCode != HttpStatusCode.NoContent)
            {
                Stream stream = response.GetResponseStream();
                responseString = new StreamReader(stream).ReadToEnd();
            }
            return responseString;
        }

        internal static string Serialize<T>(T model)
        {
            return JsonConvert.SerializeObject(model, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
        }
    }

    public class Headers
    {
    }
}
