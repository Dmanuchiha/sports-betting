using FPSPrototype.Weapons;
using UnityEngine;

namespace FPSPrototype.Player
{
    public class PlayerLoadoutSpawner : MonoBehaviour
    {
        [SerializeField] private WeaponManager weaponManager;
        [SerializeField] private LoadoutData defaultLoadout;

        private void Start()
        {
            LoadoutData selected = LoadoutSelectionCache.SelectedLoadout != null
                ? LoadoutSelectionCache.SelectedLoadout
                : defaultLoadout;

            if (selected != null)
            {
                weaponManager.InitializeFromLoadout(selected);
            }
        }
    }
}
