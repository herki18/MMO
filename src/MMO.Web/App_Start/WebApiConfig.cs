using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;

namespace MMO.Web.App_Start
{
    public static class WebApiConfig
    {
        public static void Configure(HttpConfiguration configuration) {
            //configuration.Routes.MapHttpRoute("ApiDefault", "api/v1/{controller}/id", new { id = RouteParameter.Optional});
            configuration.MapHttpAttributeRoutes();
        }
    }
}