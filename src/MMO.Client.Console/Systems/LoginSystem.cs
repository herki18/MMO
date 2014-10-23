using log4net;
using log4net.Core;
using MMO.Base.Components.Systems;

namespace MMO.Client.Console.Systems {
    public class LoginSystem : ConsoleSystemBase<ILoginSystemServer, ILoginSystemClient>, ILoginSystemClient {

        private static readonly ILog Log = LogManager.GetLogger(typeof (LoginSystem));

        public void BasicClientTest(string message) {
            Log.InfoFormat("Got Message: {0}", message);
        }
    }
}