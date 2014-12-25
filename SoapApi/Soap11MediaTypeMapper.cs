using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Formatting;

namespace SoapApi
{
    public class Soap11MediaTypeMapper : MediaTypeMapping
    {
        private const string SoapMediaType = "text/xml";

        public Soap11MediaTypeMapper()
            : base(SoapMediaType)
        {
        }

        public override double TryMatchMediaType(HttpRequestMessage request)
        {
            if (request.Content == null || request.Content.Headers == null ||
                request.Content.Headers.ContentType == null || request.Content.Headers.ContentType.MediaType == null)
                return 0;

            if (request.Headers.Any(
                header => header.Key.Equals("SOAPAction", StringComparison.InvariantCultureIgnoreCase))
                && request.Content.Headers.ContentType.MediaType.Equals("text/xml", StringComparison.InvariantCultureIgnoreCase))
            {
                var soapAction =
                    request.Headers.First(h => h.Key.Equals("SOAPAction", StringComparison.InvariantCultureIgnoreCase));



                request.Properties.Add("SOAPAction", soapAction.Value.First());
                return 1;
            }

            return 0;
        }
    }
}