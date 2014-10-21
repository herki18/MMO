using MMO.Base.Components.Systems;

namespace MMO.Server.Master.Systems {
    public class LoginSystem : PlayerSystemBase<ILoginSystemServer, ILoginSystemClient>, ILoginSystemServer {
        public void BasicTest(string message) {
            throw new System.NotImplementedException();
        }
    }
}