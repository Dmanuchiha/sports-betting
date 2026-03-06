using System;
using System.Collections;
using System.Collections.Generic;
using FPSPrototype.Data;
using FPSPrototype.Player;
using UnityEngine;

namespace FPSPrototype.Weapons
{
    public class WeaponController : MonoBehaviour
    {
        [SerializeField] private Camera fpsCamera;
        [SerializeField] private Transform weaponHolder;
        [SerializeField] private LoadoutData startingLoadout;
        [SerializeField] private LayerMask hitMask = ~0;
        [SerializeField] private float recoilVisualMultiplier = 1f;

        public WeaponData CurrentWeaponData => currentIndex >= 0 && currentIndex < weapons.Count ? weapons[currentIndex].data : null;
        public WeaponInstance CurrentWeapon => currentIndex >= 0 && currentIndex < weapons.Count ? weapons[currentIndex] : null;

        public event Action<WeaponInstance> OnWeaponChanged;
        public event Action<WeaponInstance> OnAmmoChanged;

        private readonly List<WeaponInstance> weapons = new List<WeaponInstance>();
        private int currentIndex = -1;
        private float nextFireTime;
        private bool isReloading;

        private Vector3 targetWeaponPos;
        private Vector3 currentWeaponPos;

        private void Start()
        {
            BuildWeaponsFromLoadout(startingLoadout);
            SelectWeapon(0);
        }

        private void Update()
        {
            if (CurrentWeaponData == null)
            {
                return;
            }

            HandleShootInput();
            HandleReloadInput();
            HandleSwitchInput();
            UpdateWeaponRecoilVisual();
        }

        public void BuildWeaponsFromLoadout(LoadoutData loadout)
        {
            weapons.Clear();
            if (loadout == null)
            {
                return;
            }

            TryAddWeapon(loadout.primaryWeapon);
            TryAddWeapon(loadout.secondaryWeapon);
            TryAddWeapon(loadout.specialWeapon);
        }

        private void TryAddWeapon(WeaponData data)
        {
            if (data != null)
            {
                weapons.Add(new WeaponInstance(data));
            }
        }

        private void HandleShootInput()
        {
            bool wantsFire = CurrentWeaponData.automatic ? Input.GetButton("Fire1") : Input.GetButtonDown("Fire1");
            if (!wantsFire || isReloading || Time.time < nextFireTime)
            {
                return;
            }

            if (!CurrentWeapon.CanFire())
            {
                StartCoroutine(ReloadRoutine());
                return;
            }

            Fire();
        }

        private void Fire()
        {
            WeaponData data = CurrentWeaponData;
            nextFireTime = Time.time + 1f / Mathf.Max(0.01f, data.fireRate);
            CurrentWeapon.ConsumeAmmo();
            OnAmmoChanged?.Invoke(CurrentWeapon);

            for (int i = 0; i < Mathf.Max(1, data.pellets); i++)
            {
                Vector3 direction = GetSpreadDirection(data.spread);
                if (Physics.Raycast(fpsCamera.transform.position, direction, out RaycastHit hit, data.range, hitMask, QueryTriggerInteraction.Ignore))
                {
                    if (hit.collider.TryGetComponent(out IDamageable damageable))
                    {
                        damageable.ReceiveDamage(data.damage);
                    }
                }
            }

            targetWeaponPos += new Vector3(UnityEngine.Random.Range(-data.recoilKick.y, data.recoilKick.y), data.recoilKick.x, -data.recoilKick.x) * recoilVisualMultiplier * 0.01f;
        }

        private Vector3 GetSpreadDirection(float spread)
        {
            Vector3 direction = fpsCamera.transform.forward;
            direction += fpsCamera.transform.right * UnityEngine.Random.Range(-spread, spread);
            direction += fpsCamera.transform.up * UnityEngine.Random.Range(-spread, spread);
            return direction.normalized;
        }

        private void HandleReloadInput()
        {
            if (Input.GetKeyDown(KeyCode.R))
            {
                StartCoroutine(ReloadRoutine());
            }
        }

        private IEnumerator ReloadRoutine()
        {
            if (isReloading || CurrentWeapon == null || !CurrentWeapon.CanReload())
            {
                yield break;
            }

            isReloading = true;
            yield return new WaitForSeconds(CurrentWeaponData.reloadDuration);
            CurrentWeapon.Reload();
            OnAmmoChanged?.Invoke(CurrentWeapon);
            isReloading = false;
        }

        private void HandleSwitchInput()
        {
            if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            {
                SelectWeapon((currentIndex + 1) % weapons.Count);
            }
            else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            {
                int idx = currentIndex - 1;
                if (idx < 0) idx = weapons.Count - 1;
                SelectWeapon(idx);
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)) SelectWeapon(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) SelectWeapon(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) SelectWeapon(2);
        }

        private void SelectWeapon(int index)
        {
            if (weapons.Count == 0 || index < 0 || index >= weapons.Count)
            {
                return;
            }

            currentIndex = index;
            OnWeaponChanged?.Invoke(CurrentWeapon);
            OnAmmoChanged?.Invoke(CurrentWeapon);
        }

        private void UpdateWeaponRecoilVisual()
        {
            targetWeaponPos = Vector3.Lerp(targetWeaponPos, Vector3.zero, CurrentWeaponData.recoilReturnSpeed * Time.deltaTime);
            currentWeaponPos = Vector3.Lerp(currentWeaponPos, targetWeaponPos, CurrentWeaponData.recoilSnappiness * Time.deltaTime);
            if (weaponHolder != null)
            {
                weaponHolder.localPosition = currentWeaponPos;
            }
        }

        public void AddAmmoToCurrentWeapon(int amount)
        {
            if (CurrentWeapon == null)
            {
                return;
            }

            CurrentWeapon.AddReserveAmmo(amount);
            OnAmmoChanged?.Invoke(CurrentWeapon);
        }
    }
}
