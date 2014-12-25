using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace SoapApi
{
    public class SoapPostAttribute : IActionHttpMethodProvider
    {
        public Collection<HttpMethod> HttpMethods
        {
            get { throw new NotImplementedException(); }
        }
    }

    public class SoapMethod : HttpMethod
    {
        public SoapMethod(string method) : base(method)
        {
        }
    }
}
