using Microsoft.AspNet.TestHost;
using Microsoft.AspNet.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Http;
using System.IO;
using System.Xml;
using System.Text;
using System.Runtime.Serialization;
using System.ServiceModel.Channels;
using FluentAssertions;
using Xunit;

namespace SoapApi.Tests
{
    // This project can output the Class library as a NuGet Package.
    // To enable this option, right-click on the project and select the Properties menu item. In the Build tab select "Produce outputs on build".
    public class Class1
    {
        public Class1()
        {
        }

        [Fact]
        public void TestSoap11()
        {
            using (var server = TestServer.Create(app =>
            {
                app.UseMvc();

            }, services =>
            {
                services.AddMvc()
                .AddSoapSerializerFormatters();
            }))
            {

              //  IHttpControllerTypeResolver typeResovler = new DefaultHttpControllerTypeResolver();


                //var controllerTypes = typeResovler.GetControllerTypes(new DefaultAssembliesResolver());

                SendTestMessage(server.CreateClient(), new CelsiusToFahrenheit { Celsius = "Hi" }, MessageVersion.Soap11, true)
                    .Should()
                    .NotBeNullOrWhiteSpace();
            }
        }
        [Fact]
        public void TestMe()
        {
            using (var server = TestServer.Create(app =>
            {
                app.UseMvc();

            }, services =>
            {
                services.AddMvc()
                .AddSoapSerializerFormatters();
            }))
            {
                SendTestMessage(server.CreateClient(), new CelsiusToFahrenheit { Celsius = "Hi" }, MessageVersion.Soap12WSAddressing10, false)
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

            var result = client.PostAsync("/api/Test", content);

            result.Wait();

            var contentTask = result.Result.Content.ReadAsStringAsync();

            var str = contentTask.Result;

            return str;
        }


    }

    [DataContract(Namespace = "http://www.w3schools.com/webservices/")]
    public class CelsiusToFahrenheit
    {
        [DataMember]
        public string Celsius { get; set; }
    }
}
