using AltV.Net;
using System;
using VnXGlobalSystems.Models;

namespace VnXGlobalSystems.Globals
{
    public class ClientEvents : IScript
    {
        /* Client Events */

        [ClientEvent("VnXGlobalSystems:SetDiscordID")]
        public static void UpdateDiscordInfo(PlayerModel player, string DiscordID) => player.UpdateDiscordInfo(DiscordID);

        [ClientEvent("VnXGlobalSystems:KickPlayer")]
        public static void KickPlayer(PlayerModel player, string Base64, string Reason = "")
        {
            try
            {
                Functions.SaveScreenBase64(player, Base64);
                player.Kick(Reason);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }

        [ClientEvent("VnXGlobalSystems:PrivacyPolicy")]
        public static void SetPrivacyPolicy(PlayerModel player, int privacy) => player.SetPrivacyPolicy(privacy);

        [ClientEvent("VnXGlobalSystems:OnTickCall")]
        public static void OnPlayerTickCall(PlayerModel player) => player.SetNextTick();

        [ClientEvent("VnXGlobalSystems:OnVehicleWeaponHit")]
        public static void OnPlayerVehicleWeaponHit(PlayerModel player, VehicleModel vehicle) => player.OnVehicleDamage(vehicle);
    }
}
