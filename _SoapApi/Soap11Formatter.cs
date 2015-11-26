using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.ServiceModel.Channels;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml;

namespace SoapApi
{
    public class Soap11Formatter : MediaTypeFormatter
    {
        public Soap11Formatter()
        {
            SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/xml"));

            MediaTypeMappings.Add(new Soap11MediaTypeMapper());

            SupportedEncodings.Add(new UTF8Encoding(encoderShouldEmitUTF8Identifier: false, throwOnInvalidBytes: true));
        }

        public override bool CanReadType(Type type)
        {
            return true;
        }

        public override bool CanWriteType(Type type)
        {
            return true;
        }

        public override MediaTypeFormatter GetPerRequestFormatterInstance(Type type, HttpRequestMessage request, MediaTypeHeaderValue mediaType)
        {
            return new Soap11Formatter {Action = (string) request.Properties["SOAPAction"]};
        }

        public string Action { get; set; }

        public override Task<object> ReadFromStreamAsync(Type type, Stream readStream, HttpContent content, IFormatterLogger formatterLogger, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                var reader = new XmlTextReader(readStream);

                var msg = Message.CreateMessage(reader, 1000, MessageVersion.Soap11);

                var obj = msg.GetBody(type);
                
                return obj;
            }, cancellationToken);
        }

        public override Task WriteToStreamAsync(Type type, object value, Stream writeStream, HttpContent content,
            TransportContext transportContext, CancellationToken cancellationToken)
        {
            return Task.Factory.StartNew(() =>
            {
                var writer = new XmlTextWriter(writeStream, Encoding.UTF8);

                var msg = Message.CreateMessage(MessageVersion.Soap11, Action, value);

                msg.WriteMessage(writer);

                writer.Flush();
            }, cancellationToken);
        }
    }
}