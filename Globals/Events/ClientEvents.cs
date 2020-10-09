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
        public static void KickPlayer(PlayerModel player, string[] Base64Objs)
        {
            string Base64 = String.Join(String.Empty, Base64Objs);
            player.Screenshot = Base64;
            Core.Debug.OutputDebugString(player.Name + " Got Kicked");
            Functions.SaveScreenBase64(player, player.Screenshot);
            player.Kick(reason: String.Empty);
            Core.Debug.OutputDebugString(player.Screenshot);
            player.Screenshot = "";
        }

        [ClientEvent("VnXGlobalSystems:PrivacyPolicy")]
        public static void SetPrivacyPolicy(PlayerModel player, int privacy) => player.SetPrivacyPolicy(privacy);

        [ClientEvent("VnXGlobalSystems:OnTickCall")]
        public static void OnPlayerTickCall(PlayerModel player) => player.SetNextTick();

        [ClientEvent("VnXGlobalSystems:OnVehicleWeaponHit")]
        public static void OnPlayerVehicleWeaponHit(PlayerModel player, VehicleModel vehicle) => player.OnVehicleDamage(vehicle);
    }
}
