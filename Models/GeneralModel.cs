namespace VnXGlobalSystems.Models
{
    public class GeneralModel
    {
        public bool GlobalBanSystemActive { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public bool AnticheatSystemActive { get; set; }
        public bool VPNSystemActive { get; set; }
        public string VPNKey { get; set; }
    }
}
