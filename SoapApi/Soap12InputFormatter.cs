using System;
using System.Xml;
using System.Threading.Tasks;
using System.ServiceModel.Channels;
using Microsoft.AspNet.Mvc.Internal;
using Microsoft.AspNet.Mvc.Formatters;

namespace SoapApi
{
    public class Soap12InputFormatter : InputFormatter
    {
        private XmlDictionaryReaderQuotas _readerQuotas;

        public Soap12InputFormatter()
        {
            SupportedMediaTypes.Add(KnownMediaTypes.ApplicationSoapXml);

            SupportedEncodings.Add(UTF8EncodingWithoutBOM);
            SupportedEncodings.Add(UTF16EncodingLittleEndian);

            _readerQuotas = new XmlDictionaryReaderQuotas();
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var request = context.HttpContext.Request;
            var effectiveEncoding = SelectCharacterEncoding(context);

            using (var reader = XmlDictionaryReader.CreateTextReader(new NonDisposableStream(request.Body), effectiveEncoding, _readerQuotas, onClose: null))
            {

                var msg = Message.CreateMessage(reader, 1000, MessageVersion.Soap12WSAddressing10);

                var obj = msg.GetBody(context.ModelType);

                return InputFormatterResult.SuccessAsync(obj);
            }
        }
    }
}