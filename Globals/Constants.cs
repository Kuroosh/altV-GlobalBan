using System;
using System.Collections.Generic;
using VnXGlobalSystems.Models;

namespace VnXGlobalSystems.Globals
{
    public class Constants
    {
        public const string VNXGLOBALSYSTEMSVERSION = "V.1.1.4";             //VenoX Global Systems Version.
        public const int UPDATEINTERVAL = 100;                               //Time in MS.
        public const int START_AFTER_CONNECT = 3;                            //Time in Seconds.
        public const int PLAYER_TICK_INTERVAL = 5;                           //Time in Seconds to check if Clientside Connection exists.

        public static bool AWESOME_SNAKE_MODE = true;                                                    // Beware of using this!! you could not be Awesome enough for that...
        public static int BANLIST_REFRESH_RATE = 30;                                                     //Time in Minutes
        public static int INGAME_BAN_REFRESH_RATE = 1;                                                   //Time in Minutes
        public static DateTime NEXT_BANLIST_REFRESH = DateTime.Now.AddMinutes(BANLIST_REFRESH_RATE);    //Last Time Refreshed/Renewed the Banlist.
        public static DateTime NEXT_INGAME_BAN_CHECK = DateTime.Now.AddMinutes(INGAME_BAN_REFRESH_RATE);    //Last Time Refreshed/Renewed the Banlist.

        // Cheat Check
        public const int TELEPORT_KICK_FOOT = 12;                            //Distance in Meter ( Kick after ...meter+ on Foot).
        public const int TELEPORT_KICK_VEHICLE = 40;                         //Distance in Meter ( Kick after ...meter+ in Vehicle).
        public const int TELEPORT_KICK_FLYVEHICLE = 17;                    //Distance in Meter ( Kick after ...meter+ in Vehicle).


        public static List<PrivacyModel> PrivacyAcceptedPlayers = new List<PrivacyModel>();          //Load current players who accept the privacy.


        public static Dictionary<AltV.Net.Enums.WeaponModel, float> DamageList;


        public static List<AltV.Net.Enums.VehicleModel> Helicopter = new List<AltV.Net.Enums.VehicleModel>
        {
            AltV.Net.Enums.VehicleModel.Akula,
            AltV.Net.Enums.VehicleModel.Annihilator,
            AltV.Net.Enums.VehicleModel.Buzzard,
            AltV.Net.Enums.VehicleModel.Buzzard2,
            AltV.Net.Enums.VehicleModel.Cargobob,
            AltV.Net.Enums.VehicleModel.Cargobob2,
            AltV.Net.Enums.VehicleModel.Cargobob3,
            AltV.Net.Enums.VehicleModel.Cargobob4,
            AltV.Net.Enums.VehicleModel.Frogger,
            AltV.Net.Enums.VehicleModel.Frogger2,
            AltV.Net.Enums.VehicleModel.Havok,
            AltV.Net.Enums.VehicleModel.Hunter,
            AltV.Net.Enums.VehicleModel.Maverick,
            AltV.Net.Enums.VehicleModel.Savage,
            AltV.Net.Enums.VehicleModel.Seasparrow,
            AltV.Net.Enums.VehicleModel.Skylift,
            AltV.Net.Enums.VehicleModel.Supervolito,
            AltV.Net.Enums.VehicleModel.Supervolito2,
            AltV.Net.Enums.VehicleModel.Swift,
            AltV.Net.Enums.VehicleModel.Swift2,
            AltV.Net.Enums.VehicleModel.Valkyrie,
            AltV.Net.Enums.VehicleModel.Valkyrie2,
            AltV.Net.Enums.VehicleModel.Volatus
        };
        public static List<AltV.Net.Enums.VehicleModel> Planes = new List<AltV.Net.Enums.VehicleModel>
        {
            AltV.Net.Enums.VehicleModel.AlphaZ1,
            AltV.Net.Enums.VehicleModel.Avenger,
            AltV.Net.Enums.VehicleModel.Avenger2,
            AltV.Net.Enums.VehicleModel.Besra,
            AltV.Net.Enums.VehicleModel.Blimp,
            AltV.Net.Enums.VehicleModel.Blimp2,
            AltV.Net.Enums.VehicleModel.Blimp3,
            AltV.Net.Enums.VehicleModel.Bombushka,
            AltV.Net.Enums.VehicleModel.CargoPlane,
            AltV.Net.Enums.VehicleModel.Cuban800,
            AltV.Net.Enums.VehicleModel.Dodo,
            AltV.Net.Enums.VehicleModel.Duster,
            AltV.Net.Enums.VehicleModel.Howard,
            AltV.Net.Enums.VehicleModel.Hydra,
            AltV.Net.Enums.VehicleModel.Jet,
            AltV.Net.Enums.VehicleModel.Lazer,
            AltV.Net.Enums.VehicleModel.Luxor,
            AltV.Net.Enums.VehicleModel.Luxor2,
            AltV.Net.Enums.VehicleModel.Mammatus,
            AltV.Net.Enums.VehicleModel.Microlight,
            AltV.Net.Enums.VehicleModel.Miljet,
            AltV.Net.Enums.VehicleModel.Mogul,
            AltV.Net.Enums.VehicleModel.Molotok,
            AltV.Net.Enums.VehicleModel.Nimbus,
            AltV.Net.Enums.VehicleModel.Nokota,
            AltV.Net.Enums.VehicleModel.Pyro,
            AltV.Net.Enums.VehicleModel.Rogue,
            AltV.Net.Enums.VehicleModel.Seabreeze,
            AltV.Net.Enums.VehicleModel.Shamal,
            AltV.Net.Enums.VehicleModel.Starling,
            AltV.Net.Enums.VehicleModel.Strikeforce,
            AltV.Net.Enums.VehicleModel.Stunt,
            AltV.Net.Enums.VehicleModel.Titan,
            AltV.Net.Enums.VehicleModel.Tula,
            AltV.Net.Enums.VehicleModel.Velum,
            AltV.Net.Enums.VehicleModel.Velum2,
            AltV.Net.Enums.VehicleModel.Vestra,
            AltV.Net.Enums.VehicleModel.Volatol,
        };
    }
}
