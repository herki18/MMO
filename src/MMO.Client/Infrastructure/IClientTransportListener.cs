namespace MMO.Client.Infrastructure {
    public interface IClientTransportListener {
        void TransportStatusChanged(ClientTransportStatus status);
    }
}