using Microsoft.Net.Http.Headers;

namespace SoapApi
{
    public static class KnownMediaTypes
    {
        public static MediaTypeHeaderValue TextXmlSoap11 = MediaTypeHeaderValue.Parse("text/xml");
        public static MediaTypeHeaderValue ApplicationSoapXml = MediaTypeHeaderValue.Parse("application/soap+xml");
    }
}