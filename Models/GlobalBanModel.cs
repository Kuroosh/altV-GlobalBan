using MySql.Data.MySqlClient;

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

        public GlobalBanModel(MySqlDataReader reader)
        {
            PlayerUID = reader.GetString("UID");
            PlayerName = reader.GetString("Name");
            PlayerHardwareId = reader.GetString("HardwareIdHash");
            PlayerHardwareIdExHash = reader.GetString("HardwareIdExHash");
            PlayerSocialClubId = reader.GetString("SocialClubId");
            PlayerDiscordID = reader.GetString("DiscordID");
        }
    }
}
