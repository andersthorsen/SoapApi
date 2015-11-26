using System;
using System.IO;
using System.Net.Http.Formatting;
using System.ServiceModel.Channels;
using System.Text;
using System.Xml;
using Microsoft.Owin.Hosting;
using System.Net.Http;
using SoapApi.TestServer.Controllers;

namespace SoapApi.TestServer
{

    class Program
    {
        static void Main(string[] args)
        {
            Random r = new Random();
            int portNumber = r.Next(8000, 9999);
            string baseUrl = "http://localhost:" + portNumber;

            using (WebApp.Start<Startup>(baseUrl))
            {
                Console.WriteLine("Started at address " + baseUrl );

                SendTestMessage(baseUrl, new CelsiusToFahrenheit { Celsius = "Hi"}, MessageVersion.Soap11, true);
                SendTestMessage(baseUrl, new CelsiusToFahrenheit { Celsius = "Hi" }, MessageVersion.Soap12, false);

                Console.ReadLine();
            }
        }

        private static void SendTestMessage(string baseUrl, CelsiusToFahrenheit obj, MessageVersion version, bool soap11)
        {
            using (var client = new HttpClient())
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

                var result = client.PostAsync(baseUrl + "/soap/Test", content);

                result.Wait();

                var contentTask = result.Result.Content.ReadAsStringAsync();

                var str = contentTask.Result;
            }
        }
    }
}