﻿using AltV.Net;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
                Core.Debug.WriteLogs("debug", "Server Started with Debug Mode true!");
        }
        public static void OnResourceStop()
        {
            Functions.UnloadMainFunctions();
        }

        public static void LoadPrivacyAccounts()
        {
            List<PrivacyModel> privacyClasses = JsonConvert.DeserializeObject<List<PrivacyModel>>(File.ReadAllText(Alt.Server.Resource.Path + "/settings/privacy.json"));
            if (privacyClasses is null) return;
            if (privacyClasses.Count <= 0) return;
            foreach (PrivacyModel privacyUser in privacyClasses)
                Constants.PrivacyAcceptedPlayers.Add(privacyUser);
        }
        public static bool PlayerAcceptedPrivacyPolicy(PlayerModel player)
        {
            try
            {
                return Constants.PrivacyAcceptedPlayers.Any(privacyClass => privacyClass.HardwareId == player.HardwareIdHash.ToString() || privacyClass.HardwareIdExHash == player.HardwareIdExHash.ToString() || privacyClass.IP == player.Ip.ToString() || privacyClass.SocialID == player.SocialClubId.ToString());
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); return false; }
        }
    }
}
