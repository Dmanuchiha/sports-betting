using FPSPrototype.Player;
using FPSPrototype.Weapons;
using TMPro;
using UnityEngine;

namespace FPSPrototype.UI
{
    public class LoadoutMenuUI : MonoBehaviour
    {
        [SerializeField] private LoadoutData[] availableLoadouts;
        [SerializeField] private TMP_Text selectedLoadoutLabel;

        private int index;

        private void Start()
        {
            SetSelected(index);
        }

        public void NextLoadout()
        {
            if (availableLoadouts.Length == 0) return;
            index = (index + 1) % availableLoadouts.Length;
            SetSelected(index);
        }

        public void PreviousLoadout()
        {
            if (availableLoadouts.Length == 0) return;
            index = (index - 1 + availableLoadouts.Length) % availableLoadouts.Length;
            SetSelected(index);
        }

        public void ConfirmLoadout()
        {
            if (availableLoadouts.Length == 0) return;
            LoadoutSelectionCache.SelectedLoadout = availableLoadouts[index];
        }

        private void SetSelected(int idx)
        {
            if (availableLoadouts.Length == 0)
            {
                selectedLoadoutLabel.text = "No loadouts configured";
                return;
            }

            selectedLoadoutLabel.text = availableLoadouts[idx].loadoutName;
        }
    }
}
