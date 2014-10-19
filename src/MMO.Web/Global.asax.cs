using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using MMO.Web.App_Start;
using Serilog;
using Serilog.Events;

namespace MMO.Web
{
    public class MvcApplication : System.Web.HttpApplication
    {
        protected void Application_Start() {
            SetUpSerilog();
            log4net.Config.XmlConfigurator.Configure();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AreaRegistration.RegisterAllAreas();
            
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configuration.EnsureInitialized();

            
        }

        public void SetUpSerilog() {
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Seq("http://herki.cloudapp.net:5341")
                .CreateLogger();

            // Standard .NET format string, useful if you're migrating from another logger
            Log.Information("Starting up on {0} with {1} bytes allocated", Environment.MachineName, Environment.WorkingSet);
 
        }
    }
}
