using System.Collections.Generic;
using ExitGames.Client.Photon;
using UnityEngine;
using System.Collections;

public class TestPhoton : MonoBehaviour, IPhotonPeerListener {
    private PhotonPeer _peer;
    private List<PhotonPeer> _regionServers; 

    public void Awake() {
        _regionServers = new List<PhotonPeer>();
        _peer = new PhotonPeer(this, ConnectionProtocol.Udp);
        _peer.Connect("168.63.57.205:5055", "MMOMaster");
        Debug.Log("Connected Photon");
    }

    public void Update() {
        _peer.Service();
        foreach (var regionServer in _regionServers)
        {
            regionServer.Service();
        }

    }

    public void OnApplicationQuit()
    {
        _peer.Disconnect();

        foreach (var regionServer in _regionServers) {
            regionServer.Disconnect();
        }
    }

    public void DebugReturn(DebugLevel level, string message) {
    }

    public void OnOperationResponse(OperationResponse operationResponse) {
    }

    public void OnStatusChanged(StatusCode statusCode) {
        Debug.Log(string.Format("Status Changed: {0}", statusCode));
    }

    public void OnEvent(EventData eventData) {
        Debug.Log(eventData.Parameters[0].ToString());
        var regionServers = eventData.Parameters[0].ToString().Split(',');
        Debug.Log(regionServers);
        foreach (var server in regionServers)
        {
            var regionServer = new PhotonPeer(this, ConnectionProtocol.Udp);
            regionServer.Connect(server, "MMORegion");
            _regionServers.Add(regionServer);
        }
    }
}
