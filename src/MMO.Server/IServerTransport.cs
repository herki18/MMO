namespace MMO.Server {
    public interface IServerTransport {
        void SendData(Event @event);
        void Disconnect();
    }
}