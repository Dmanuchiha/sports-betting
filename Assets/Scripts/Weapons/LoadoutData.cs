using UnityEngine;

namespace FPSPrototype.Weapons
{
    [CreateAssetMenu(fileName = "LoadoutData", menuName = "FPS/Loadout Data")]
    public class LoadoutData : ScriptableObject
    {
        public string loadoutName;
        public WeaponData primaryWeapon;
        public WeaponData secondaryWeapon;
        public WeaponData heavyWeapon;
    }
}
