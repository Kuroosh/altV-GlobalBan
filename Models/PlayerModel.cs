using AltV.Net;
using AltV.Net.Elements.Entities;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace VnXGlobalSystems.Models
{
    public class Proofs
    {
        //bulletProof: boolean, fireProof: boolean, explosionProof: boolean, collisionProof: boolean, meleeProof: boolean, p6: boolean, p7: boolean, drownProof: boolean
        public bool BulletProof { get; set; }
        public bool FireProof { get; set; }
        public bool ExplosionProof { get; set; }
        public bool CollisionProof { get; set; }
        public bool MeleeProof { get; set; }
        public bool DrownProof { get; set; }
        public Proofs(Player player)
        {
            try
            {

            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
    }
    public class PlayerModel : Player
    {
        public Proofs Proofs { get; }
        public Vector3 LastPosition { get; set; }
        public bool EntityIsFlying { get; set; }
        public bool EntityLogsCreated { get; set; }
        public DateTime NextTickUpdate { get; set; }
        public DateTime NextFlyUpdate { get; set; }
        public int FlyTicks { get; set; }
        public List<uint> Weapons { get; set; }
        public DateTime NextWeaponTickCheck { get; set; }
        public int WeaponTickCheck { get; set; }
        public uint LastWeapon { get; set; }
        public int Team { get; set; }
        public string DiscordID { get; set; }
        public PlayerModel(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {
            try
            {
                Proofs = new Proofs(this);
                LastPosition = new Vector3();
                EntityIsFlying = false;
                NextFlyUpdate = DateTime.Now;
                NextTickUpdate = DateTime.Now.AddSeconds(15);
                EntityLogsCreated = false;
                FlyTicks = 0;
                Weapons = new List<uint>();
                LastWeapon = CurrentWeapon;
                WeaponTickCheck = 0;
                Team = 0;
                DiscordID = "";
                Proofs.BulletProof = true;
                Proofs.FireProof = false;
                Proofs.ExplosionProof = false;
                Proofs.CollisionProof = false;
                Proofs.MeleeProof = true;
                Proofs.DrownProof = false;
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); }
        }
    }
    public class MyPlayerFactory : IEntityFactory<IPlayer>
    {
        public IPlayer Create(IntPtr playerPointer, ushort id)
        {
            try { return new PlayerModel(playerPointer, id); }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); return null; }
        }
    }
}
