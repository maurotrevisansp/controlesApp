using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ControleApp.Web
{
    public class Request
    {
        private string url;
        HttpWebRequest request;
        public Dictionary<string, string> Headers { get; set; }
        private ContentType content;

        public ContentType Content
        {
            get { return content; }
            set
            {
                switch (value)
                {
                    case ContentType.Json:
                        request.ContentType = "application/json";
                        break;
                    case ContentType.Xml:
                        request.ContentType = "text / xml";
                        break;
                    case ContentType.Form_Url_Encoded:
                        request.ContentType = "application/x-www-form-urlencoded";
                        break;
                }
                content = value;
            }
        }

        public enum ContentType
        {
            Json = 1,
            Xml = 2,
            Form_Url_Encoded = 3
        }


        public Request(string url, Dictionary<string, string> headers = null, uint timeout = 360)
        {
            try
            {

                if (headers != null) Headers = headers;
                this.url = url;
            }
            catch (Exception ex)
            {
                throw new WebRequestException("Erro durante requisição ao servidor.") { Request = this };
            }

        }



        private string GenerateParameterString(Dictionary<string, string> p)
        {
            try
            {
                if (p == null)
                    return string.Empty;
                string strReturn = String.Empty;
                if (p.Count > 0)
                {
                    strReturn += "?";

                    foreach (var parameter in p)
                    {
                        strReturn += WebUtility.UrlEncode(parameter.Key) + "=" + WebUtility.UrlEncode(parameter.Value) +
                                     "&";
                    }
                    strReturn = strReturn.Remove(strReturn.Length - 1);
                }
                return strReturn;
            }
            catch (Exception)
            {

                throw;
            }
        }

        public async Task<T> Get<T>(Dictionary<string, string> parameters = null,
            ContentType contentType = ContentType.Json)
        {
            try
            {


                url += GenerateParameterString(parameters);
                var uri = new Uri(url);

                request = (HttpWebRequest)HttpWebRequest.Create(uri);

                Content = contentType;
                SetHeaders();

                request.Method = "GET";

                using (WebResponse response = await request.GetResponseAsync())
                {
                    // Get a stream representation of the HTTP web response:
                    using (Stream stream = response.GetResponseStream())
                    {
                        var serializer = new JsonSerializer();
                        if (((HttpWebResponse)response).StatusCode != HttpStatusCode.OK)
                        {
                            throw new WebRequestException("Erro durante requisição ao servidor.")
                            {
                                Request = this,
                                Status = ((HttpWebResponse)response).StatusCode
                            };
                        }

                        using (var reader = new StreamReader(stream))
                        {
                            string value = reader.ReadToEnd();

                            return JsonConvert.DeserializeObject<T>(value);
                        }
                        using (var sr = new StreamReader(stream))
                        using (var jsonTextReader = new JsonTextReader(sr))
                        {

                            return (T)serializer.Deserialize(jsonTextReader, typeof(T));
                        }
                    }
                }
            }
            catch (WebRequestException e)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new WebRequestException("Erro durante requisição ao servidor.") { Request = this };
                ex.ToString();
            }

        }

        private void SetHeaders()
        {
            if (Headers != null)
            {
                foreach (var header in Headers)
                {
                    request.Headers[header.Key] = header.Value;
                }
            }
        }

        public async Task<T> Post<T>(object content = null, ContentType contentType = ContentType.Json,
            Dictionary<string, string> urlParams = null)
        {
            try
            {
                url += GenerateParameterString(urlParams);
                var uri = new Uri(url);

                request = (HttpWebRequest)HttpWebRequest.Create(uri);
                Content = contentType;
                request.Method = "POST";

                SetHeaders();

                string data;
                if (content == null)
                {
                    data = String.Empty;
                }
                else
                {
                    if (contentType == ContentType.Form_Url_Encoded)
                    {
                        data = await new FormUrlEncodedContent((IEnumerable<KeyValuePair<string, string>>)content)
                            .ReadAsStringAsync();
                    }
                    else
                    {
                        data = Newtonsoft.Json.JsonConvert.SerializeObject(content);
                    }
                }




                var stream = await request.GetRequestStreamAsync();

                using (var writer = new StreamWriter(stream))
                {
                    writer.Write(data);
                    writer.Flush();
                    writer.Dispose();
                }

                var response = await request.GetResponseAsync();
                var respStream = response.GetResponseStream();


                using (StreamReader sr = new StreamReader(respStream))
                {
                    //Need to return this response 
                    if (((HttpWebResponse)response).StatusCode != HttpStatusCode.OK && ((HttpWebResponse)response).StatusCode != HttpStatusCode.NoContent)
                    {

                        throw new WebRequestException("Erro durante requisição ao servidor.")
                        {
                            Request = this,
                            Status = ((HttpWebResponse)response).StatusCode
                        };
                    }
                    var serializer = new JsonSerializer();

                    using (var jsonTextReader = new JsonTextReader(sr))
                    {

                        return (T)serializer.Deserialize(jsonTextReader, typeof(T));
                    }
                }
            }
            catch (WebRequestException e)
            {
                throw;
            }
            catch (WebException exx)
            {
                throw new WebRequestException("Erro durante requisição ao servidor.")
                {
                    Request = this,
                    Status = ((HttpWebResponse)exx.Response).StatusCode
                };
            }
            catch (Exception ex)
            {
                throw new WebRequestException("Erro durante requisição ao servidor.")
                {
                    Request = this
                };
            }

        }
    }

    public class WebRequestException : Exception
    {
        public WebRequestException(string msg) : base(msg) { }
        public Request Request { get; set; }
        public HttpStatusCode Status { get; set; }
    }

    public class WebRequestResponseError : Exception
    {
        public WebRequestResponseError(string msg) : base(msg) { }
        public Request Request { get; set; }
    }
}
