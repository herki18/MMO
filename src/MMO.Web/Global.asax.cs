using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MMO.Web.App_Start;

namespace MMO.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start() {
            GlobalConfiguration.Configure(WebApiConfig.Configure);

            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AreaRegistration.RegisterAllAreas();
            RouteConfig.RegisterRoutes(RouteTable.Routes);
        }
    }
}
