using System.Web.Http;

namespace SoapApi.TestServer.Controllers
{
    public class TestController : ApiController
    {
        [HttpPost]
        public string Test([FromBody] CelsiusToFahrenheit obj)
        {
            return "Hello";
        }
    }
}
