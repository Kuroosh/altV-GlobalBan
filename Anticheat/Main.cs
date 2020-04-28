using System;
using VnXGlobalSystems.Globals;
using VnXGlobalSystems.Models;

namespace VnXGlobalSystems.Anticheat
{
    public class Main
    {
        private static int GetMaxTeleportVehicleDistance(PlayerModel player)
        {
            try
            {
                if (Constants.Helicopter.Contains((AltV.Net.Enums.VehicleModel)player.Vehicle.Model) || Constants.Planes.Contains((AltV.Net.Enums.VehicleModel)player.Vehicle.Model))
                {
                    return Constants.TELEPORT_KICK_FLYVEHICLE;
                }
                else { return Constants.TELEPORT_KICK_VEHICLE; }
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("GetMaxTeleportVehicleDistance", ex); return Constants.TELEPORT_KICK_VEHICLE; }
        }
        public static void AntiNoRagdoll(PlayerModel playerClass)
        {
            if (!Functions.AnticheatModel.AntiNoRagdoll) { return; }
            try { playerClass.Emit("VnXGlobalSystemsClient:SetPedCanRagdoll", true); }
            catch (Exception ex) { Core.Debug.CatchExceptions("[Anticheat-Error] : NoRagdoll", ex); }
        }
        public static void AntiFly(PlayerModel playerClass)
        {
            if (!Functions.AnticheatModel.AntiFly) { return; }
            try
            {
                if (playerClass.IsInVehicle) { return; }
                else if (playerClass.NextFlyUpdate >= DateTime.Now) { return; }
                else if (playerClass.Position.Z > 150) { return; }
                if (playerClass.EntityIsFlying)
                {
                    if (playerClass.FlyTicks > 5)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Core.Debug.OutputDebugString("[INFO] : " + playerClass.Name + " got kicked! Reason : Fly-Anticheat!");
                        Console.ResetColor();
                        string reason = "[VenoX Global Systems " + Constants.VNXGLOBALSYSTEMSVERSION + "] : Kicked by Anticheat";
                        playerClass.Log(reason);
                        playerClass.Kick(reason);
                    }
                    playerClass.NextFlyUpdate = DateTime.Now.AddSeconds(3);
                }
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("[Anticheat-Error] : AntiFly", ex); }
        }
        public static void CheckTeleport(PlayerModel playerClass)
        {
            if (!Functions.AnticheatModel.CheckTeleport) { return; }
            try
            {
                if (playerClass.IsInVehicle)
                {
                    if (playerClass.Position.Distance(playerClass.LastPosition) > GetMaxTeleportVehicleDistance(playerClass))
                    {
                        Core.Debug.OutputDebugString(playerClass.Name + " is in a Vehicle. Distance : " + playerClass.Position.Distance(playerClass.LastPosition));
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Core.Debug.OutputDebugString("[INFO] : " + playerClass.Name + " got kicked! Reason : Vehicle-Teleport-Anticheat!");
                        Console.ResetColor();
                        string reason = "[VenoX Global Systems " + Constants.VNXGLOBALSYSTEMSVERSION + "] : Kicked by Anticheat";
                        playerClass.Log(reason);
                        playerClass.Kick(reason);
                    }
                }
                else
                {
                    if (playerClass.Position.Distance(playerClass.LastPosition) > Constants.TELEPORT_KICK_FOOT)
                    {
                        if (playerClass.Position.Z < 150)
                        {
                            Console.ForegroundColor = ConsoleColor.Yellow;
                            Core.Debug.OutputDebugString(playerClass.Name + " is on foot. Distance : " + playerClass.Position.Distance(playerClass.LastPosition));
                            Core.Debug.OutputDebugString("[INFO] : " + playerClass.Name + " got kicked! Reason : Teleport-Anticheat!");
                            Console.ResetColor();
                            string reason = "[VenoX Global Systems " + Constants.VNXGLOBALSYSTEMSVERSION + "] : Kicked by Anticheat";
                            playerClass.Log(reason);
                            playerClass.Kick(reason);
                        }
                    }
                }
                playerClass.LastPosition = playerClass.Position;
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("[Anticheat-Error] : CheckTeleport", ex); }
        }
        public static void CheckWeapons(PlayerModel playerClass)
        {
            if (!Functions.AnticheatModel.CheckWeapons) { return; }
            try
            {
                if (playerClass.Weapon == (uint)AltV.Net.Enums.WeaponModel.Fist) { return; }

                if (!playerClass.Weapons.Contains((AltV.Net.Enums.WeaponModel)playerClass.Weapon))
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Core.Debug.OutputDebugString("[INFO] : " + playerClass.Name + " got kicked! Reason : Weapon-Anticheat!");
                    Console.ResetColor();
                    string reason = "[VenoX Global Systems " + Constants.VNXGLOBALSYSTEMSVERSION + "] : Kicked by Anticheat";
                    playerClass.RemoveAllWeapons();
                    playerClass.Log(reason);
                    playerClass.Kick(reason);
                }

            }
            catch (Exception ex) { Core.Debug.CatchExceptions("[Anticheat-Error] : CheckWeapons", ex); }
        }
    }
}
