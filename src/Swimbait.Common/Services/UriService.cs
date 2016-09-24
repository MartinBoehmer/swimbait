using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Xml.Linq;
using Newtonsoft.Json;

namespace Swimbait.Common
{
    public class UriService
    {
        public static ResponseLog GetResponse(IPAddress targetIp, int port, string pathAndQuery)
        {
            var result = new ResponseLog();
            using (var httpClient = new HttpClient())
            {
                var uri = new Uri($"http://{targetIp}:{port}" + pathAndQuery);

                result.RequestUri = uri;

                try
                {
                    HttpResponseMessage response = httpClient.GetAsync(uri).Result;
                    var stream = response.Content.ReadAsStreamAsync().Result;

                    var reader = new StreamReader(stream);
                    result.ResponseBody = reader.ReadToEnd();

                    stream.Seek(0, SeekOrigin.Begin);
                    result.ResponseBytes = ReadFully(stream);
                    
                    if (result.ResponseBody.StartsWith("<"))
                    {
                        result.ResponseBody = AsFormattedXml(result.ResponseBody);
                    }
                    if (result.ResponseBody.StartsWith("{"))
                    {
                        result.ResponseBody = AsFormattedJson(result.ResponseBody);
                    }
                }
                catch (Exception e)
                {
                    result.ResponseBody = e.Message;
                }
            }
            return result;
        }

        public static ResponseLog GetManInTheMiddleResult(IPAddress targetIp, Uri thisRequest, Func<Uri,int> portRemap)
        {
            var relayPort = portRemap(thisRequest);
            var result = GetResponse(targetIp, relayPort, thisRequest.PathAndQuery);
            return result;
        }

        static string AsFormattedXml(string xml)
        {
            try
            {
                var doc = XDocument.Parse(xml);
                return doc.ToString();
            }
            catch (Exception)
            {
                return xml;
            }
        }

        public static byte[] ReadFully(Stream input)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                input.CopyTo(ms);
                return ms.ToArray();
            }
        }

        private static string AsFormattedJson(string json)
        {
            dynamic parsedJson = JsonConvert.DeserializeObject(json);
            return JsonConvert.SerializeObject(parsedJson, Formatting.Indented);
        }
    }
}
