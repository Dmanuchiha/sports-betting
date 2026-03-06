using UnityEngine;
using FPSPrototype.Data;

namespace FPSPrototype.Weapons
{
    [System.Serializable]
    public class WeaponInstance
    {
        public WeaponData data;
        public int currentMagazine;
        public int reserveAmmo;

        public WeaponInstance(WeaponData weaponData)
        {
            data = weaponData;
            currentMagazine = weaponData != null ? weaponData.magazineSize : 0;
            reserveAmmo = weaponData != null ? weaponData.reserveAmmo : 0;
        }

        public bool CanFire() => data != null && currentMagazine > 0;

        public bool CanReload() => data != null && currentMagazine < data.magazineSize && reserveAmmo > 0;

        public void ConsumeAmmo(int amount = 1)
        {
            currentMagazine = Mathf.Max(0, currentMagazine - amount);
        }

        public void Reload()
        {
            if (!CanReload())
            {
                return;
            }

            int needed = data.magazineSize - currentMagazine;
            int moved = Mathf.Min(needed, reserveAmmo);
            reserveAmmo -= moved;
            currentMagazine += moved;
        }

        public void AddReserveAmmo(int amount)
        {
            reserveAmmo += Mathf.Max(0, amount);
        }
    }
}
