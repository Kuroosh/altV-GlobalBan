using AltV.Net.Async;
using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using VnXGlobalSystems.Globals;
using VnXGlobalSystems.Models;

namespace VnXGlobalSystems.Database
{
    public class Main
    {
        public static string connectionString;

        public static List<GlobalBanModel> GlobalBannedPlayers;
        public static async void OnResourceStart()
        {
            string host = "5.180.66.146";
            string user = "VenoXV_Global_Systems";
            string pass = "ri@75Cl03?Y9ul6dp26l_Ud8";
            string db = "VenoXV_Global_Systems";
            connectionString = "SERVER=" + host + "; DATABASE=" + db + "; UID=" + user + "; PASSWORD=" + pass + "; SSLMODE=none;";

            await Task.Run(async () =>
            {
                await AltAsync.Do(() =>
                {
                    GlobalBannedPlayers = LoadBanlist();
                    if (!Constants.AWESOME_SNAKE_MODE) { return; }
                    foreach (GlobalBanModel model in GlobalBannedPlayers)
                    {
                        Core.Debug.OutputLog("~~~~~~~~~~~~  [Banned]    ~~~~~~~~~~~~~~", ConsoleColor.Red);
                        Core.Debug.OutputLog("Name : " + model.PlayerName, ConsoleColor.White);
                        Core.Debug.OutputLog("HWID : " + model.PlayerHardwareId, ConsoleColor.White);
                        Core.Debug.OutputLog("HWID-ExHash : " + model.PlayerHardwareIdExHash, ConsoleColor.White);
                        Core.Debug.OutputLog("SocialID : " + model.PlayerSocialClubId, ConsoleColor.White);
                        Core.Debug.OutputLog("DiscordID : " + model.PlayerDiscordID, ConsoleColor.White);
                        Core.Debug.OutputLog("IP-Adress : " + model.PlayerIPAdress, ConsoleColor.White);
                        Core.Debug.OutputLog("Reason : " + model.PlayerReason, ConsoleColor.White);
                        Core.Debug.OutputLog("ServerOwner : " + model.PlayerServerOwner, ConsoleColor.White);
                        Core.Debug.OutputLog("Server : " + model.PlayerServer, ConsoleColor.White);
                        Core.Debug.OutputLog("Created : " + model.PlayerCreated, ConsoleColor.White);
                        Core.Debug.OutputLog("~~~~~~~~~~~~  [Banned]    ~~~~~~~~~~~~~~", ConsoleColor.Red);
                    }
                });
            });
        }
        /*
        public static void RegisterTestInsert(string Name, string HardwareIdHash, string HardwareIdExHash, string SocialClubId, string DiscordID)
        {
            using MySqlConnection connection = new MySqlConnection(connectionString);
            connection.Open();
            MySqlCommand command = connection.CreateCommand();
            command.CommandText = "INSERT INTO spieler (Name, SpielerSocial, HardwareIdHash, HardwareIdExHash, SocialClubId ) VALUES(@Name, @HardwareIdHash, @HardwareIdExHash, @SocialClubId)";
            command.Parameters.AddWithValue("@Name", Name);
            command.Parameters.AddWithValue("@HardwareIdHash", HardwareIdHash);
            command.Parameters.AddWithValue("@HardwareIdExHash", HardwareIdExHash);
            command.Parameters.AddWithValue("@SocialClubId", SocialClubId);
            command.ExecuteNonQuery();
        }*/

        public static void RefreshBanlist()
        {
            try
            {
                GlobalBannedPlayers = LoadBanlist();
                Core.Debug.OutputLog("[Global Systems] : Updated Global-Banlist.", ConsoleColor.White);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("Global Systems : RefreshBanlist", ex); }
        }

        private static List<GlobalBanModel> LoadBanlist()
        {
            try
            {
                List<GlobalBanModel> Banlist = new List<GlobalBanModel>();
                using MySqlConnection connection = new MySqlConnection(connectionString);
                connection.Open();
                MySqlCommand command = connection.CreateCommand();
                command.CommandText = "SELECT * FROM player";

                using MySqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    GlobalBanModel ban = new GlobalBanModel(reader);
                    Banlist.Add(ban);
                }
                return Banlist;
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("LoadBanlist", ex); return new List<GlobalBanModel>(); }
        }
    }
}