using AltV.Net;
using AltV.Net.Elements.Entities;
using System;

namespace VnXGlobalSystems.Models
{
    public class VehicleModel : Vehicle
    {
        public uint GlobalSystemsHealth { get; set; }
        public VehicleModel(IntPtr nativePointer, ushort id) : base(nativePointer, id)
        {
        }
    }
    public class MyVehicleFactory : IEntityFactory<IVehicle>
    {
        public IVehicle Create(IntPtr playerPointer, ushort id)
        {
            return new VehicleModel(playerPointer, id);
        }
    }
}
