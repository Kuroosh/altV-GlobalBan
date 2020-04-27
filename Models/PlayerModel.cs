using AltV.Net;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace VnXGlobalSystems.Models
{
    public class PlayerModel : Player
    {
        public Vector3 LastPosition { get; set; }
        public bool EntityIsFlying { get; set; }
        public DateTime NextFlyUpdate { get; set; }
        public int FlyTicks { get; set; }
        public List<AltV.Net.Enums.WeaponModel> Weapons { get; set; }
        public string DiscordID { get; set; }

        public PlayerModel(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {
            try
            {
                LastPosition = new Vector3();
                EntityIsFlying = false;
                NextFlyUpdate = DateTime.Now;
                FlyTicks = 0;
                Weapons = new List<AltV.Net.Enums.WeaponModel>();
                DiscordID = "";
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("PlayerModel-Create", ex); }
        }
    }
    public class MyPlayerFactory : IEntityFactory<IPlayer>
    {
        public IPlayer Create(IntPtr playerPointer, ushort id)
        {
            try
            {
                return new PlayerModel(playerPointer, id);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("PlayerFactory:Create", ex); return null; }
        }
    }
}
