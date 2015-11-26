using Microsoft.AspNet.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SoapApi.Tests.Controllers
{
    [Route("api/[controller]")]
    public class TestController : Controller
    {
        [HttpPost]
        public string CelsiusToFahrenheit([FromBody] CelsiusToFahrenheit obj)
        {
            return "Hello";
        }
    }
}
