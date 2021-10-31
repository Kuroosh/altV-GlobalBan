using AltV.Net;
using AltV.Net.Data;
using AltV.Net.Elements.Entities;
using System;

namespace VnXGlobalSystems.Models
{
    public class VehicleModel : Vehicle
    {
        private uint _GlobalSystemsHealth { get; set; }
        public uint GlobalSystemsHealth 
        { 
            get => _GlobalSystemsHealth;
            set { 
                BodyHealth = _GlobalSystemsHealth; 
                _GlobalSystemsHealth = value; 
            } 
        }

        public VehicleModel(IServer server, uint model, Position position, Rotation rotation) : base(server, model, position, rotation){ }

        public VehicleModel(IServer server, IntPtr nativePointer, ushort id) : base(server, nativePointer, id)
        {
            GlobalSystemsHealth = BodyHealth;
        }
    }

    public class MyVehicleFactory : IEntityFactory<IVehicle>
    {
        public IVehicle Create(IServer server, IntPtr entityPointer, ushort id) => new VehicleModel(server, entityPointer, id) ?? null;
    }
}