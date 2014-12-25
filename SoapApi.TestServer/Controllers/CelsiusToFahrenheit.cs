using System.Runtime.Serialization;

namespace SoapApi.TestServer.Controllers
{
    [DataContract(Namespace = "http://www.w3schools.com/webservices/")]
    public class CelsiusToFahrenheit
    {
        [DataMember]
        public string Celsius { get; set; }
    }
}