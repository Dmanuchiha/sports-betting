using FPSPrototype.Crafting;
using TMPro;
using UnityEngine;

namespace FPSPrototype.UI
{
    public class CraftingMenuUI : MonoBehaviour
    {
        [SerializeField] private GameObject craftingPanel;
        [SerializeField] private TMP_Text feedbackText;
        [SerializeField] private CraftingSystem craftingSystem;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                bool show = !craftingPanel.activeSelf;
                craftingPanel.SetActive(show);
                Cursor.lockState = show ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = show;
            }
        }

        public void CraftShield() => TryCraft(CraftingRecipe.ShieldPack);
        public void CraftAmmo() => TryCraft(CraftingRecipe.AmmoPack);
        public void CraftGrenade() => TryCraft(CraftingRecipe.Grenade);
        public void CraftArmor() => TryCraft(CraftingRecipe.ArmorUpgrade);

        private void TryCraft(CraftingRecipe recipe)
        {
            bool success = craftingSystem.TryCraft(recipe);
            feedbackText.text = success ? $"Crafted {recipe}" : "Not enough resources";
        }
    }
}
