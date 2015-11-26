using System.Web.Http;
using Owin;

namespace SoapApi.TestServer
{
    public class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            HttpConfiguration config = new HttpConfiguration();
            config.Routes.MapHttpRoute(
                name: "DefaultWebApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new {id = RouteParameter.Optional}
                );

            config.Routes.MapSoapRoute("DefaultSoap", "soap/{controller}");
            
            config.UseSoapApi();
            app.UseWebApi(config);
        }
    }
}