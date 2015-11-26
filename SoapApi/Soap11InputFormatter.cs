using System.Threading.Tasks;
using Microsoft.AspNet.Mvc.Formatters;
using System.ServiceModel.Channels;
using System.Xml;
using Microsoft.AspNet.Mvc.Internal;
using System;

namespace SoapApi
{

    public class Soap11InputFormatter : InputFormatter
    {
        private XmlDictionaryReaderQuotas _readerQuotas = new XmlDictionaryReaderQuotas();

        public Soap11InputFormatter()
        {
            SupportedEncodings.Add(UTF8EncodingWithoutBOM);
            SupportedEncodings.Add(UTF16EncodingLittleEndian);

            SupportedMediaTypes.Add(KnownMediaTypes.TextXmlSoap11);
        }

        public override bool CanRead(InputFormatterContext context)
        {
            if (context.HttpContext.Request.Headers.ContainsKey("SOAPAction"))
            {
                return true;
            }

            return false;
        }

        public XmlDictionaryReaderQuotas XmlDictionaryReaderQuotas
        {
            get { return _readerQuotas; }
        }

        protected override bool CanReadType(Type type)
        {
            return (type != null);
        }

        public override Task<InputFormatterResult> ReadRequestBodyAsync(InputFormatterContext context)
        {
            var effectiveEncoding = SelectCharacterEncoding(context);
            if (effectiveEncoding == null || !context.HttpContext.Request.Headers.ContainsKey("SOAPAction"))
            {
                return InputFormatterResult.FailureAsync();
            }

            var soapAction = context.HttpContext.Request.Headers["SOAPAction"];

            var request = context.HttpContext.Request;

            using (var reader = XmlDictionaryReader.CreateTextReader(new NonDisposableStream(request.Body), effectiveEncoding, _readerQuotas, onClose: null))
            {

                var msg = Message.CreateMessage(reader, 1000, MessageVersion.Soap11);

                var obj = msg.GetBody(context.ModelType);

                return InputFormatterResult.SuccessAsync(obj);
            }
        }
    }
}
