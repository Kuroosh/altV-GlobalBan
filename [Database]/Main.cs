using AltV.Net.Async;
using MySql.Data.MySqlClient;
using System.Collections.Generic;
using System.Threading.Tasks;
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
            string user = "VenoXVGlobalSystems";
            string pass = "n9Cc~5j5q04f6K%d75Leak9";
            string db = "VenoXVGlobalSystems";
            connectionString = "SERVER=" + host + "; DATABASE=" + db + "; UID=" + user + "; PASSWORD=" + pass + "; SSLMODE=none;";

            await Task.Run(async () =>
            {
                await AltAsync.Do(() =>
                {
                    GlobalBannedPlayers = LoadBanlist();
                });
            });
        }

        private static List<GlobalBanModel> LoadBanlist()
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
    }
}