using System.Collections.Generic;
using UnityEngine;

namespace FPSPrototype.Weapons
{
    public class WeaponManager : MonoBehaviour
    {
        [SerializeField] private Camera playerCamera;
        [SerializeField] private Transform weaponSocket;
        [SerializeField] private float recoilKickback = 2f;
        [SerializeField] private float recoilReturnSpeed = 10f;

        public WeaponInstance CurrentWeapon => weapons.Count == 0 ? null : weapons[currentIndex];

        private readonly List<WeaponInstance> weapons = new();
        private int currentIndex;
        private Vector3 recoilEuler;

        private void Update()
        {
            if (CurrentWeapon == null) return;

            HandleInput();
            recoilEuler = Vector3.Lerp(recoilEuler, Vector3.zero, Time.deltaTime * recoilReturnSpeed);
            weaponSocket.localRotation = Quaternion.Euler(recoilEuler);
        }

        public void InitializeFromLoadout(LoadoutData loadout)
        {
            ClearWeapons();

            TrySpawnWeapon(loadout.primaryWeapon);
            TrySpawnWeapon(loadout.secondaryWeapon);
            TrySpawnWeapon(loadout.heavyWeapon);

            Equip(0);
        }

        private void HandleInput()
        {
            bool trigger = CurrentWeapon.Data.fireMode == FireMode.FullAuto
                ? Input.GetButton("Fire1")
                : Input.GetButtonDown("Fire1");

            if (trigger && CurrentWeapon.TryFire(playerCamera, gameObject))
            {
                recoilEuler += new Vector3(-CurrentWeapon.Data.verticalRecoil * recoilKickback,
                    Random.Range(-CurrentWeapon.Data.horizontalRecoil, CurrentWeapon.Data.horizontalRecoil), 0f);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                CurrentWeapon.StartReload();
            }

            if (Input.GetKeyDown(KeyCode.Alpha1)) Equip(0);
            if (Input.GetKeyDown(KeyCode.Alpha2)) Equip(1);
            if (Input.GetKeyDown(KeyCode.Alpha3)) Equip(2);

            float scroll = Input.GetAxisRaw("Mouse ScrollWheel");
            if (scroll > 0f) Equip((currentIndex + 1) % weapons.Count);
            if (scroll < 0f) Equip((currentIndex - 1 + weapons.Count) % weapons.Count);
        }

        private void TrySpawnWeapon(WeaponData data)
        {
            if (data == null || data.weaponPrefab == null) return;

            GameObject instance = Instantiate(data.weaponPrefab, weaponSocket);
            WeaponInstance weapon = instance.GetComponent<WeaponInstance>();
            if (weapon == null)
            {
                weapon = instance.AddComponent<WeaponInstance>();
            }

            weapon.Initialize(data);
            instance.SetActive(false);
            weapons.Add(weapon);
        }

        private void Equip(int index)
        {
            if (weapons.Count == 0 || index < 0 || index >= weapons.Count) return;

            for (int i = 0; i < weapons.Count; i++)
            {
                weapons[i].gameObject.SetActive(i == index);
            }

            currentIndex = index;
        }

        private void ClearWeapons()
        {
            foreach (WeaponInstance weapon in weapons)
            {
                if (weapon != null)
                {
                    Destroy(weapon.gameObject);
                }
            }
            weapons.Clear();
            currentIndex = 0;
        }
    }
}
