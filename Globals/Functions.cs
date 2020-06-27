using AltV.Net;
using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using VnXGlobalSystems.Models;

namespace VnXGlobalSystems.Globals
{
    public class Functions
    {
        public static Timer timer;
        public static AnticheatModel AnticheatModel;
        public static WeaponModel WeaponModel;
        public static GeneralModel GeneralModel;
        public static void LoadAnticheatConfig()
        {
            try
            {
                string jsonString = File.ReadAllText(Alt.Server.Resource.Path + "/settings/anticheat.json");
                AnticheatModel = JsonSerializer.Deserialize<AnticheatModel>(jsonString);
                Console.ResetColor();
                //Fly Notify
                if (AnticheatModel.AntiFly) { Core.Debug.OutputLog("-------- Global Systems AntiFly = [ON] --------", ConsoleColor.Green); }
                else { Core.Debug.OutputLog("-------- Global Systems AntiFly = [OFF] --------", ConsoleColor.Red); }
                //Ragdoll Notify
                if (AnticheatModel.AntiNoRagdoll) { Core.Debug.OutputLog("-------- Global Systems AntiNoRagdoll = [ON] --------", ConsoleColor.Green); }
                else { Core.Debug.OutputLog("-------- Global Systems AntiNoRagdoll = [OFF] --------", ConsoleColor.Red); }
                //Godmode Notify
                if (AnticheatModel.AntiGodmode) { Core.Debug.OutputLog("-------- Global Systems AntiGodmode = [ON] --------", ConsoleColor.Green); LoadWeaponDamageConfig(); }
                else { Core.Debug.OutputLog("-------- Global Systems AntiGodmode = [OFF] --------", ConsoleColor.Red); }
                //CheckTeleport Notify
                if (AnticheatModel.CheckTeleport) { Core.Debug.OutputLog("-------- Global Systems CheckTeleport = [ON] --------", ConsoleColor.Green); }
                else { Core.Debug.OutputLog("-------- Global Systems CheckTeleport = [OFF] --------", ConsoleColor.Red); }
                //CheckWeapons Notify
                if (AnticheatModel.CheckWeapons) { Core.Debug.OutputLog("-------- Global Systems CheckWeapons = [ON] --------", ConsoleColor.Green); }
                else { Core.Debug.OutputLog("-------- Global Systems CheckWeapons = [OFF] --------", ConsoleColor.Red); }
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("Anticheat-Error", ex); }
        }
        public static void OnUpdate(Object unused)
        {
            try
            {
                if (Constants.NEXT_BANLIST_REFRESH <= DateTime.Now)
                {
                    Core.Debug.OutputLog("[Global Systems] : Trying to Update Global-Banlist.", ConsoleColor.White);
                    Database.Main.RefreshBanlist();
                    Constants.NEXT_BANLIST_REFRESH = DateTime.Now.AddMinutes(Constants.BANLIST_REFRESH_RATE);
                }
                foreach (PlayerModel player in Main.ConnectedPlayers)
                {
                    if (Constants.NEXT_INGAME_BAN_CHECK <= DateTime.Now) { CheckPlayerGlobalBans(player); Constants.NEXT_INGAME_BAN_CHECK = DateTime.Now.AddMinutes(Constants.INGAME_BAN_REFRESH_RATE); }
                    if (!Functions.GeneralModel.AnticheatSystemActive) { return; }
                    Anticheat.Main.CheckTick(player);
                    Anticheat.Main.AntiFly(player);
                    Anticheat.Main.AntiNoRagdoll(player);
                    Anticheat.Main.AntiGodmode(player);
                    Anticheat.Main.CheckTeleport(player);
                    Anticheat.Main.CheckWeapons(player);
                }
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("VnX-Global-Systems:OnUpdate", ex); }
        }
        public static void LoadMainFunctions()
        {
            string jsonString = File.ReadAllText(Alt.Server.Resource.Path + "/settings/main.json");
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("-------- VenoX Global Systems is starting... --------");
            Console.WriteLine("-------- " + Constants.VNXGLOBALSYSTEMSVERSION + " --------");
            Console.WriteLine("-------- Loading Config File.... --------");
            Console.WriteLine("---------------------------------------------");
            //
            GeneralModel = JsonSerializer.Deserialize<GeneralModel>(jsonString);
            //
            Console.ResetColor();
            if (GeneralModel.VPNSystemActive) { Core.Debug.OutputLog("-------- [Settings] : VPN-Detection Active! --------", ConsoleColor.Green); }
            else { Core.Debug.OutputLog("-------- [Settings] : VPN-Detection not Active! --------", ConsoleColor.Red); }
            //
            if (GeneralModel.AnticheatSystemActive) { Core.Debug.OutputLog("-------- [Settings] : Anticheat Active! --------", ConsoleColor.Green); LoadAnticheatConfig(); }
            else { Core.Debug.OutputLog("-------- [Settings] : Anticheat Inactive! --------", ConsoleColor.Red); }
            //
            if (GeneralModel.GlobalBanSystemActive) { Core.Debug.OutputLog("-------- [Settings] : Global-Ban-System Active! --------", ConsoleColor.Green); }
            else { Core.Debug.OutputLog("-------- [Settings] : Global-Ban-System not Active! --------", ConsoleColor.Red); }
            //
            Core.Debug.OutputLog("-------- [VenoX Global Systems started] --------", ConsoleColor.DarkGreen);
            //
            Console.ForegroundColor = ConsoleColor.DarkCyan;
            Console.WriteLine("------------------------------------------------------------------------");
            Console.ResetColor();
        }
        public static void UnloadMainFunctions()
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("------------------------------------------------------------------------");
            Console.WriteLine("-------- VenoX Global Systems is stopping... --------");
            Console.WriteLine("-------- " + Constants.VNXGLOBALSYSTEMSVERSION + " --------");
            Console.WriteLine("-------- VenoX Global Systems stopped --------");
            Console.WriteLine("------------------------------------------------------------------------");
        }
        private static string Sha256(string randomString)
        {
            using SHA256 sha256Hash = SHA256.Create();
            byte[] bytes = sha256Hash.ComputeHash(Encoding.UTF8.GetBytes(randomString));
            StringBuilder stringbuilder = new StringBuilder();
            for (int i = 0; i < bytes.Length; i++)
            {
                stringbuilder.Append(bytes[i].ToString("x2"));
            }
            return stringbuilder.ToString();
        }
        public static void CheckPlayerGlobalBans(PlayerModel player)
        {
            try
            {
                foreach (GlobalBanModel BanModel in Database.Main.GlobalBannedPlayers)
                {
                    string BanFoundBy = "";
                    bool Kick = false;
                    if (Sha256(player.DiscordID) == BanModel.PlayerDiscordID) { BanFoundBy = "Discord"; Kick = true; }
                    if (Sha256(player.HardwareIdHash.ToString()) == BanModel.PlayerHardwareId) { BanFoundBy = "HWID"; Kick = true; }
                    if (Sha256(player.HardwareIdExHash.ToString()) == BanModel.PlayerHardwareIdExHash) { BanFoundBy = "HwIdExHash"; Kick = true; }
                    if (Sha256(player.Ip.ToString()) == BanModel.PlayerIPAdress) { BanFoundBy = "IP"; Kick = true; }
                    if (Sha256(player.SocialClubId.ToString()) == BanModel.PlayerSocialClubId) { BanFoundBy = "SocialClub"; Kick = true; }
                    if (Kick)
                    {
                        Core.Debug.OutputDebugString("VenoX Global Systems : " + player.Name + " could not Connect. [" + BanFoundBy + "]");
                        Core.Debug.OutputDebugString("VenoX Global Systems : " + player.Name + " Ban Reason : [" + BanModel.PlayerReason + "].");
                        player.KickGlobal();
                    }
                    if (player.EntityLogsCreated) { return; }
                    if (Constants.AWESOME_SNAKE_MODE)
                    {
                        string logname = "debug";
                        Core.Debug.WriteLogs(logname, "~~~~~~~~~~~~  [De La Info Spieler]    ~~~~~~~~~~~~~~");
                        Core.Debug.WriteLogs(logname, "Name : " + player.Name);
                        Core.Debug.WriteLogs(logname, "HWID : " + Sha256(player.HardwareIdHash.ToString()));
                        Core.Debug.WriteLogs(logname, "HWID-ExHash : " + Sha256(player.HardwareIdExHash.ToString()));
                        Core.Debug.WriteLogs(logname, "SocialID : " + Sha256(player.SocialClubId.ToString()));
                        Core.Debug.WriteLogs(logname, "DiscordID : " + Sha256(player.DiscordID));
                        Core.Debug.WriteLogs(logname, "DiscordID2 : " + player.DiscordID);
                        Core.Debug.WriteLogs(logname, "IP-Adress : " + Sha256(player.Ip.ToString()));
                        Core.Debug.WriteLogs(logname, "~~~~~~~~~~~~  [De La Info Spieler]    ~~~~~~~~~~~~~~");
                        player.EntityLogsCreated = true;
                    }
                }
            }
            catch { }
        }
        public static void LoadWeaponDamageConfig()
        {
            string jsonString = File.ReadAllText(Alt.Server.Resource.Path + "/settings/weapon.json");
            WeaponModel = JsonSerializer.Deserialize<WeaponModel>(jsonString);
            Console.ResetColor();
            Core.Debug.OutputLog("~~~~~~~~~~~~  [Weapon-Config]    ~~~~~~~~~~~~~~", ConsoleColor.Cyan);
            if (WeaponModel.Headshot) { Core.Debug.OutputLog("-------- Global Systems Headshot = [ON] --------", ConsoleColor.Green); }
            else { Core.Debug.OutputLog("-------- Global Systems Headshot = [OFF] --------", ConsoleColor.Red); }
            if (WeaponModel.TeamDamage) { Core.Debug.OutputLog("-------- Global Systems Team-Damage = [ON] --------", ConsoleColor.Green); }
            else { Core.Debug.OutputLog("-------- Global Systems Team-Damage = [OFF] --------", ConsoleColor.Red); }
            if (WeaponModel.HeadDamage) { Core.Debug.OutputLog("-------- Global Systems HeadDamage = [ON] --------", ConsoleColor.Green); }
            else { Core.Debug.OutputLog("-------- Global Systems HeadDamage = [OFF] --------", ConsoleColor.Red); }
            Constants.DamageList = new Dictionary<AltV.Net.Enums.WeaponModel, float>
            {
                { AltV.Net.Enums.WeaponModel.SniperRifle, WeaponModel.SniperRifle },
                { AltV.Net.Enums.WeaponModel.FireExtinguisher, WeaponModel.FireExtinguisher },
                { AltV.Net.Enums.WeaponModel.CompactGrenadeLauncher, WeaponModel.CompactGrenadeLauncher },
                { AltV.Net.Enums.WeaponModel.Snowballs, WeaponModel.Snowballs },
                { AltV.Net.Enums.WeaponModel.VintagePistol, WeaponModel.VintagePistol },
                { AltV.Net.Enums.WeaponModel.CombatPDW, WeaponModel.CombatPDW },
                { AltV.Net.Enums.WeaponModel.HeavySniperMkII, WeaponModel.HeavySniperMkII },
                { AltV.Net.Enums.WeaponModel.HeavySniper, WeaponModel.HeavySniper },
                { AltV.Net.Enums.WeaponModel.SweeperShotgun, WeaponModel.SweeperShotgun },
                { AltV.Net.Enums.WeaponModel.MicroSMG, WeaponModel.MicroSMG },
                { AltV.Net.Enums.WeaponModel.PipeWrench, WeaponModel.PipeWrench },
                { AltV.Net.Enums.WeaponModel.Pistol, WeaponModel.Pistol },
                { AltV.Net.Enums.WeaponModel.PumpShotgun, WeaponModel.PumpShotgun },
                { AltV.Net.Enums.WeaponModel.APPistol, WeaponModel.APPistol },
                { AltV.Net.Enums.WeaponModel.Baseball, WeaponModel.Baseball },
                { AltV.Net.Enums.WeaponModel.MolotovCocktail, WeaponModel.MolotovCocktail },
                { AltV.Net.Enums.WeaponModel.SMG, WeaponModel.SMG },
                { AltV.Net.Enums.WeaponModel.StickyBomb, WeaponModel.StickyBomb },
                { AltV.Net.Enums.WeaponModel.JerryCan, WeaponModel.JerryCan },
                { AltV.Net.Enums.WeaponModel.StunGun, WeaponModel.StunGun },
                { AltV.Net.Enums.WeaponModel.StoneHatchet, WeaponModel.StoneHatchet },
                { AltV.Net.Enums.WeaponModel.AussaultRifleMkII, WeaponModel.AussaultRifleMkII },
                { AltV.Net.Enums.WeaponModel.HeavyShotgun, WeaponModel.HeavyShotgun },
                { AltV.Net.Enums.WeaponModel.Minigun, WeaponModel.Minigun },
                { AltV.Net.Enums.WeaponModel.GolfClub, WeaponModel.GolfClub },
                { AltV.Net.Enums.WeaponModel.UnholyHellbringer, WeaponModel.UnholyHellbringer },
                { AltV.Net.Enums.WeaponModel.FlareGun, WeaponModel.FlareGun },
                { AltV.Net.Enums.WeaponModel.Flare, WeaponModel.Flare },
                { AltV.Net.Enums.WeaponModel.GrenadeLauncherSmoke, WeaponModel.GrenadeLauncherSmoke },
                { AltV.Net.Enums.WeaponModel.Hammer, WeaponModel.Hammer },
                { AltV.Net.Enums.WeaponModel.PumpShotgunMkII, WeaponModel.PumpShotgunMkII },
                { AltV.Net.Enums.WeaponModel.CombatPistol, WeaponModel.CombatPistol },
                { AltV.Net.Enums.WeaponModel.GusenbergSweeper, WeaponModel.GusenbergSweeper },
                { AltV.Net.Enums.WeaponModel.CompactRifle, WeaponModel.CompactRifle },
                { AltV.Net.Enums.WeaponModel.HomingLauncher, WeaponModel.HomingLauncher },
                { AltV.Net.Enums.WeaponModel.Nightstick, WeaponModel.Nightstick },
                { AltV.Net.Enums.WeaponModel.MarksmanRifleMkII, WeaponModel.MarksmanRifleMkII },
                { AltV.Net.Enums.WeaponModel.Railgun, WeaponModel.Railgun },
                { AltV.Net.Enums.WeaponModel.SawedOffShotgun, WeaponModel.SawedOffShotgun },
                { AltV.Net.Enums.WeaponModel.SMGMkII, WeaponModel.SMGMkII },
                { AltV.Net.Enums.WeaponModel.BullpupRifle, WeaponModel.BullpupRifle },
                { AltV.Net.Enums.WeaponModel.FireworkLauncher, WeaponModel.FireworkLauncher },
                { AltV.Net.Enums.WeaponModel.CombatMG, WeaponModel.CombatMG },
                { AltV.Net.Enums.WeaponModel.CarbineRifle, WeaponModel.CarbineRifle },
                { AltV.Net.Enums.WeaponModel.Crowbar, WeaponModel.Crowbar },
                { AltV.Net.Enums.WeaponModel.BullpupRifleMkII, WeaponModel.BullpupRifleMkII },
                { AltV.Net.Enums.WeaponModel.SNSPistolMkII, WeaponModel.SNSPistolMkII },
                { AltV.Net.Enums.WeaponModel.Flashlight, WeaponModel.Flashlight },
                { AltV.Net.Enums.WeaponModel.AntiqueCavalryDagger, WeaponModel.AntiqueCavalryDagger },
                { AltV.Net.Enums.WeaponModel.Grenade, WeaponModel.Grenade },
                { AltV.Net.Enums.WeaponModel.PoolCue, WeaponModel.PoolCue },
                { AltV.Net.Enums.WeaponModel.BaseballBat, WeaponModel.BaseballBat },
                { AltV.Net.Enums.WeaponModel.SpecialCarbineMkII, WeaponModel.SpecialCarbineMkII },
                { AltV.Net.Enums.WeaponModel.DoubleActionRevolver, WeaponModel.DoubleActionRevolver },
                { AltV.Net.Enums.WeaponModel.Pistol50, WeaponModel.Pistol50 },
                { AltV.Net.Enums.WeaponModel.Knife, WeaponModel.Knife },
                { AltV.Net.Enums.WeaponModel.MG, WeaponModel.MG },
                { AltV.Net.Enums.WeaponModel.BullpupShotgun, WeaponModel.BullpupShotgun },
                { AltV.Net.Enums.WeaponModel.BZGas, WeaponModel.BZGas },
                { AltV.Net.Enums.WeaponModel.Fist, WeaponModel.Fist },
                { AltV.Net.Enums.WeaponModel.GrenadeLauncher, WeaponModel.GrenadeLauncher },
                { AltV.Net.Enums.WeaponModel.Musket, WeaponModel.Musket },
                { AltV.Net.Enums.WeaponModel.ProximityMines, WeaponModel.ProximityMines },
                { AltV.Net.Enums.WeaponModel.AdvancedRifle, WeaponModel.AdvancedRifle },
                { AltV.Net.Enums.WeaponModel.UpnAtomizer, WeaponModel.UpnAtomizer },
                { AltV.Net.Enums.WeaponModel.RPG, WeaponModel.RPG },
                { AltV.Net.Enums.WeaponModel.Widowmaker, WeaponModel.Widowmaker },
                { AltV.Net.Enums.WeaponModel.PipeBombs, WeaponModel.PipeBombs },
                { AltV.Net.Enums.WeaponModel.MiniSMG, WeaponModel.MiniSMG },
                { AltV.Net.Enums.WeaponModel.SNSPistol, WeaponModel.SNSPistol },
                { AltV.Net.Enums.WeaponModel.PistolMkII, WeaponModel.PistolMkII },
                { AltV.Net.Enums.WeaponModel.AssaultRifle, WeaponModel.AssaultRifle },
                { AltV.Net.Enums.WeaponModel.SpecialCarbine, WeaponModel.SpecialCarbine },
                { AltV.Net.Enums.WeaponModel.HeavyRevolver, WeaponModel.HeavyRevolver },
                { AltV.Net.Enums.WeaponModel.MarksmanRifle, WeaponModel.MarksmanRifle },
                { AltV.Net.Enums.WeaponModel.HeavyRevolverMkII, WeaponModel.HeavyRevolverMkII },
                { AltV.Net.Enums.WeaponModel.BattleAxe, WeaponModel.BattleAxe },
                { AltV.Net.Enums.WeaponModel.HeavyPistol, WeaponModel.HeavyPistol },
                { AltV.Net.Enums.WeaponModel.BrassKnuckles, WeaponModel.BrassKnuckles },
                { AltV.Net.Enums.WeaponModel.MachinePistol, WeaponModel.MachinePistol },
                { AltV.Net.Enums.WeaponModel.CombatMGMkII, WeaponModel.CombatMGMkII },
                { AltV.Net.Enums.WeaponModel.MarksmanPistol, WeaponModel.MarksmanPistol },
                { AltV.Net.Enums.WeaponModel.Machete, WeaponModel.Machete },
                { AltV.Net.Enums.WeaponModel.Switchblade, WeaponModel.Switchblade },
                { AltV.Net.Enums.WeaponModel.AssaultShotgun, WeaponModel.AssaultShotgun },
                { AltV.Net.Enums.WeaponModel.DoubleBarrelShotgun, WeaponModel.DoubleBarrelShotgun },
                { AltV.Net.Enums.WeaponModel.AssaultSMG, WeaponModel.AssaultSMG },
                { AltV.Net.Enums.WeaponModel.Hatchet, WeaponModel.Hatchet },
                { AltV.Net.Enums.WeaponModel.BrokenBottle, WeaponModel.BrokenBottle },
                { AltV.Net.Enums.WeaponModel.CarbineRifleMkII, WeaponModel.CarbineRifleMkII },
                { AltV.Net.Enums.WeaponModel.TearGas, WeaponModel.TearGas },
            };
            if (!Constants.AWESOME_SNAKE_MODE) { return; }
            int c = 0;
            foreach (var weaponmodel in Constants.DamageList)
            {
                Core.Debug.OutputDebugString("[" + ++c + "]--- WeaponModel loaded " + weaponmodel.Key + " | " + weaponmodel.Value + " ---");
            }
        }
    }
}
