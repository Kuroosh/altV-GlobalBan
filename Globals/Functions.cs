using AltV.Net;
using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using VnXGlobalSystems.Models;

namespace VnXGlobalSystems.Globals
{
    public class Functions
    {
        public static Timer timer;
        public static AnticheatModel AnticheatModel;
        public static GeneralModel GeneralModel;
        public static void LoadAnticheatConfig()
        {
            try
            {
                string jsonString = File.ReadAllText(Alt.Server.Resource.Path + "/anticheat.json");
                AnticheatModel = JsonSerializer.Deserialize<AnticheatModel>(jsonString);
                Console.ResetColor();
                //Fly Notify
                if (AnticheatModel.AntiFly) { Core.Debug.OutputLog("-------- Global Systems AntiFly = [ON] --------", ConsoleColor.Green); }
                else { Core.Debug.OutputLog("-------- Global Systems AntiFly = [OFF] --------", ConsoleColor.Red); }
                //Ragdoll Notify
                if (AnticheatModel.AntiNoRagdoll) { Core.Debug.OutputLog("-------- Global Systems AntiNoRagdoll = [ON] --------", ConsoleColor.Green); }
                else { Core.Debug.OutputLog("-------- Global Systems AntiNoRagdoll = [OFF] --------", ConsoleColor.Red); }
                //CheckTeleport Notify
                if (AnticheatModel.CheckTeleport) { Core.Debug.OutputLog("-------- Global Systems CheckTeleport = [ON] --------", ConsoleColor.Green); }
                else { Core.Debug.OutputLog("-------- Global Systems CheckTeleport = [OFF] --------", ConsoleColor.Red); }
                //CheckWeapons Notify
                if (AnticheatModel.CheckWeapons) { Core.Debug.OutputLog("-------- Global Systems CheckWeapons = [ON] --------", ConsoleColor.Green); }
                else { Core.Debug.OutputLog("-------- Global Systems CheckWeapons = [OFF] --------", ConsoleColor.Red); }
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("Anticheat-Error", ex); }
        }
        public static void OnUpdate(Object unused)
        {
            try
            {
                if (Constants.NEXT_BANLIST_REFRESH <= DateTime.Now)
                {
                    Core.Debug.OutputLog("[Global Systems] : Trying to Update Global-Banlist.", ConsoleColor.White);
                    Database.Main.RefreshBanlist();
                    Constants.NEXT_BANLIST_REFRESH = DateTime.Now.AddMinutes(Constants.BANLIST_REFRESH_RATE);
                }
                if (!Functions.GeneralModel.AnticheatSystemActive) { return; }
                foreach (PlayerModel player in Main.ConnectedPlayers)
                {
                    Anticheat.Main.AntiFly(player);
                    Anticheat.Main.AntiNoRagdoll(player);
                    Anticheat.Main.CheckTeleport(player);
                    Anticheat.Main.CheckWeapons(player);
                    if (Constants.NEXT_INGAME_BAN_CHECK <= DateTime.Now) { CheckPlayerGlobalBans(player); Constants.NEXT_INGAME_BAN_CHECK = DateTime.Now.AddMinutes(Constants.INGAME_BAN_REFRESH_RATE); }
                }
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("VnX-Global-Systems:OnUpdate", ex); }
        }



