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
        public static Timer timer = new Timer(OnUpdate, null, Constants.UPDATEINTERVAL, Constants.UPDATEINTERVAL);
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
                foreach (PlayerModel player in Main.ConnectedPlayers)
                {
                    Anticheat.Main.AntiFly(player);
                    Anticheat.Main.AntiNoRagdoll(player);
                    Anticheat.Main.CheckTeleport(player);
                    Anticheat.Main.CheckWeapons(player);
                }
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("VnX-Global-Systems:OnUpdate", ex); }
        }



        public static void LoadMainFunctions()
        {
            //Console.BackgroundColor = ConsoleColor.DarkMagenta;
            string jsonString = File.ReadAllText(Alt.Server.Resource.Path + "/settings.json");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("-------- VenoX Global Systems is starting...");
            Console.WriteLine("-------- " + Constants.VNXGLOBALSYSTEMSVERSION + " --------");
            Console.WriteLine("-------- Loading Config File.... --------");
            Console.WriteLine("---------------------------------------------");
            //
            GeneralModel GeneralModel = JsonSerializer.Deserialize<GeneralModel>(jsonString);
            //
            if (GeneralModel.AnticheatSystemActive) { Core.Debug.OutputLog("-------- [Settings] : Anticheat Active! --------", ConsoleColor.Green); LoadAnticheatConfig(); }
            else { Core.Debug.OutputLog("-------- [Settings] : Anticheat Inactive! --------", ConsoleColor.Red); }
            if (GeneralModel.GlobalBanSystemActive) { Core.Debug.OutputLog("-------- [Settings] : Global-Ban-System Active! --------", ConsoleColor.Green); }
            else { Core.Debug.OutputLog("-------- [Settings] : Global-Ban-System not Active! --------", ConsoleColor.Red); }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("-------- VenoX Global Systems started --------");
            Console.WriteLine("------------------------------------------------------------------------");
        }
        public static void UnloadMainFunctions()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("-------- VenoX Global Systems is stopping...");
            Console.WriteLine("-------- " + Constants.VNXGLOBALSYSTEMSVERSION + " --------");
            Console.WriteLine("-------- VenoX Global Systems stopped --------");
            Console.WriteLine("------------------------------------------------------------------------");
        }
        public static PlayerModel FindPlayerClassByPlayer(PlayerModel player)
        {
            try
            {
                return player;
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("FindPlayerClassByPlayer", ex); return null; }
        }
    }
}
