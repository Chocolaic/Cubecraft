using Cubecraft.Net.Protocol;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Global
{
    public static SessionToken sessionToken;
    public static string clientID;
    public static bool state;
    public static string currentServerHost;
    public static int currentServerPort;
    public static int protocolVersion = Protocol.MC1122Version;
}
