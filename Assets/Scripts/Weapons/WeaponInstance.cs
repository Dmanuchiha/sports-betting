using System;
using FPSPrototype.Core;
using UnityEngine;

namespace FPSPrototype.Weapons
{
    public class WeaponInstance : MonoBehaviour
    {
        [SerializeField] private Transform firePoint;
        [SerializeField] private LayerMask hitMask;

        public WeaponData Data { get; private set; }
        public int CurrentMagazine { get; private set; }
        public int ReserveAmmo { get; private set; }
        public bool IsReloading => reloadTimer > 0f;

        public event Action<WeaponInstance> OnAmmoChanged;

        private float fireCooldown;
        private float reloadTimer;

        public void Initialize(WeaponData data)
        {
            Data = data;
            CurrentMagazine = data.magazineSize;
            ReserveAmmo = data.maxReserveAmmo;
            fireCooldown = 0f;
            reloadTimer = 0f;
            OnAmmoChanged?.Invoke(this);
        }

        private void Update()
        {
            if (fireCooldown > 0f) fireCooldown -= Time.deltaTime;

            if (reloadTimer > 0f)
            {
                reloadTimer -= Time.deltaTime;
                if (reloadTimer <= 0f)
                {
                    CompleteReload();
                }
            }
        }

        public bool TryFire(Camera viewCamera, GameObject instigator)
        {
            if (IsReloading || fireCooldown > 0f || CurrentMagazine <= 0) return false;

            int shots = Mathf.Max(1, Data.fireMode == FireMode.Shotgun ? Data.pellets : 1);
            for (int i = 0; i < shots; i++)
            {
                Vector3 spread = UnityEngine.Random.insideUnitSphere * (Data.spread * 0.01f);
                Vector3 direction = (viewCamera.transform.forward + spread).normalized;

                if (Physics.Raycast(viewCamera.transform.position, direction, out RaycastHit hit, Data.range, hitMask, QueryTriggerInteraction.Ignore))
                {
                    if (hit.collider.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.TakeDamage(new DamageInfo(Data.damage, hit.point, hit.normal, instigator));
                    }
                }
            }

            CurrentMagazine--;
            fireCooldown = 1f / Data.fireRate;
            OnAmmoChanged?.Invoke(this);
            return true;
        }

        public bool StartReload()
        {
            if (IsReloading || CurrentMagazine >= Data.magazineSize || ReserveAmmo <= 0) return false;
            reloadTimer = Data.reloadDuration;
            return true;
        }

        private void CompleteReload()
        {
            int needed = Data.magazineSize - CurrentMagazine;
            int load = Mathf.Min(needed, ReserveAmmo);
            CurrentMagazine += load;
            ReserveAmmo -= load;
            OnAmmoChanged?.Invoke(this);
        }

        public void AddAmmo(int amount)
        {
            ReserveAmmo = Mathf.Clamp(ReserveAmmo + amount, 0, Data.maxReserveAmmo);
            OnAmmoChanged?.Invoke(this);
        }
    }
}
