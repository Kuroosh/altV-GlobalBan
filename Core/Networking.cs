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

                using (StreamReader sr = new StreamReader(rs.GetResponseStream()))
                {
                    result = sr.ReadToEnd();
                }
                Networking obj = JsonConvert.DeserializeObject<Networking>(result);
                Core.Debug.OutputLog("~~~~~ [VPN - Detection] ~~~~~", ConsoleColor.Cyan);
                Core.Debug.OutputLog("~~~~~ Name :" + playerClass.Name + " ~~~~~", ConsoleColor.Cyan);
                Core.Debug.OutputLog("~~~~~ IP : " + obj.IP + " ~~~~~", ConsoleColor.Cyan);
                Core.Debug.OutputLog("~~~~~ CountryCode : " + obj.CountryCode + " ~~~~~", ConsoleColor.Cyan);
                Core.Debug.OutputLog("~~~~~ CountryName : " + obj.CountryName + " ~~~~~", ConsoleColor.Cyan);
                Core.Debug.OutputLog("~~~~~ Asn : " + obj.Asn + " ~~~~~", ConsoleColor.Cyan);
                Core.Debug.OutputLog("~~~~~ Isp : " + obj.Isp + " ~~~~~", ConsoleColor.Cyan);
                if (obj.Block == "1")
                {
                    Core.Debug.OutputLog("~~~~~ VPN : [True] ~~~~~", ConsoleColor.Red);
                    Alt.Emit("GlobalSystems:OnVPNConnect", playerClass, obj.IP, obj.CountryCode, obj.CountryName, obj.Asn, obj.Isp);
                }
                else
                {
                    Core.Debug.OutputLog("~~~~~ VPN : [False] ~~~~~", ConsoleColor.Green);
                }
                Core.Debug.OutputLog("~~~~~ Block : " + obj.Block + " ~~~~~", ConsoleColor.Cyan);
                Core.Debug.OutputLog("~~~~~ Hostname : " + obj.Hostname + " ~~~~~", ConsoleColor.Cyan);
                Core.Debug.OutputLog("~~~~~ [VPN - Detection] ~~~~~", ConsoleColor.Cyan);


                if (Constants.AWESOME_SNAKE_MODE)
                {
                    /////////////////////

                    Core.Debug.WriteLogs("vpn", "------------------- VPN INFO -------------------");
                    Core.Debug.WriteLogs("vpn", "~~~~~ [VPN - Detection] ~~~~~");
                    Core.Debug.WriteLogs("vpn", "~~~~~ Name :" + playerClass.Name + " ~~~~~");
                    Core.Debug.WriteLogs("vpn", "~~~~~ IP : " + obj.IP + " ~~~~~");
                    Core.Debug.WriteLogs("vpn", "~~~~~ CountryCode : " + obj.CountryCode + " ~~~~~");
                    Core.Debug.WriteLogs("vpn", "~~~~~ CountryName : " + obj.CountryName + " ~~~~~");
                    Core.Debug.WriteLogs("vpn", "~~~~~ Asn : " + obj.Asn + " ~~~~~");
                    Core.Debug.WriteLogs("vpn", "~~~~~ Isp : " + obj.Isp + " ~~~~~");
                    if (obj.Block == "1")
                    {
                        Core.Debug.WriteLogs("vpn", "~~~~~ VPN : [True] ~~~~~");
                        playerClass.Kick("VPN");
                    }
                    else
                    {
                        Core.Debug.WriteLogs("vpn", "~~~~~ VPN : [False] ~~~~~");
                    }
                    Core.Debug.WriteLogs("vpn", "~~~~~ Block : " + obj.Block + " ~~~~~");
                    Core.Debug.WriteLogs("vpn", "~~~~~ Hostname : " + obj.Hostname + " ~~~~~");
                    Core.Debug.WriteLogs("vpn", "~~~~~ [VPN - Detection] ~~~~~");

                    Core.Debug.WriteLogs("vpn", "------------------- VPN INFO -------------------");
                }
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("CheckIP", ex); }
        }
    }
}
