using FPSPrototype.Crafting;
using TMPro;
using UnityEngine;

namespace FPSPrototype.UI
{
    public class CraftingMenuUI : MonoBehaviour
    {
        [SerializeField] private CraftingSystem craftingSystem;
        [SerializeField] private CraftingRecipe[] recipes;
        [SerializeField] private TMP_Text feedbackText;
        [SerializeField] private GameObject rootPanel;

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                rootPanel.SetActive(!rootPanel.activeSelf);
            }
        }

        public void CraftByIndex(int index)
        {
            if (index < 0 || index >= recipes.Length) return;

            CraftingRecipe recipe = recipes[index];
            bool crafted = craftingSystem.TryCraft(recipe);
            feedbackText.text = crafted ? $"Crafted: {recipe.recipeName}" : $"Missing resources for {recipe.recipeName}";
        }
    }
}
