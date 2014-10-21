namespace MMO.Server {
    public class PlayerContext : ClientContext {
        public PlayerContext(ServerContext application, IServerTransport transport)
            : base(application, application.PlayerSystemComponentMap, transport) {
            application.InitPlayerContext(this);
        }
    }
}