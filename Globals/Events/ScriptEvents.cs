using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using System;
using VnXGlobalSystems.Models;

namespace VnXGlobalSystems.Globals
{
    public class ScriptEvents : IScript
    {
        [ScriptEvent(ScriptEventType.PlayerConnect)]
        public static void OnPlayerConnect(PlayerModel player, string reason)
        {
            try
            {
                if (Functions.GeneralModel.VPNSystemActive) 
                    Core.Main.CheckIP(player);
                if (Functions.AnticheatModel.CheckClothes) 
                    player.Emit("VnXGlobalSystemsClient:SyncClothes", true);
                
                player.Emit("VnXGlobalSystemsClient:GetDiscordID");
                Functions.CheckPlayerGlobalBans(player);
                Main.ConnectedPlayers.Add(player);
                if (!Events.PlayerAcceptedPrivacyPolicy(player))
                {
                    player.Emit("VnXGlobalSystemsClient:ShowPrivacyPolicy");
                    return;
                }
                Alt.Emit("GlobalSystems:PlayerReady", player);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }

        [ScriptEvent(ScriptEventType.PlayerDisconnect)]
        public static void OnPlayerDisconnect(PlayerModel player, string reason)
        {
            try { Main.ConnectedPlayers.Remove(player); }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }

        [ScriptEvent(ScriptEventType.WeaponDamage)]
        public static void WeaponDamage(PlayerModel player, IEntity entity, uint weapon, ushort dmg, Position offset, BodyPart bodypart)
        {
            try
            {
                if (!Functions.AnticheatModel.AntiGodmode) return;
                if (entity is null || !entity.Exists) return;
                switch (entity)
                {
                    case PlayerModel target:
                        WeaponSync.WeaponDamage(player, target, weapon, dmg, offset, bodypart);
                        break;
                    case VehicleModel vehicle:
                        WeaponSync.OnVehicleDamage(player, vehicle, weapon);
                        break;
                }
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }

        [ScriptEvent(ScriptEventType.PlayerEvent)]
        public static void OnServerEventReceive(PlayerModel player, string EventName, params object[] args)
        {
            try
            {
                player.EventCallCounter++;
                if (player.EventCallCounter < Constants.PLAYER_KICK_AFTER_EMITS) return;
                Core.Debug.WriteLogs("Events", " Name : " + player.Name + " | SCID : " + player.SocialClubId + " called Event[" + player.EventCallCounter + "] : " + EventName + " | args : " + string.Join(", ", args));
                Anticheat.Main.CheckEmits(player);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
    }
}
