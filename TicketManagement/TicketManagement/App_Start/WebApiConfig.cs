using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Web.Http;

namespace TicketManagement
{
    public class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            config.MapHttpAttributeRoutes();
            config.Formatters.JsonFormatter.SupportedMediaTypes.Add(new MediaTypeHeaderValue("text/html"));
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{action}/{id}",
                defaults: new { controller = "BaseApi", action = "Get", id = RouteParameter.Optional }
            );
            config.Routes.MapHttpRoute(
                name: "DualParameterApi",
                routeTemplate: "api/{controller}/{action}/{parameter1}/{parameter2}",
                defaults: new { controller = "BaseApi", action = "Get", parameter1 = RouteParameter.Optional, parameter2 = RouteParameter.Optional }
            );

            JsonMediaTypeFormatter formatter = GlobalConfiguration.Configuration.Formatters.JsonFormatter;
            formatter.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver();
        }
    }
}
