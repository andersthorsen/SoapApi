using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Web.Http.Routing;
using System.Xml;

namespace SoapApi
{
    public class SoapRoute : HttpRoute
    {
        private SoapVersionDetected DetectSoapRequest(HttpRequestMessage request)
        {
            if (request.Content == null || request.Content.Headers == null || request.Content.Headers.ContentType == null || request.Content.Headers.ContentType.MediaType == null)
                return SoapVersionDetected.None;

            if (request.Content.Headers.ContentType.MediaType.Equals("text/xml", StringComparison.InvariantCultureIgnoreCase) &&
                request.Headers.Any(x => x.Key.Equals("SOAPAction", StringComparison.InvariantCultureIgnoreCase)))
            {
                return SoapVersionDetected.Soap11;
            }

            if (request.Content.Headers.ContentType.MediaType.Equals("application/soap+xml", StringComparison.InvariantCultureIgnoreCase))
            {
                return SoapVersionDetected.Soap12;
            }

            return SoapVersionDetected.None;
        }

        public SoapRoute(string routeTemplate, HttpRouteValueDictionary defaults, HttpRouteValueDictionary constraints, HttpRouteValueDictionary dataTokens, HttpMessageHandler handler) 
            : base(routeTemplate, defaults, constraints, dataTokens, handler)
        {
            
        }

        public override IHttpRouteData GetRouteData(string virtualPathRoot, HttpRequestMessage request)
        {
            var version = DetectSoapRequest(request);
            var b = base.GetRouteData(virtualPathRoot, request);

            if (b == null)
                return null;

            var soapAction = "";

            switch (version)
            {
                case SoapVersionDetected.None:
                    return null;

                case SoapVersionDetected.Soap11:
                   soapAction = request.Headers.GetValues("SOAPAction").Single();
                     
                    break;

                case SoapVersionDetected.Soap12:
                    var readTask = request.Content.ReadAsStreamAsync();

                    readTask.ContinueWith(parent =>
                    {

                        BackupStream backupStream = null;
                        XmlTextReader reader;

                        if (!parent.Result.CanSeek)
                        {

                            backupStream = new BackupStream(parent.Result);
                            reader = new XmlTextReader(backupStream);
                        }
                        else
                        {
                            reader = new XmlTextReader(parent.Result);
                        }

                        while (reader.NodeType != XmlNodeType.Element
                            || string.Compare(reader.LocalName, "Envelope", StringComparison.InvariantCultureIgnoreCase) != 0)
                            reader.Read();


                        if (reader.NamespaceURI == "http://schemas.xmlsoap.org/soap/envelope/")
                        {
                            // Soap11???
                        }
                        else if (reader.NamespaceURI == "http://www.w3.org/2003/05/soap-envelope")
                        {
                            // Soap12

                            while (reader.NodeType != XmlNodeType.Element
                            || string.Compare(reader.LocalName, "action", StringComparison.InvariantCultureIgnoreCase) != 0)
                                reader.Read();
                          
                        }

                        if (string.Compare(reader.LocalName, "action", StringComparison.InvariantCultureIgnoreCase) == 0)
                        {
                            soapAction = reader.ReadString();
                        }

                        if (backupStream != null)
                        {
                            backupStream.Stream.Seek(0, SeekOrigin.Begin);
                            var mergeStream = new MergeStream(backupStream.Stream, parent.Result);

                            request.Content = new HttpContentWraper(request.Content, mergeStream);
                        }
                        else
                        {
                            parent.Result.Seek(0, SeekOrigin.Begin);
                        }

                    }).Wait();
                    break;
            }

            if (string.IsNullOrWhiteSpace(soapAction))
                return null;

            var path = new Uri(soapAction).AbsolutePath;
            var splitPath = path.Split('/');

            b.Values["action"] = splitPath.Last();

            request.Properties.Add("SoapAction", soapAction);

            return b;
        }

        public override IHttpVirtualPathData GetVirtualPath(HttpRequestMessage request, IDictionary<string, object> values)
        {
            throw new NotImplementedException();
        }
    }
}
