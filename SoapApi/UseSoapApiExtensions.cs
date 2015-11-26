//using System.Net.Http;
//using System.Web.Http;
//using System.Web.Http.Routing;

//namespace SoapApi
//{
//    public static class SoapApiExtensions
//    {
//        public static void UseSoapApi(this HttpConfiguration config)
//        {
//            config.Formatters.Insert(0, new Soap12Formatter());
//            config.Formatters.Insert(0, new Soap11Formatter());
//        }

//        /// <summary>
//        /// Maps the specified route template.
//        /// </summary>
//        /// <param name="routes">A collection of routes for the application.</param>
//        /// <param name="name">The name of the route to map.</param>
//        /// <param name="routeTemplate">The route template for the route.</param>
//        /// <returns>A reference to the mapped route.</returns>
//        public static IHttpRoute MapSoapRoute(this HttpRouteCollection routes, string name, string routeTemplate)
//        {
//            var route = new SoapRoute(routeTemplate, new HttpRouteValueDictionary(), new HttpRouteValueDictionary(), new HttpRouteValueDictionary(), null);

//            routes.Add(name, route);

//            return route;
//        }

//        /// <summary>
//        /// Maps the specified route template and sets default route values.
//        /// </summary>
//        /// <param name="routes">A collection of routes for the application.</param>
//        /// <param name="name">The name of the route to map.</param>
//        /// <param name="routeTemplate">The route template for the route.</param>
//        /// <param name="defaults">An object that contains default route values.</param>
//        /// <returns>A reference to the mapped route.</returns>
//        public static IHttpRoute MapHttpRoute(this HttpRouteCollection routes, string name, string routeTemplate,
//            HttpRouteValueDictionary defaults)
//        {
//            var route = new SoapRoute(routeTemplate, defaults, new HttpRouteValueDictionary(), new HttpRouteValueDictionary(), null);

//            routes.Add(name, route );

//            return route;
//        }

//        /// <summary>
//        /// Maps the specified route template and sets default route values and constraints.
//        /// </summary>
//        /// <param name="routes">A collection of routes for the application.</param>
//        /// <param name="name">The name of the route to map.</param>
//        /// <param name="routeTemplate">The route template for the route.</param>
//        /// <param name="defaults">An object that contains default route values.</param>
//        /// <param name="constraints">A set of expressions that specify values for routeTemplate.</param>
//        /// <returns>A reference to the mapped route.</returns>
//        public static IHttpRoute MapHttpRoute(this HttpRouteCollection routes, string name, string routeTemplate,
//            HttpRouteValueDictionary defaults, HttpRouteValueDictionary constraints)
//        {
//            var route = new SoapRoute(routeTemplate, defaults, constraints, new HttpRouteValueDictionary(), null);

//            routes.Add(name, route);

//            return route;
//        }
        
//        /// <summary>
//        /// Maps the specified route template and sets default route values, constraints, and end-point message handler.
//        /// </summary>
//        /// <param name="routes">A collection of routes for the application.</param>
//        /// <param name="name">The name of the route to map.</param>
//        /// <param name="routeTemplate">The route template for the route.</param>
//        /// <param name="defaults">An object that contains default route values.</param>
//        /// <param name="constraints">A set of expressions that specify values for routeTemplate.</param>
//        /// <param name="handler"> The handler to which the request will be dispatched.</param>
//        /// <returns>A reference to the mapped route.</returns>
//        public static IHttpRoute MapHttpRoute(this HttpRouteCollection routes, string name, string routeTemplate,
//            HttpRouteValueDictionary defaults, HttpRouteValueDictionary constraints, HttpMessageHandler handler)
//        {
//            var route = new SoapRoute(routeTemplate, defaults, constraints, new HttpRouteValueDictionary(), handler);

//            return route;
//        }
//    }
//}
