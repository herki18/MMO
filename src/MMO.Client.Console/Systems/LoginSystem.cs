using log4net;
using log4net.Core;
using MMO.Base.Components.Systems;

namespace MMO.Client.Console.Systems {
    public class LoginSystem : ConsoleSystemBase<ILoginSystemServer, ILoginSystemClient>, ILoginSystemClient {

        private static readonly ILog Log = LogManager.GetLogger(typeof (LoginSystem));

        protected override void Awake() {
            Log.InfoFormat("Created Login system");
        }

        public void BasicClientTest(string message) {
            Log.InfoFormat("Got Message: {0}", message);
        }
    }
}