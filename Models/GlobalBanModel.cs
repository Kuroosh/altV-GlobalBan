using MySql.Data.MySqlClient;
using System;

namespace VnXGlobalSystems.Models
{
    public class GlobalBanModel
    {
        public string PlayerUID { get; set; }
        public string PlayerName { get; set; }
        public string PlayerHardwareId { get; set; }
        public string PlayerHardwareIdExHash { get; set; }
        public string PlayerSocialClubId { get; set; }
        public string PlayerDiscordID { get; set; }
        public string PlayerIPAdress { get; set; }
        public string PlayerReason { get; set; }
        public string PlayerServerOwner { get; set; }
        public string PlayerServer { get; set; }
        public DateTime PlayerCreated { get; set; }

        public GlobalBanModel(MySqlDataReader reader)
        {
            PlayerUID = reader.GetString("UID");
            PlayerName = reader.GetString("Name");
            PlayerHardwareId = reader.GetString("HardwareIdHash");
            PlayerHardwareIdExHash = reader.GetString("HardwareIdExHash");
            PlayerSocialClubId = reader.GetString("SocialClubId");
            PlayerDiscordID = reader.GetString("DiscordID");
            PlayerIPAdress = reader.GetString("IP-Adress");
            PlayerReason = reader.GetString("Reason");
            PlayerServerOwner = reader.GetString("ServerOwner");
            PlayerServer = reader.GetString("Server");
            PlayerCreated = reader.GetDateTime("Created");
        }
    }
}
