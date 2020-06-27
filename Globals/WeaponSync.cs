using AltV.Net;
using AltV.Net.Data;
using System;
using VnXGlobalSystems.Models;

namespace VnXGlobalSystems.Globals
{
    public class WeaponSync
    {
        public static float GetBoneDamageMul(BodyPart hitbone)
        {
            try
            {
                float _num = 1;
                if (Functions.WeaponModel.HeadDamage) { if (hitbone == BodyPart.Head) { _num = Functions.WeaponModel.HeadDamageMultiplier; } }
                return _num;
            }
            catch { return 1; }
        }
        public static float GetWeaponDamage(AltV.Net.Enums.WeaponModel Weapon)
        {
            try { return Constants.DamageList[Weapon]; }
            catch { Core.Debug.OutputLog("ERROR: COULD NOT FIND WEAPON : " + Weapon + " IN DamageList! [PLEASE CONTACT SOLID_SNAKE - https://forum.altv.mp/profile/1466-solid_snake/!]", ConsoleColor.Red); return 1; }
        }
        public static void WeaponDamage(PlayerModel player, PlayerModel target, uint weapon, ushort dmg, Position offset, BodyPart bodypart)
        {
            try
            {
                if (target == player) { return; }
                if (!Functions.WeaponModel.TeamDamage && target.Team == player.Team) { return; }
                if (target.Health <= 0 || target.IsDead || player.Health <= 0 || player.IsDead) { return; }
                if (Functions.AnticheatModel.CheckWeapons)
                {
                    if (!player.Weapons.Contains(weapon) && weapon != (uint)AltV.Net.Enums.WeaponModel.Fist && weapon != 0)
                    {
                        Console.ForegroundColor = ConsoleColor.Yellow;
                        Core.Debug.OutputDebugString("[INFO] : " + player.Name + " got kicked! Reason : Weapon-Anticheat!");
                        Console.ResetColor();
                        string reason = "[VenoX Global Systems " + Constants.VNXGLOBALSYSTEMSVERSION + "] : Kicked by Anticheat";
                        player.KickPlayer(reason);
                    }
                }
                if (Functions.WeaponModel.Headshot && bodypart == BodyPart.Head)
                {
                    target.Health = 0;
                    Alt.Emit("GlobalSystems:OnPlayerSyncDamage", target, player);
                    return;
                }
                AltV.Net.Enums.WeaponModel ConvertedWeapon = (AltV.Net.Enums.WeaponModel)weapon;
                if (ConvertedWeapon == AltV.Net.Enums.WeaponModel.SniperRifle && bodypart == BodyPart.Head)
                {
                    if (Functions.WeaponModel.SniperHeadshotOneshot)
                    {
                        target.Health = 0;
                        Alt.Emit("GlobalSystems:OnPlayerSyncDamage", target, player);
                        return;
                    }
                }
                if (ConvertedWeapon == AltV.Net.Enums.WeaponModel.RPG)
                {
                    target.Health -= 150;
                    return;
                }

                float Damage = GetWeaponDamage(ConvertedWeapon); // GetWeaponDamage
                Damage *= GetBoneDamageMul(bodypart); //Damage * BoneMule
                Alt.Emit("GlobalSystems:OnPlayerSyncDamage", target, player);
                if (target.Armor > 0)
                {
                    int Adiff = target.Armor - Convert.ToInt32(Damage);
                    if (Adiff <= 0) { target.Health += (ushort)Adiff; target.Armor = 0; }
                    else { target.Armor -= (ushort)Damage; }
                }
                else
                {
                    target.Health -= (ushort)Damage;
                }
            }
            catch (Exception ex) { Core.Debug.CatchExceptions("WeaponDamage", ex); }
        }
    }
}
