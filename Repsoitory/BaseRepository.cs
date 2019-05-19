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
        internal enum requestType { GET, POST, PATCH, DELETE };
        internal string _host = "https://api.spotify.com/v1/";
        internal string _apiKey;

        internal JToken Query(requestType method, string query, string body)
        {
            HttpWebResponse response = null;
            response = MakeRequest(NewRequest(method, _host + query, body));
            JToken result = ParseResult(response, query);
            return result;
        }

        internal JToken Query(requestType method, string query)
        {
            HttpWebResponse response = null;
            response = MakeRequest(NewRequest(method, _host + query));
            JToken result = ParseResult(response, query);
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
        internal HttpWebRequest NewRequest(requestType method, string uri)
        {
            HttpWebRequest request = (HttpWebRequest)WebRequest.Create(uri);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request = SetLoginDetails(request);
            request.Method = method.ToString();
            request.Accept = "application/vnd.api+json";//application/json
            return request;
        }

        /// <summary>
        /// Add a body to a request.
        /// </summary>
        internal HttpWebRequest NewRequest(requestType method, string uri, string body)
        {
            HttpWebRequest request = NewRequest(method, uri);
            request.AutomaticDecompression = DecompressionMethods.GZip;
            request.ContentType = "application/json";
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


        internal JToken ParseResult(HttpWebResponse response, string query)
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
}
