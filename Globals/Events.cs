using AltV.Net;
using AltV.Net.Data;
using System;
using System.Numerics;
using System.Threading;
using VnXGlobalSystems.Models;

namespace VnXGlobalSystems.Globals
{
    public class Events : IScript
    {
        ////////////////////////// Resource Start/Stop /////////////////////////////////////////////////////////
        public static void OnResourceStart()
        {
            Functions.timer = new Timer(Functions.OnUpdate, null, Constants.UPDATEINTERVAL, Constants.UPDATEINTERVAL);
            Functions.LoadMainFunctions();
            Database.Main.OnResourceStart();
        }
        public static void OnResourceStop()
        {
            Functions.UnloadMainFunctions();
        }

        ////////////////////////// Player Events /////////////////////////////////////////////////////////
        [ScriptEvent(ScriptEventType.PlayerConnect)]
        public static void OnPlayerConnect(PlayerModel player, string reason)
        {
            try
            {
                player.Emit("VnXGlobalSystemsClient:GetDiscordID");
                Functions.CheckPlayerGlobalBans(player);
                Main.ConnectedPlayers.Add(player);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("OnPlayerConnect", ex); }
        }

        [ScriptEvent(ScriptEventType.PlayerDisconnect)]
        public static void OnPlayerDisconnect(PlayerModel player, string reason)
        {
            try
            {
                Main.ConnectedPlayers.Remove(player);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("OnPlayerDisconnect", ex); }
        }

        [ScriptEvent(ScriptEventType.WeaponDamage)]
        public static void WeaponDamage(PlayerModel player, PlayerModel target, uint weapon, ushort dmg, Position offset, BodyPart bodypart)
        {
            try { WeaponSync.WeaponDamage(player, target, weapon, dmg, offset, bodypart); Alt.Emit("GlobalSystems:OnEntityHit", player, target); }
            catch { }
        }

        [ClientEvent("VnXGlobalSystems:SetDiscordID")]
        public static void UpdateDiscordInfo(PlayerModel player, string DiscordID) => player.UpdateDiscordInfo(DiscordID);

        [ClientEvent("VnXGlobalSystems:KickPlayer")]
        public static void KickPlayer(PlayerModel player) => player?.Kick(reason: String.Empty);

        ////////////////////////// Weapon Anticheat /////////////////////////////////////////////////////////
        [ServerEvent("GlobalSystems:GiveWeapon")]
        public static void OnWeaponEventCall(PlayerModel player, uint WeaponHash, byte ammo, bool selectWeapon) => player.GivePlayerWeapon(WeaponHash, ammo, selectWeapon);

        [ServerEvent("GlobalSystems:RemovePlayerWeapon")]
        public static void RemovePlayerWeapon(PlayerModel player, uint WeaponHash) => player.RemovePlayerWeapon(WeaponHash);

        [ServerEvent("GlobalSystems:RemoveAllPlayerWeapons")]
        public static void RemoveAllPlayerWeapon(PlayerModel player) => player.RemoveAllPlayerWeapon();

        ////////////////////////// Player Anticheat /////////////////////////////////////////////////////////
        [ServerEvent("GlobalSystems:PlayerPosition")]
        public static void PlayerPosition(PlayerModel player, Vector3 position) => player.Position(position);

    }
    public static class EventFunctions
    {
        public static void KickPlayer(this PlayerModel player, string reason)
        {
            try
            {
                player.Emit("VnXGlobalSystemsClient:Kick", reason);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("Global-Systems:Kick", ex); }
        }
        public static void KickGlobal(this PlayerModel player)
        {
            try
            {
                player.Emit("VnXGlobalSystemsClient:KickGlobal");
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("Global-Systems:KickGlobal", ex); }
        }
        public static void UpdateDiscordInfo(this PlayerModel player, string DiscordID)
        {
            try
            {
                player.DiscordID = DiscordID;
                //Core.Debug.OutputDebugString("Discord ID [" + player.Name + "] : " + DiscordID);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("Global-Systems:UpdateDiscordInfo", ex); }
        }
        public static void GivePlayerWeapon(this PlayerModel player, uint WeaponHash, byte ammo, bool selectWeapon)
        {
            try { player.GiveWeapon(WeaponHash, ammo, selectWeapon); player.Weapons.Add(WeaponHash); }
            catch (Exception ex) { Core.Debug.CatchExceptions("Global-Systems:GivePlayerWeapon", ex); }
        }

        public static void RemovePlayerWeapon(this PlayerModel player, uint WeaponHash)
        {
            try { player.RemoveWeapon(WeaponHash); player.Weapons.Remove(WeaponHash); }
            catch (Exception ex) { Core.Debug.CatchExceptions("Global-Systems:RemovePlayerWeapon", ex); }
        }

        public static void RemoveAllPlayerWeapon(this PlayerModel player)
        {
            try { player.RemoveAllWeapons(); player.Weapons.Clear(); }
            catch (Exception ex) { Core.Debug.CatchExceptions("Global-Systems:RemoveAllPlayerWeapon", ex); }
        }

        public static void Position(this PlayerModel player, Vector3 position)
        {
            try { player.LastPosition = position; player.Position = position; }
            catch (Exception ex) { Core.Debug.CatchExceptions("Global-Systems:Position", ex); }
        }
    }
}
