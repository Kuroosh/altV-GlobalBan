using AltV.Net;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using VnXGlobalSystems.Models;

namespace VnXGlobalSystems.Globals
{
    public class Functions
    {
        public static Timer timer;
        public static AnticheatModel AnticheatModel;
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
            GeneralModel GeneralModel = JsonSerializer.Deserialize<GeneralModel>(jsonString);
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

        public static void CheckPlayerGlobalBans(PlayerModel player)
        {
            try
            {
                foreach (GlobalBanModel BanModel in Database.Main.GlobalBannedPlayers)
                {
                    string BanFoundBy = "";
                    bool Kick = false;
                    if (player.DiscordID == BanModel.PlayerDiscordID) { BanFoundBy = "Discord"; Kick = true; }
                    if (player.HardwareIdHash.ToString() == BanModel.PlayerHardwareId) { BanFoundBy = "HWID"; Kick = true; }
                    if (player.HardwareIdExHash.ToString() == BanModel.PlayerHardwareIdExHash) { BanFoundBy = "HwIdExHash"; Kick = true; }
                    //if (player.Ip.ToString() == BanModel.PlayerIPAdress) { BanFoundBy = "IP"; Kick = true; }
                    if (player.SocialClubId.ToString() == BanModel.PlayerSocialClubId) { BanFoundBy = "SocialClub"; Kick = true; }

                    if (Kick)
                    {
                        Core.Debug.OutputDebugString("VenoX Global Systems : " + player.Name + " could not Connect. [" + BanFoundBy + "]");
                        player.Kick("Your Globally Banned by VenoX Global Systems! E-Mail vnx_solidsnake@gmail.com");
                    }
                    else
                    {
                        Core.Debug.OutputDebugString("---------------- " + BanModel.PlayerName + " ----------------");
                        Core.Debug.OutputDebugString("BanModel : DiscordID : " + BanModel.PlayerDiscordID + "| HardwareIdHash : " + BanModel.PlayerHardwareId + " | HardwareIdExHash : " + BanModel.PlayerHardwareIdExHash + " | Ip : " + BanModel.PlayerIPAdress + " | SocialClubId : " + BanModel.PlayerSocialClubId);
                        Core.Debug.OutputDebugString("--------------------------------------------------------------------");
                        Core.Debug.OutputDebugString("Player : DiscordID : " + player.DiscordID + "| HardwareIdHash : " + player.HardwareIdHash + " | HardwareIdExHash : " + player.HardwareIdExHash + " | Ip : " + player.Ip + " | SocialClubId : " + player.SocialClubId);
                    }

                }
            }
            catch { }
        }


    }
}
