using System.Runtime.Serialization;
using System.Web.Http;

namespace SoapApi.Tests.Controllers
{
    [DataContract(Namespace = "http://www.w3schools.com/webservices/")]
    public class CelsiusToFahrenheit
    {
        [DataMember]
        public string Celsius { get; set; }
    }

    public class TestController : ApiController
    {
        [HttpPost]
        public string CelsiusToFahrenheit([FromBody] CelsiusToFahrenheit obj)
        {
            return "Hello";
        }
    }
}