        public static void LoadMainFunctions()
        {
            string jsonString = File.ReadAllText(Alt.Server.Resource.Path + "/settings.json");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("-------- VenoX Global Systems is starting... --------");
            Console.WriteLine("-------- " + Constants.VNXGLOBALSYSTEMSVERSION + " --------");
            Console.WriteLine("-------- Loading Config File.... --------");
            Console.WriteLine("---------------------------------------------");
            //
            GeneralModel = JsonSerializer.Deserialize<GeneralModel>(jsonString);
            //
            Console.ResetColor();
            if (GeneralModel.AnticheatSystemActive) { Core.Debug.OutputLog("-------- [Settings] : Anticheat Active! --------", ConsoleColor.Green); LoadAnticheatConfig(); }
            else { Core.Debug.OutputLog("-------- [Settings] : Anticheat Inactive! --------", ConsoleColor.Red); }
            if (GeneralModel.GlobalBanSystemActive) { Core.Debug.OutputLog("-------- [Settings] : Global-Ban-System Active! --------", ConsoleColor.Green); }
            else { Core.Debug.OutputLog("-------- [Settings] : Global-Ban-System not Active! --------", ConsoleColor.Red); }
            Core.Debug.OutputLog("-------- [VenoX Global Systems started] --------", ConsoleColor.DarkGreen);
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("------------------------------------------------------------------------");
            Console.ResetColor();
        }
        public static void UnloadMainFunctions()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("-------- VenoX Global Systems is stopping... --------");
            Console.WriteLine("-------- " + Constants.VNXGLOBALSYSTEMSVERSION + " --------");
            Console.WriteLine("-------- VenoX Global Systems stopped --------");
            Console.WriteLine("------------------------------------------------------------------------");
        }
        private static string Sha256(string randomString)
        {
            using SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            StringBuilder stringbuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                stringbuilder.Append(bytes[i].ToString("x2"));
            }
            return stringbuilder.ToString();
        }
        public static void CheckPlayerGlobalBans(PlayerModel player)
        {
            try
            {
                foreach (GlobalBanModel BanModel in Database.Main.GlobalBannedPlayers)
                {
                    string BanFoundBy = "";
                    bool Kick = false;
                    if (Sha256(player.DiscordID) == BanModel.PlayerDiscordID) { BanFoundBy = "Discord"; Kick = true; }
                    if (Sha256(player.HardwareIdHash.ToString()) == BanModel.PlayerHardwareId) { BanFoundBy = "HWID"; Kick = true; }
                    if (Sha256(player.HardwareIdExHash.ToString()) == BanModel.PlayerHardwareIdExHash) { BanFoundBy = "HwIdExHash"; Kick = true; }
                    if (Sha256(player.Ip.ToString()) == BanModel.PlayerIPAdress) { BanFoundBy = "IP"; Kick = true; }
                    if (Sha256(player.SocialClubId.ToString()) == BanModel.PlayerSocialClubId) { BanFoundBy = "SocialClub"; Kick = true; }

                    if (Kick)
                    {
                        Core.Debug.OutputDebugString("VenoX Global Systems : " + player.Name + " could not Connect. [" + BanFoundBy + "]");
                        player.Kick("You´re Globally Banned by VenoX Global Systems! E-Mail https://forum.altv.mp/profile/1466-solid_snake/");
                    }
                    else
                    {
                        Core.Debug.OutputLog("~~~~~~~~~~~~  [Banned]    ~~~~~~~~~~~~~~", ConsoleColor.Red);
                        Core.Debug.OutputDebugString("BanModel : DiscordID : " + BanModel.PlayerDiscordID + "| HardwareIdHash : " + BanModel.PlayerHardwareId + " | HardwareIdExHash : " + BanModel.PlayerHardwareIdExHash + " | Ip : " + BanModel.PlayerIPAdress + " | SocialClubId : " + BanModel.PlayerSocialClubId);
                        Core.Debug.OutputDebugString("--------------------------------");
                        Core.Debug.OutputLog("Name : " + player.Name, ConsoleColor.White);
                        Core.Debug.OutputLog("HWID : " + Sha256(player.HardwareIdHash.ToString()), ConsoleColor.White);
                        Core.Debug.OutputLog("HWID-ExHash : " + Sha256(player.HardwareIdExHash.ToString()), ConsoleColor.White);
                        Core.Debug.OutputLog("SocialID : " + Sha256(player.SocialClubId.ToString()), ConsoleColor.White);
                        Core.Debug.OutputLog("DiscordID : " + Sha256(player.DiscordID), ConsoleColor.White);
                        Core.Debug.OutputLog("IP-Adress : " + Sha256(player.Ip.ToString()), ConsoleColor.White);
                        Core.Debug.OutputLog("~~~~~~~~~~~~  [Banned]    ~~~~~~~~~~~~~~", ConsoleColor.Red);
                    }

                }
            }
            catch { }
        }


    }
}
