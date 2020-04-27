using AltV.Net;
using System;
using System.IO;
using System.Text.Json;
using System.Threading;
using VenoX_Global_Systems._Models_;

namespace VenoX_Global_Systems._Globals_
{
    public class Functions
    {
        public static Timer timer = new Timer(OnUpdate, null, Constants.UPDATE_INTERVAL, Constants.UPDATE_INTERVAL);
        public static AnticheatModel AnticheatModel;
        public static void LoadAnticheatConfig()
        {
            try
            {
                string jsonString = File.ReadAllText(Alt.Server.Resource.Path + "/anticheat.json");
                AnticheatModel = JsonSerializer.Deserialize<AnticheatModel>(jsonString);
                Console.ResetColor();
                //Fly Notify
                if (AnticheatModel.AntiFly) { _Core_.Debug.OutputLog("-------- Global Systems AntiFly = [ON] --------", ConsoleColor.Green); }
                else { _Core_.Debug.OutputLog("-------- Global Systems AntiFly = [OFF] --------", ConsoleColor.Red); }
                //Ragdoll Notify
                if (AnticheatModel.AntiNoRagdoll) { _Core_.Debug.OutputLog("-------- Global Systems AntiNoRagdoll = [ON] --------", ConsoleColor.Green); }
                else { _Core_.Debug.OutputLog("-------- Global Systems AntiNoRagdoll = [OFF] --------", ConsoleColor.Red); }
                //CheckTeleport Notify
                if (AnticheatModel.CheckTeleport) { _Core_.Debug.OutputLog("-------- Global Systems CheckTeleport = [ON] --------", ConsoleColor.Green); }
                else { _Core_.Debug.OutputLog("-------- Global Systems CheckTeleport = [OFF] --------", ConsoleColor.Red); }
                //CheckWeapons Notify
                if (AnticheatModel.CheckWeapons) { _Core_.Debug.OutputLog("-------- Global Systems CheckWeapons = [ON] --------", ConsoleColor.Green); }
                else { _Core_.Debug.OutputLog("-------- Global Systems CheckWeapons = [OFF] --------", ConsoleColor.Red); }
            }
            catch (Exception ex) { _Core_.Debug.CatchExceptions("Anticheat-Error", ex); }
        }
        public static void OnUpdate(Object unused)
        {
            try
            {
                foreach (PlayerModel player in Main.ConnectedPlayers)
                {
                    _Anticheat_.Main.AntiFly(player);
                    _Anticheat_.Main.AntiNoRagdoll(player);
                    _Anticheat_.Main.CheckTeleport(player);
                    _Anticheat_.Main.CheckWeapons(player);
                }
            }
            catch (Exception ex) { _Core_.Debug.CatchExceptions("VnX-Global-Systems:OnUpdate", ex); }
        }



        public static void LoadMainFunctions()
        {
            //Console.BackgroundColor = ConsoleColor.DarkMagenta;
            string jsonString = File.ReadAllText(Alt.Server.Resource.Path + "/settings.json");
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("-------- VenoX Global Systems is starting...");
            Console.WriteLine("-------- " + Constants.VNX_GLOBAL_SYSTEMS_VERSION + " --------");
            Console.WriteLine("-------- Loading Config File.... --------");
            Console.WriteLine("---------------------------------------------");
            //
            GeneralModel GeneralModel = JsonSerializer.Deserialize<GeneralModel>(jsonString);
            //
            if (GeneralModel.AnticheatSystemActive) { _Core_.Debug.OutputLog("-------- [Settings] : Anticheat Active! --------", ConsoleColor.Green); LoadAnticheatConfig(); }
            else { _Core_.Debug.OutputLog("-------- [Settings] : Anticheat Inactive! --------", ConsoleColor.Red); }
            if (GeneralModel.GlobalBanSystemActive) { _Core_.Debug.OutputLog("-------- [Settings] : Global-Ban-System Active! --------", ConsoleColor.Green); }
            else { _Core_.Debug.OutputLog("-------- [Settings] : Global-Ban-System not Active! --------", ConsoleColor.Red); }
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("-------- VenoX Global Systems started --------");
            Console.WriteLine("------------------------------------------------------------------------");
        }
        public static void UnloadMainFunctions()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("-------- VenoX Global Systems is stopping...");
            Console.WriteLine("-------- " + Constants.VNX_GLOBAL_SYSTEMS_VERSION + " --------");
            Console.WriteLine("-------- VenoX Global Systems stopped --------");
            Console.WriteLine("------------------------------------------------------------------------");
        }
        public static PlayerModel FindPlayerClassByPlayer(PlayerModel player)
        {
            try
            {
                return player;
            }
            catch (Exception ex) { _Core_.Debug.CatchExceptions("FindPlayerClassByPlayer", ex); return null; }
        }
    }
}
