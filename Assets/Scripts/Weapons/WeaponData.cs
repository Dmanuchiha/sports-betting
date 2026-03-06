using UnityEngine;

namespace FPSPrototype.Weapons
{
    public enum FireMode { SemiAuto, FullAuto, Shotgun }

    [CreateAssetMenu(fileName = "WeaponData", menuName = "FPS/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [Header("Identity")]
        public string weaponName;
        public FireMode fireMode;

        [Header("Ballistics")]
        public float damage = 20f;
        public float range = 120f;
        public float fireRate = 8f;
        public int pellets = 1;
        public float spread = 1.5f;

        [Header("Ammo")]
        public int magazineSize = 30;
        public int maxReserveAmmo = 180;
        public float reloadDuration = 2f;

        [Header("Recoil")]
        public float verticalRecoil = 1.2f;
        public float horizontalRecoil = 0.6f;
        public float recoilRecovery = 10f;

        [Header("Presentation")]
        public GameObject weaponPrefab;
        public Sprite icon;
    }
}
