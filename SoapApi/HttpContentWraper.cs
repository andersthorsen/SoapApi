using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;

namespace SoapApi
{
    public class HttpContentWraper : HttpContent
    {
        private readonly Stream _bodyStream;

        public HttpContentWraper(HttpContent content, Stream bodyStream)
        {
            _bodyStream = bodyStream;
            foreach (var header in content.Headers)
            {
                Headers.Add(header.Key, header.Value);
            }
        }

        protected override Task SerializeToStreamAsync(Stream stream, TransportContext context)
        {
            throw new NotImplementedException();
        }

        protected override bool TryComputeLength(out long length)
        {
            length = 0;
            return false;
        }

        protected override Task<Stream> CreateContentReadStreamAsync()
        {
            return Task.Factory.StartNew(() => _bodyStream);
        }
    }
}