﻿using AltV.Net;
using Newtonsoft.Json;
using System;
using System.Numerics;
using VnXGlobalSystems.Models;

namespace VnXGlobalSystems.Globals
{
    public static class EventFunctions
    {
        /* Event Functions */
        public static void SetPlayerClothes(this PlayerModel player, int slot, int drawable, int texture)
        {
            try
            {
                if (player is null || !player.Exists) return;
                player.Emit("VnXGlobalSystemsClient:SetClothes", slot, drawable, texture);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
        public static void SetPlayerProps(this PlayerModel player, int slot, int drawable, int texture)
        {
            try
            {
                if (player is null || !player.Exists) return;
                player.Emit("VnXGlobalSystemsClient:SetProps", slot, drawable, texture);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
        public static void SetPlayerGodmode(this PlayerModel player, bool EntityGodmode)
        {
            try
            {
                if (player is null || !player.Exists) return;
                player.EntityGodmode = EntityGodmode;
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
        public static void SetPlayerProofs(this PlayerModel player, bool BulletProof, bool FireProof, bool ExplosionProof, bool CollisionProof, bool MeleeProof, bool DrownProof)
        {
            try
            {
                if (player is null || !player.Exists) return;
                player.Proofs.BulletProof = BulletProof;
                player.Proofs.FireProof = FireProof;
                player.Proofs.ExplosionProof = ExplosionProof;
                player.Proofs.CollisionProof = CollisionProof;
                player.Proofs.MeleeProof = MeleeProof;
                player.Proofs.DrownProof = DrownProof;
                Anticheat.Main.AntiGodmode(player);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
        public static void SetPlayerTeam(this PlayerModel player, int TeamId)
        {
            try
            {
                if (player is null || !player.Exists) return;
                player.Team = TeamId;
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
        public static void SetPrivacyPolicy(this PlayerModel player, int privacy)
        {
            try
            {
                if (player is null || !player.Exists) return;
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
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
        public static void KickPlayer(this PlayerModel player, string reason)
        {
            try
            {
                player.IsKicked = true;
                player.KickedDateTime = DateTime.Now.AddSeconds(10);
                player.Emit("VnXGlobalSystemsClient:Kick", reason);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
        public static void KickGlobal(this PlayerModel player)
        {
            try
            {
                player.IsKicked = true;
                player.KickedDateTime = DateTime.Now.AddSeconds(10);
                player.Emit("VnXGlobalSystemsClient:KickGlobal");
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
        public static void UpdateDiscordInfo(this PlayerModel player, string DiscordID)
        {
            try
            {
                if (player is null || !player.Exists) return;
                if (player.DiscordID.Length > 0) return;
                player.DiscordID = DiscordID;
                player.EntityLogsCreated = false;
                Functions.CheckPlayerGlobalBans(player);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
        public static void GivePlayerWeapon(this PlayerModel player, uint WeaponHash, int ammo, bool selectWeapon)
        {
            try
            {
                if (player is null || !player.Exists) return;
                player.Weapons.Add(WeaponHash);
                player.GiveWeapon(WeaponHash, ammo, selectWeapon);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }

        public static void RemovePlayerWeapon(this PlayerModel player, uint WeaponHash)
        {
            try
            {
                if (player is null || !player.Exists) return;
                player.RemoveWeapon(WeaponHash);
                player.Weapons.Remove(WeaponHash);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }

        public static void RemoveAllPlayerWeapons(this PlayerModel player)
        {
            try
            {
                if (player is null || !player.Exists) return;
                player.RemoveAllWeapons();
                player.Weapons.Clear();
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
        public static void Position(this PlayerModel player, Vector3 position)
        {
            try
            {
                if (player is null || !player.Exists) return;
                player.LastPosition = position;
                player.Position = position;
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
        public static void SetNextTick(this PlayerModel player)
        {
            try
            {
                if (player is null || !player.Exists) return;
                player.NextTickUpdate = DateTime.Now.AddSeconds(Constants.PLAYER_TICK_INTERVAL);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }

        public static void OnVehicleDamage(this PlayerModel player, VehicleModel vehicle)
        {
            try
            {
                if (player is null || !player.Exists) return;
                WeaponSync.OnVehicleDamage(player, vehicle, player.CurrentWeapon);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
    }
}
