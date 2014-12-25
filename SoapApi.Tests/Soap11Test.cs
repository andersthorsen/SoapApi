using System.IO;
using System.Net.Http;
using System.ServiceModel.Channels;
using System.Text;
using System.Web.Http;
using System.Web.Http.Dispatcher;
using System.Xml;
using FluentAssertions;
using Microsoft.Owin.Testing;
using NUnit.Framework;
using Owin;
using SoapApi.Tests.Controllers;

namespace SoapApi.Tests
{
    [TestFixture]
    public class Soap11Test : SoapBaseTest
    {
        [Test]
        public void TestSoap11()
        {
            using (var server = TestServer.Create(app =>
            {
                var config = new HttpConfiguration();
                config.Routes.MapSoapRoute("DefaultSoap", "soap/{controller}");

                config.UseSoapApi();

                app.UseWebApi(config);
            }))
            {

                IHttpControllerTypeResolver typeResovler = new DefaultHttpControllerTypeResolver();


                var controllerTypes = typeResovler.GetControllerTypes(new DefaultAssembliesResolver());

                SendTestMessage(server.HttpClient, new CelsiusToFahrenheit {Celsius = "Hi"}, MessageVersion.Soap11, true)
                    .Should()
                    .NotBeNullOrWhiteSpace();
            }
        }

        [Test]
        public void TestSoap12()
        {
            using (var server = TestServer.Create(app =>
            {
                var config = new HttpConfiguration();
                config.Routes.MapSoapRoute("DefaultSoap", "soap/{controller}");

                config.UseSoapApi();

                app.UseWebApi(config);
            }))
            {
                SendTestMessage(server.HttpClient, new CelsiusToFahrenheit { Celsius = "Hi" }, MessageVersion.Soap12, false)
                    .Should()
                    .NotBeNullOrWhiteSpace();
            }
        }

        private static string SendTestMessage(HttpClient client, CelsiusToFahrenheit obj, MessageVersion version,
            bool soap11)
        {

            var msg = Message.CreateMessage(version,
                "http://www.w3schools.com/webservices/CelsiusToFahrenheit", obj);

            var data = new MemoryStream();
            var writer = new XmlTextWriter(data, Encoding.UTF8);
            msg.WriteMessage(writer);
            writer.Flush();
            data.Seek(0, SeekOrigin.Begin);

            var content = new StreamContent(data);

            if (soap11)
            {
                content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("text/xml");
                content.Headers.Add("SOAPAction", "http://www.w3schools.com/webservices/CelsiusToFahrenheit");
            }
            else
            {
                content.Headers.ContentType =
                    new System.Net.Http.Headers.MediaTypeHeaderValue("application/soap+xml");
            }

            var result = client.PostAsync("/soap/Test", content);

            result.Wait();

            var contentTask = result.Result.Content.ReadAsStringAsync();

            var str = contentTask.Result;

            return str;
        }
    }
}
