using AltV.Net;
using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using VnXGlobalSystems.Globals;
using VnXGlobalSystems.Models;

namespace VnXGlobalSystems.Core
{
    public class Main
    {
        public static void CheckIP(PlayerModel playerClass)
        {
            try
            {
                HttpWebRequest r = (HttpWebRequest)WebRequest.Create("http://v2.api.iphub.info/ip/" + playerClass.Ip);
                r.Method = "GET";
                r.Headers["X-key"] = Functions.GeneralModel.VPNKey;

                HttpWebResponse rs = (HttpWebResponse)r.GetResponse();
                string result = null;

                using (StreamReader sr = new StreamReader(rs.GetResponseStream())) result = sr.ReadToEnd();

                Networking obj = JsonConvert.DeserializeObject<Networking>(result);

                Debug.OutputLog("~~~~~ [VPN - Detection] ~~~~~", ConsoleColor.Cyan);
                Debug.OutputLog("~~~~~ Name :" + playerClass.Name + " ~~~~~", ConsoleColor.Cyan);
                Debug.OutputLog("~~~~~ IP : " + obj.IP + " ~~~~~", ConsoleColor.Cyan);
                Debug.OutputLog("~~~~~ CountryCode : " + obj.CountryCode + " ~~~~~", ConsoleColor.Cyan);
                Debug.OutputLog("~~~~~ CountryName : " + obj.CountryName + " ~~~~~", ConsoleColor.Cyan);
                Debug.OutputLog("~~~~~ Asn : " + obj.Asn + " ~~~~~", ConsoleColor.Cyan);
                Debug.OutputLog("~~~~~ Isp : " + obj.Isp + " ~~~~~", ConsoleColor.Cyan);

                if (obj.Block == "1")
                {
                    Debug.OutputLog("~~~~~ VPN : [True] ~~~~~", ConsoleColor.Red);
                    Alt.Emit("GlobalSystems:OnVPNConnect", playerClass, obj.IP, obj.CountryCode, obj.CountryName, obj.Asn, obj.Isp);
                }
                else
                    Debug.OutputLog("~~~~~ VPN : [False] ~~~~~", ConsoleColor.Green);

                Debug.OutputLog("~~~~~ Block : " + obj.Block + " ~~~~~", ConsoleColor.Cyan);
                Debug.OutputLog("~~~~~ Hostname : " + obj.Hostname + " ~~~~~", ConsoleColor.Cyan);
                Debug.OutputLog("~~~~~ [VPN - Detection] ~~~~~", ConsoleColor.Cyan);


                if (Constants.AWESOME_SNAKE_MODE)
                {
                    /////////////////////

                    Debug.WriteLogs("vpn", "------------------- VPN INFO -------------------");
                    Debug.WriteLogs("vpn", "~~~~~ [VPN - Detection] ~~~~~");
                    Debug.WriteLogs("vpn", "~~~~~ Name :" + playerClass.Name + " ~~~~~");
                    Debug.WriteLogs("vpn", "~~~~~ IP : " + obj.IP + " ~~~~~");
                    Debug.WriteLogs("vpn", "~~~~~ CountryCode : " + obj.CountryCode + " ~~~~~");
                    Debug.WriteLogs("vpn", "~~~~~ CountryName : " + obj.CountryName + " ~~~~~");
                    Debug.WriteLogs("vpn", "~~~~~ Asn : " + obj.Asn + " ~~~~~");
                    Debug.WriteLogs("vpn", "~~~~~ Isp : " + obj.Isp + " ~~~~~");
                    if (obj.Block == "1")
                    {
                        Debug.WriteLogs("vpn", "~~~~~ VPN : [True] ~~~~~");
                        playerClass.Kick("VPN");
                    }
                    else
                    {
                        Debug.WriteLogs("vpn", "~~~~~ VPN : [False] ~~~~~");
                    }
                    Debug.WriteLogs("vpn", "~~~~~ Block : " + obj.Block + " ~~~~~");
                    Debug.WriteLogs("vpn", "~~~~~ Hostname : " + obj.Hostname + " ~~~~~");
                    Debug.WriteLogs("vpn", "~~~~~ [VPN - Detection] ~~~~~");

                    Debug.WriteLogs("vpn", "------------------- VPN INFO -------------------");
                }
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
    }
}
