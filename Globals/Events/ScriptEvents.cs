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
                if (Functions.GeneralModel.VPNSystemActive) { Core.Main.CheckIP(player); }
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
                //Core.Debug.OutputDebugString(" Entity : " + entity);
                if (entity is PlayerModel target)
                {
                    if (target == null) { return; }
                    if (!Functions.AnticheatModel.AntiGodmode) { return; }
                    WeaponSync.WeaponDamage(player, target, weapon, dmg, offset, bodypart);
                    Alt.Emit("GlobalSystems:OnEntityHit", player, target);
                }
                else if (entity is VehicleModel vehicle)
                {
                    WeaponSync.OnVehicleDamage(player, vehicle, weapon);
                }
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }

        [ScriptEvent(ScriptEventType.PlayerDead)]
        public static void OnPlayerDeath(PlayerModel player, IEntity entity, uint reason)
        {
            try { player.RemoveAllPlayerWeapons(); }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }

    }
}
