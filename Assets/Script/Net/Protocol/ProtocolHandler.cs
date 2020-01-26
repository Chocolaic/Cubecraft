using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Cubecraft.Net.Protocol
{
    public delegate void HttpCallBack(bool isSuccess, string result);
    internal delegate void LoginCallBack(ProtocolHandler.LoginResult result, SessionToken session);
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
        public enum LoginResult { OtherError, ServiceUnavailable, SSLError, Success, WrongPassword, AccountMigrated, NotPremium, LoginRequired, InvalidToken, InvalidResponse, NullError };
        public static void GetLogin(string user, string pass, LoginCallBack call)
        {
            Global.clientID = Guid.NewGuid().ToString().Replace("-", "");
            SessionToken session = new SessionToken();
            try
            {
                string json_request = "{\"agent\": { \"name\": \"Minecraft\", \"version\": 1 }, \"username\": \"" + JsonEncode(user) + "\", \"password\": \"" + JsonEncode(pass) + "\", \"clientToken\": \"" + JsonEncode(Global.clientID) + "\" }";
                DoHTTPSPost("https://authserver.mojang.com/authenticate", json_request, (bool isSuccess, string result) =>
                {
                    if (isSuccess)
                    {
                        if (result.Contains("availableProfiles\":[]}"))
                        {
                            call(LoginResult.NotPremium, session);
                        }
                        else
                        {
                            session = JsonUtility.FromJson<SessionToken>(result); 
                            if (!string.IsNullOrEmpty(session.accessToken))
                            {
                                call(LoginResult.Success, session);
                            }
                        }
                    }
                });
            }
            catch(Exception e)
            {
                UnityEngine.Debug.Log(e.Message);
            }
            call(LoginResult.InvalidResponse, session);
        }
        private async static void DoHTTPSPost(string url, string data, HttpCallBack call)
        {
            UnityEngine.Debug.Log("Post to " + url);
            HttpWebRequest request;
            if(url.StartsWith("https", StringComparison.OrdinalIgnoreCase))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback((object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors) =>
                {
                    return true;
                });
            }
            request = WebRequest.Create(url) as HttpWebRequest;
            request.Method = "POST";
            try
            {
                byte[] postData = Encoding.UTF8.GetBytes(data);
                request.ContentLength = postData.Length;
                var writer = request.GetRequestStream();
                writer.Write(postData, 0, postData.Length);
                writer.Close();

                HttpWebResponse response = await request.GetResponseAsync() as HttpWebResponse;
                using(StreamReader reader = new StreamReader(response.GetResponseStream()))
                {
                    string result = reader.ReadToEnd();
                    response.Close();
                    call(true, result);
                }
            }
            catch (Exception e)
            {
                call(false, e.Message);
            }
        }
        private static string JsonEncode(string text)
        {
            StringBuilder result = new StringBuilder();

            foreach (char c in text)
            {
                if ((c >= '0' && c <= '9') ||
                    (c >= 'a' && c <= 'z') ||
                    (c >= 'A' && c <= 'Z'))
                {
                    result.Append(c);
                }
                else
                {
                    result.AppendFormat(@"\u{0:x4}", (int)c);
                }
            }

            return result.ToString();
        }
    }
}
