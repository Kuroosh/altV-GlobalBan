using AltV.Net;
using AltV.Net.Async;
using AltV.Net.Elements.Entities;
using System.Collections.Generic;
using VenoX_Global_Systems._Models_;

namespace VenoX_Global_Systems
{
    internal class VenoXResource : AsyncResource
    {
        public override IEntityFactory<IPlayer> GetPlayerFactory()
        {
            return new MyPlayerFactory();
        }
        public override void OnStart()
        {
            _Globals_.Events.OnResourceStart();
        }
        public override void OnStop()
        {
            _Globals_.Events.OnResourceStop();
        }
    }
    public class Main
    {
        public static List<PlayerModel> ConnectedPlayers = new List<PlayerModel>();
    }
}
