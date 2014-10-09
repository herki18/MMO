using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Photon.SocketServer;

namespace MMO.Server.Region
{
    public class Application : ApplicationBase
    {
        protected override PeerBase CreatePeer(InitRequest initRequest)
        {
            return new Peer(initRequest);
        }

        protected override void Setup()
        {
        }

        protected override void TearDown()
        {
        }
    }
}
