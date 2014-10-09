using System.Web.Http;

namespace MMO.Web.App_Start
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration configuration) {
            configuration.MapHttpAttributeRoutes();
        }
    }
}