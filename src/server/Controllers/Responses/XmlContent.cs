using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace Swimbait.Server.Controllers.Responses
{
    /// <summary>
    /// http://stackoverflow.com/questions/15366096/how-to-return-xml-data-from-a-web-api-method
    /// </summary>
    public class XmlContent : HttpContent
    {
        private readonly MemoryStream _stream = new MemoryStream();

        public XmlContent(XmlResponse xmlResponse)
        {
            _stream = new MemoryStream(Encoding.UTF8.GetBytes(xmlResponse.GetXml()));
            _stream.Position = 0;
            Headers.ContentType = new MediaTypeHeaderValue("text/xml");
        }

        protected override Task SerializeToStreamAsync(Stream stream, System.Net.TransportContext context)
        {
            _stream.CopyTo(stream);

            var tcs = new TaskCompletionSource<object>();
            tcs.SetResult(null);
            return tcs.Task;
        }

        protected override bool TryComputeLength(out long length)
        {
            length = _stream.Length;
            return true;
        }
    }
}
