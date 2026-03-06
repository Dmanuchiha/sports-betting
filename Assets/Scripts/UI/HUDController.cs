using FPSPrototype.Combat;
using FPSPrototype.Crafting;
using FPSPrototype.Weapons;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace FPSPrototype.UI
{
    public class HUDController : MonoBehaviour
    {
        [SerializeField] private HealthShieldSystem vitals;
        [SerializeField] private WeaponManager weaponManager;
        [SerializeField] private ResourceInventory inventory;

        [Header("Vitals")]
        [SerializeField] private Slider healthBar;
        [SerializeField] private Slider shieldBar;

        [Header("Weapon")]
        [SerializeField] private TMP_Text weaponNameText;
        [SerializeField] private TMP_Text ammoText;

        [Header("Crafting Inventory")]
        [SerializeField] private TMP_Text resourcesText;

        private WeaponInstance cachedWeapon;

        private void Start()
        {
            vitals.OnVitalsChanged += UpdateVitals;
            inventory.OnInventoryChanged += UpdateResources;
            UpdateVitals(vitals.Health, vitals.Shield);
            UpdateResources();
        }

        private void Update()
        {
            if (weaponManager.CurrentWeapon != cachedWeapon)
            {
                if (cachedWeapon != null)
                {
                    cachedWeapon.OnAmmoChanged -= UpdateWeapon;
                }

                cachedWeapon = weaponManager.CurrentWeapon;
                if (cachedWeapon != null)
                {
                    cachedWeapon.OnAmmoChanged += UpdateWeapon;
                    UpdateWeapon(cachedWeapon);
                }
            }
        }

        private void UpdateVitals(float health, float shield)
        {
            healthBar.maxValue = vitals.MaxHealth;
            shieldBar.maxValue = vitals.MaxShield;
            healthBar.value = health;
            shieldBar.value = shield;
        }

        private void UpdateWeapon(WeaponInstance weapon)
        {
            weaponNameText.text = weapon.Data.weaponName;
            ammoText.text = $"{weapon.CurrentMagazine}/{weapon.ReserveAmmo}";
        }

        private void UpdateResources()
        {
            resourcesText.text =
                $"Metal: {inventory.Get(ResourceType.Metal)}\n" +
                $"Electronics: {inventory.Get(ResourceType.Electronics)}\n" +
                $"Energy Cells: {inventory.Get(ResourceType.EnergyCells)}";
        }
    }
}
