using AltV.Net;
using AltV.Net.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
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
            LoadPrivacyAccounts();
            if (Constants.AWESOME_SNAKE_MODE)
            {
                Core.Debug.WriteLogs("debug", "Server Started with Debug Mode true!");
            }
        }
        public static void OnResourceStop()
        {
            Functions.UnloadMainFunctions();
        }

        public static void LoadPrivacyAccounts()
        {
            List<PrivacyModel> privacyClasses = JsonConvert.DeserializeObject<List<PrivacyModel>>(File.ReadAllText(Alt.Server.Resource.Path + "/settings/privacy.json"));
            if (privacyClasses.Count <= 0) { return; }
            foreach (PrivacyModel privacyUser in privacyClasses)
            {
                Constants.PrivacyAcceptedPlayers.Add(privacyUser);
            }
        }
        public static bool PlayerAcceptedPrivacyPolicy(PlayerModel player)
        {
            try
            {
                foreach (PrivacyModel privacyClass in Constants.PrivacyAcceptedPlayers)
                {
                    if (privacyClass.HardwareId == player.HardwareIdHash.ToString() || privacyClass.HardwareIdExHash == player.HardwareIdExHash.ToString() || privacyClass.IP == player.Ip.ToString() || privacyClass.SocialID == player.SocialClubId.ToString())
                    {
                        return true;
                    }
                }
                return false;
            }
            catch { return false; }
        }

        ////////////////////////// Player Events /////////////////////////////////////////////////////////
        [ScriptEvent(ScriptEventType.PlayerConnect)]
        public static void OnPlayerConnect(PlayerModel player, string reason)
        {
            try
            {
                if (Functions.GeneralModel.VPNSystemActive) { Core.Main.CheckIP(player); }
                player.Emit("VnXGlobalSystemsClient:GetDiscordID");
                Functions.CheckPlayerGlobalBans(player);
                Main.ConnectedPlayers.Add(player);
                if (!PlayerAcceptedPrivacyPolicy(player))
                {
                    player.Emit("VnXGlobalSystemsClient:ShowPrivacyPolicy");
                    return;
                }
                Alt.Emit("GlobalSystems:PlayerReady", player);
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
            try { if (target == null) { return; } if (!Functions.AnticheatModel.AntiGodmode) { return; } WeaponSync.WeaponDamage(player, target, weapon, dmg, offset, bodypart); Alt.Emit("GlobalSystems:OnEntityHit", player, target); }
            catch (Exception ex) { Core.Debug.CatchExceptions("WeaponDamage", ex); }
        }

        [ClientEvent("VnXGlobalSystems:SetDiscordID")]
        public static void UpdateDiscordInfo(PlayerModel player, string DiscordID) => player.UpdateDiscordInfo(DiscordID);

        [ClientEvent("VnXGlobalSystems:KickPlayer")]
        public static void KickPlayer(PlayerModel player) => player?.Kick(reason: String.Empty);

        [ClientEvent("VnXGlobalSystems:PrivacyPolicy")]
        public static void SetPrivacyPolicy(PlayerModel player, int privacy) => player?.SetPrivacyPolicy(privacy);

        ////////////////////////// Weapon Anticheat /////////////////////////////////////////////////////////
        [ServerEvent("GlobalSystems:GiveWeapon")]
        public static void OnWeaponEventCall(PlayerModel player, uint WeaponHash, int ammo, bool selectWeapon) => player.GivePlayerWeapon(WeaponHash, ammo, selectWeapon);

        [ServerEvent("GlobalSystems:RemovePlayerWeapon")]
        public static void RemovePlayerWeapon(PlayerModel player, uint WeaponHash) => player.RemovePlayerWeapon(WeaponHash);

        [ServerEvent("GlobalSystems:RemoveAllPlayerWeapons")]
        public static void RemoveAllPlayerWeapon(PlayerModel player) => player.RemoveAllPlayerWeapon();

        ////////////////////////// Player Anticheat /////////////////////////////////////////////////////////
        [ServerEvent("GlobalSystems:PlayerPosition")]
        public static void PlayerPosition(PlayerModel player, Vector3 position) => player.Position(position);

        [ServerEvent("GlobalSystems:PlayerTeam")]
        public static void Playerteam(PlayerModel player, int TeamId) => player.SetPlayerTeam(TeamId);


    }
    public static class EventFunctions
    {
        public static void SetPlayerTeam(this PlayerModel player, int TeamId)
        {
            try
            {
                player.Team = TeamId;
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("Global-Systems:SetPlayerTeam", ex); }
        }
        public static void SetPrivacyPolicy(this PlayerModel player, int privacy)
        {
            try
            {
                switch (privacy)
                {
                    case 0:
                        player.KickPlayer(String.Empty);
                        return;
                    case 1:
                        PrivacyModel privacyClass = new PrivacyModel
                        {
                            Name = player.Name,
                            HardwareId = player.HardwareIdHash.ToString(),
                            HardwareIdExHash = player.HardwareIdExHash.ToString(),
                            IP = player.Ip.ToString(),
                            SocialID = player.SocialClubId.ToString()
                        };
                        string Json = JsonConvert.SerializeObject(privacyClass);
                        Core.Debug.WriteJsonString("privacy", Json);
                        Constants.PrivacyAcceptedPlayers.Add(privacyClass);
                        player.Emit("VnXGlobalSystemsClient:HidePrivacyPolicy");
                        Alt.Emit("GlobalSystems:PlayerReady", player);
                        return;
                }
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("Global-Systems:SetPrivacyPolicy", ex); }
        }
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
                if (player.DiscordID.Length > 0) { return; }
                player.DiscordID = DiscordID;
                Core.Debug.OutputDebugString("Discord ID [" + player.Name + "] : " + DiscordID);
                Functions.CheckPlayerGlobalBans(player);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("Global-Systems:UpdateDiscordInfo", ex); }
        }
        public static void GivePlayerWeapon(this PlayerModel player, uint WeaponHash, int ammo, bool selectWeapon)
        {
            try { if (player == null) { return; } player.Weapons.Add(WeaponHash); player.GiveWeapon(WeaponHash, ammo, selectWeapon); }
            catch (Exception ex) { Core.Debug.CatchExceptions("Global-Systems:GivePlayerWeapon", ex); }
        }

        public static void RemovePlayerWeapon(this PlayerModel player, uint WeaponHash)
        {
            try { if (player == null) { return; } player.RemoveWeapon(WeaponHash); player.Weapons.Remove(WeaponHash); }
            catch (Exception ex) { Core.Debug.CatchExceptions("Global-Systems:RemovePlayerWeapon", ex); }
        }

        public static void RemoveAllPlayerWeapon(this PlayerModel player)
        {
            try { if (player == null) { return; } player.RemoveAllWeapons(); player.Weapons.Clear(); }
            catch (Exception ex) { Core.Debug.CatchExceptions("Global-Systems:RemoveAllPlayerWeapon", ex); }
        }

        public static void Position(this PlayerModel player, Vector3 position)
        {
            try { if (player == null) { return; } player.LastPosition = position; player.Position = position; }
            catch (Exception ex) { Core.Debug.CatchExceptions("Global-Systems:Position", ex); }
        }
    }
}
