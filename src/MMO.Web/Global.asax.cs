using System;
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
            log4net.Config.XmlConfigurator.Configure();
            GlobalConfiguration.Configure(WebApiConfig.Register);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            AreaRegistration.RegisterAllAreas();
            
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            GlobalConfiguration.Configuration.EnsureInitialized();

            SetUpSerilog();
        }

        public void SetUpSerilog() {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(LogEventLevel.Verbose)
                .WriteTo.Logentries("ceae825c-f44d-41de-a7b8-18bb6a358808", false, 50, null, LogEventLevel.Debug)
                //.WriteTo.Logentries("5e4801c4-2fca-49e0-9369-79cc7b138c79", false, 50, null, LogEventLevel.Debug)
                .WriteTo.Seq("http://herki.cloudapp.net:5341", LogEventLevel.Debug)
                .CreateLogger();

            //// Standard .NET format string, useful if you're migrating from another logger
            Log.Information("Starting up on {0} with {1} bytes allocated", Environment.MachineName, Environment.WorkingSet);

            //var position = new { Latitude = 25, Longitude = 134 };
            //const int elapsedMs = 34;

            //Log.Information("Processed {@Position} in {Elapsed:000} ms.", position, elapsedMs);
        }


    }
}
