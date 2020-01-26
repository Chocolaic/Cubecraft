using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cubecraft.Net.Protocol
{
    interface INetworkHandler
    {
        void OnConnectionLost(DisconnectReason reason, string msg);
    }
}
