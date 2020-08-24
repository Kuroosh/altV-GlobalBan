using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using System.Collections.Generic;
using VnXGlobalSystems.Models;

namespace VnXGlobalSystems
{
    internal class VenoXResource : AsyncResource
    {
        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new MyPlayerFactory();
        }
        public override IEntityFactory<IVehicle> GetVehicleFactory()
        {
            return new MyVehicleFactory();
        }
        public override void OnStart()
        {
            Globals.Events.OnResourceStart();
        }
        public override void OnStop()
        {
            Globals.Events.OnResourceStop();
        }
    }
    public class Main
    {
        public static List<PlayerModel> ConnectedPlayers = new List<PlayerModel>();
    }
}
