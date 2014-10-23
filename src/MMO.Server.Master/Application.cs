using System.IO;
using ExitGames.Logging;
using ExitGames.Logging.Log4Net;
using log4net.Config;
using MMO.Base.Infrastructure;
using Photon.SocketServer;

namespace MMO.Server.Master
{
    public class Application : ApplicationBase {
        private readonly MasterServerContext _application;

        public Application() {
            _application = new MasterServerContext(new SimpleSerializer());
        }

        protected override PeerBase CreatePeer(InitRequest initRequest) {
            return new MMOPeer(_application, initRequest);
        }

        protected override void Setup() {
            var file = new FileInfo(Path.Combine(BinaryPath, "log4net.config"));
            if (file.Exists)
            {
                LogManager.SetLoggerFactory(Log4NetLoggerFactory.Instance);
                XmlConfigurator.ConfigureAndWatch(file);

            }
        }

        protected override void TearDown() {
        }
    }
}
