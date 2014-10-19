namespace MMO.Server {
    public interface IInternalSystem {
        void Init(ClientContext client, object proxy);
    }
}