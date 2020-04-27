using MySql.Data.MySqlClient;

namespace VenoX_Global_Systems._Models_
{
    public class GlobalBanModel
    {
        public string Player_UID { get; set; }
        public string Player_Name { get; set; }
        public string Player_HardwareId { get; set; }
        public string Player_HardwareIdExHash { get; set; }
        public string Player_SocialClubId { get; set; }
        public string Player_DiscordID { get; set; }

        public GlobalBanModel(MySqlDataReader reader)
        {
            Player_UID = reader.GetString("UID");
            Player_Name = reader.GetString("Name");
            Player_HardwareId = reader.GetString("HardwareIdHash");
            Player_HardwareIdExHash = reader.GetString("HardwareIdExHash");
            Player_SocialClubId = reader.GetString("SocialClubId");
            Player_DiscordID = reader.GetString("DiscordID");
        }
    }
}
