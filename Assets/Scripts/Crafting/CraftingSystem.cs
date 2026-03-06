using FPSPrototype.Combat;
using FPSPrototype.Weapons;
using UnityEngine;

namespace FPSPrototype.Crafting
{
    public class CraftingSystem : MonoBehaviour
    {
        [SerializeField] private ResourceInventory inventory;
        [SerializeField] private HealthShieldSystem vitals;
        [SerializeField] private WeaponManager weaponManager;
        [SerializeField] private int grenadeCount;
        [SerializeField] private int armorTier;

        public bool TryCraft(CraftingRecipe recipe)
        {
            if (!CanCraft(recipe)) return false;

            foreach (var cost in recipe.costs)
            {
                inventory.Spend(cost.type, cost.amount);
            }

            ApplyCraftResult(recipe);
            return true;
        }

        public bool CanCraft(CraftingRecipe recipe)
        {
            foreach (var cost in recipe.costs)
            {
                if (!inventory.Has(cost.type, cost.amount))
                {
                    return false;
                }
            }

            return true;
        }

        private void ApplyCraftResult(CraftingRecipe recipe)
        {
            switch (recipe.craftableType)
            {
                case CraftableType.ShieldPack:
                    vitals.RestoreShield(25f * recipe.outputAmount);
                    break;
                case CraftableType.AmmoPack:
                    WeaponInstance weapon = weaponManager.CurrentWeapon;
                    if (weapon != null)
                    {
                        weapon.AddAmmo(30 * recipe.outputAmount);
                    }
                    break;
                case CraftableType.Grenade:
                    grenadeCount += recipe.outputAmount;
                    break;
                case CraftableType.ArmorUpgrade:
                    armorTier = Mathf.Clamp(armorTier + recipe.outputAmount, 0, 5);
                    vitals.RestoreHealth(10f * recipe.outputAmount);
                    break;
            }
        }
    }
}
