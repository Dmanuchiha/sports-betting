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
        [Header("References")]
        [SerializeField] private HealthShieldSystem playerVitals;
        [SerializeField] private WeaponController weaponController;
        [SerializeField] private CraftingSystem craftingSystem;

        [Header("Bars")]
        [SerializeField] private Slider healthSlider;
        [SerializeField] private Slider shieldSlider;

        [Header("Weapon Text")]
        [SerializeField] private TMP_Text weaponNameText;
        [SerializeField] private TMP_Text ammoText;

        [Header("Crafting Inventory")]
        [SerializeField] private TMP_Text metalText;
        [SerializeField] private TMP_Text electronicsText;
        [SerializeField] private TMP_Text energyCellsText;

        private void Start()
        {
            playerVitals.OnHealthChanged += UpdateHealth;
            playerVitals.OnShieldChanged += UpdateShield;
            weaponController.OnWeaponChanged += UpdateWeapon;
            weaponController.OnAmmoChanged += UpdateAmmo;
            craftingSystem.OnInventoryChanged += UpdateInventory;

            UpdateHealth(playerVitals.CurrentHealth, playerVitals.MaxHealth);
            UpdateShield(playerVitals.CurrentShield, playerVitals.MaxShield);
            UpdateWeapon(weaponController.CurrentWeapon);
            UpdateAmmo(weaponController.CurrentWeapon);
            UpdateInventory();
        }

        private void OnDestroy()
        {
            if (playerVitals != null)
            {
                playerVitals.OnHealthChanged -= UpdateHealth;
                playerVitals.OnShieldChanged -= UpdateShield;
            }

            if (weaponController != null)
            {
                weaponController.OnWeaponChanged -= UpdateWeapon;
                weaponController.OnAmmoChanged -= UpdateAmmo;
            }

            if (craftingSystem != null)
            {
                craftingSystem.OnInventoryChanged -= UpdateInventory;
            }
        }

        private void UpdateHealth(float current, float max)
        {
            healthSlider.value = max <= 0 ? 0 : current / max;
        }

        private void UpdateShield(float current, float max)
        {
            shieldSlider.value = max <= 0 ? 0 : current / max;
        }

        private void UpdateWeapon(WeaponInstance weapon)
        {
            weaponNameText.text = weapon?.data != null ? weapon.data.weaponName : "No Weapon";
        }

        private void UpdateAmmo(WeaponInstance weapon)
        {
            if (weapon?.data == null)
            {
                ammoText.text = "-- / --";
                return;
            }

            ammoText.text = $"{weapon.currentMagazine} / {weapon.reserveAmmo}";
        }

        private void UpdateInventory()
        {
            metalText.text = $"Metal: {craftingSystem.GetAmount(ResourceType.Metal)}";
            electronicsText.text = $"Electronics: {craftingSystem.GetAmount(ResourceType.Electronics)}";
            energyCellsText.text = $"Energy Cells: {craftingSystem.GetAmount(ResourceType.EnergyCells)}";
        }
    }
}
