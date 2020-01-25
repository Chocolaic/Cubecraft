using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace Cubecraft.Net.Protocol
{
    class ProtocolHandler
    {
        public static bool MinecraftServiceLookup(ref string domain, ref ushort port)
        {
            if (!String.IsNullOrEmpty(domain) && domain.Any(c => char.IsLetter(c)))
            {
                try
                {
                    Console.WriteLine("Resolving {0}...", domain);
                    Heijden.DNS.Response response = new Heijden.DNS.Resolver().Query("_minecraft._tcp." + domain, Heijden.DNS.QType.SRV);
                    Heijden.DNS.RecordSRV[] srvRecords = response.RecordsSRV;
                    if (srvRecords != null && srvRecords.Any())
                    {
                        //Order SRV records by priority and weight, then randomly
                        Heijden.DNS.RecordSRV result = srvRecords
                            .OrderBy(record => record.PRIORITY)
                            .ThenByDescending(record => record.WEIGHT)
                            .ThenBy(record => Guid.NewGuid())
                            .First();
                        string target = result.TARGET.Trim('.');
                        UnityEngine.Debug.Log(String.Format("§8Found server {0}:{1} for domain {2}", target, result.PORT, domain));
                        domain = target;
                        port = result.PORT;
                        return true;
                    }
                }
                catch (Exception e)
                {
                    UnityEngine.Debug.Log(String.Format("Failed to perform SRV lookup for {0}\n{1}: {2}", domain, e.GetType().FullName, e.Message));
                }
            }
            return false;
        }
    }
}
