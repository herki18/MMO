using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MMO.CJR.Framework
{
    public interface IConnectionCollection <Server, Client>
    {

        #region Server interface

        void OnConnect(Server serverPeer);
        void OnDisconnect(Server serverPeer);

        Server GetServerByType(int serverType);

        #endregion

        #region Client interface

        void OnClientConnect(Client clientPeer);
        void OnClientDisconnect(Client clientPeer);

        #endregion
    }
}
