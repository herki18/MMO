using System;
using System.IO;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using MMO.Base.Infrastructure;
using Photon.SocketServer;
using Serilog;
using Serilog.Events;

namespace MMO.Server.Master
{
    public class Application : ApplicationBase {
        private readonly MasterServerContext _application;

        public Application() {
            _application = new MasterServerContext(new SimpleSerializer());
            
        }

        protected override PeerBase CreatePeer(InitRequest initRequest) {
            Log.Information("Test Create Peer");
            return new MMOPeer(_application, initRequest);
        }

        protected override void Setup() {
            SetUpSerilog();
            //Log.Logger = new LoggerConfiguration()
            //    .MinimumLevel.Debug()
            //    .WriteTo.Logentries("ceae825c-f44d-41de-a7b8-18bb6a358808", false, 50, null, LogEventLevel.Debug)
            //    //.WriteTo.Logentries("5e4801c4-2fca-49e0-9369-79cc7b138c79", false, 50, null, LogEventLevel.Debug)
            //    .WriteTo.Seq("http://herki.cloudapp.net:5341", LogEventLevel.Debug)
            //    .CreateLogger();

            ////// Standard .NET format string, useful if you're migrating from another logger
            //Log.Information("Starting up on {0} with {1} bytes allocated", Environment.MachineName, Environment.WorkingSet);

        }

        protected override void TearDown() {
        }

        public void SetUpSerilog()
        {
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
