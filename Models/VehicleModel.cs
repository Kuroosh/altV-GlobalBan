using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using System;

namespace VnXGlobalSystems.Models
{
    public class VehicleModel : Vehicle
    {
        private uint _GlobalSystemsHealth { get; set; }
        public uint GlobalSystemsHealth { get { return _GlobalSystemsHealth; } set { BodyHealth = _GlobalSystemsHealth; _GlobalSystemsHealth = value; } }

        public VehicleModel(uint model, Position position, Rotation rotation) : base(model, position, rotation)
        {

        }
        public VehicleModel(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {
            GlobalSystemsHealth = BodyHealth;
        }

    }

    public class MyVehicleFactory : IEntityFactory<IVehicle>
    {
        public IVehicle Create(IntPtr playerPointer, ushort id)
        {
            try
            {
                return new VehicleModel(playerPointer, id);
            }
            catch (Exception ex) { Core.Debug.CatchExceptions(ex); return null; }
        }
    }
}