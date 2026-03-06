using UnityEngine;

namespace FPSPrototype.Data
{
    [CreateAssetMenu(fileName = "LoadoutData", menuName = "FPS Prototype/Loadout Data")]
    public class LoadoutData : ScriptableObject
    {
        public string loadoutName;
        public WeaponData primaryWeapon;
        public WeaponData secondaryWeapon;
        public WeaponData specialWeapon;
    }
}
