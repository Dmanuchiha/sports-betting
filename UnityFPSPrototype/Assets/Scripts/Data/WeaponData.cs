using UnityEngine;

namespace FPSPrototype.Data
{
    public enum WeaponType
    {
        AssaultRifle,
        SMG,
        Sniper,
        Shotgun
    }

    [CreateAssetMenu(fileName = "WeaponData", menuName = "FPS Prototype/Weapon Data")]
    public class WeaponData : ScriptableObject
    {
        [Header("Identity")]
        public string weaponName;
        public WeaponType weaponType;
        public Sprite weaponIcon;

        [Header("Combat")]
        public bool automatic = true;
        public float damage = 20f;
        public float fireRate = 10f;
        public float range = 150f;
        public float spread = 0.01f;
        public int pellets = 1;

        [Header("Ammo")]
        public int magazineSize = 30;
        public int reserveAmmo = 120;
        public float reloadDuration = 1.8f;

        [Header("Recoil")]
        public Vector2 recoilKick = new Vector2(1.3f, 0.75f);
        public float recoilReturnSpeed = 12f;
        public float recoilSnappiness = 18f;

        [Header("FX")]
        public GameObject muzzleFlashPrefab;
        public AudioClip fireSound;
        public AudioClip reloadSound;
    }
}
