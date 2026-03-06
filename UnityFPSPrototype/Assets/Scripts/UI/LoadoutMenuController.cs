using FPSPrototype.Data;
using FPSPrototype.Weapons;
using TMPro;
using UnityEngine;

namespace FPSPrototype.UI
{
    public class LoadoutMenuController : MonoBehaviour
    {
        [SerializeField] private GameObject menuRoot;
        [SerializeField] private TMP_Dropdown loadoutDropdown;
        [SerializeField] private LoadoutData[] loadouts;
        [SerializeField] private WeaponController weaponController;

        private bool hasSpawned;

        private void Start()
        {
            loadoutDropdown.ClearOptions();
            var options = new System.Collections.Generic.List<string>();
            foreach (LoadoutData loadout in loadouts)
            {
                options.Add(loadout != null ? loadout.loadoutName : "Empty");
            }

            loadoutDropdown.AddOptions(options);
            Time.timeScale = 0f;
            menuRoot.SetActive(true);
        }

        public void SpawnWithSelectedLoadout()
        {
            if (hasSpawned || loadouts.Length == 0)
            {
                return;
            }

            int index = Mathf.Clamp(loadoutDropdown.value, 0, loadouts.Length - 1);
            weaponController.BuildWeaponsFromLoadout(loadouts[index]);

            menuRoot.SetActive(false);
            Time.timeScale = 1f;
            hasSpawned = true;
        }
    }
}
