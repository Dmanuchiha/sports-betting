using System;
using System.Collections.Generic;
using FPSPrototype.Combat;
using FPSPrototype.Weapons;
using UnityEngine;

namespace FPSPrototype.Crafting
{
    public enum ResourceType
    {
        Metal,
        Electronics,
        EnergyCells
    }

    public enum CraftingRecipe
    {
        ShieldPack,
        AmmoPack,
        Grenade,
        ArmorUpgrade
    }

    public class CraftingSystem : MonoBehaviour
    {
        [SerializeField] private WeaponController weaponController;
        [SerializeField] private HealthShieldSystem healthShieldSystem;

        [SerializeField] private int metal;
        [SerializeField] private int electronics;
        [SerializeField] private int energyCells;

        public event Action OnInventoryChanged;

        private readonly Dictionary<CraftingRecipe, Dictionary<ResourceType, int>> recipeCosts =
            new Dictionary<CraftingRecipe, Dictionary<ResourceType, int>>
            {
                {
                    CraftingRecipe.ShieldPack,
                    new Dictionary<ResourceType, int>
                    {
                        { ResourceType.Metal, 4 },
                        { ResourceType.Electronics, 2 },
                        { ResourceType.EnergyCells, 3 }
                    }
                },
                {
                    CraftingRecipe.AmmoPack,
                    new Dictionary<ResourceType, int>
                    {
                        { ResourceType.Metal, 4 },
                        { ResourceType.EnergyCells, 1 }
                    }
                },
                {
                    CraftingRecipe.Grenade,
                    new Dictionary<ResourceType, int>
                    {
                        { ResourceType.Metal, 2 },
                        { ResourceType.Electronics, 3 }
                    }
                },
                {
                    CraftingRecipe.ArmorUpgrade,
                    new Dictionary<ResourceType, int>
                    {
                        { ResourceType.Metal, 6 },
                        { ResourceType.Electronics, 4 },
                        { ResourceType.EnergyCells, 4 }
                    }
                }
            };

        public int GetAmount(ResourceType type)
        {
            return type switch
            {
                ResourceType.Metal => metal,
                ResourceType.Electronics => electronics,
                ResourceType.EnergyCells => energyCells,
                _ => 0
            };
        }

        public void AddResource(ResourceType type, int amount)
        {
            if (amount == 0) return;

            switch (type)
            {
                case ResourceType.Metal:
                    metal = Mathf.Max(0, metal + amount);
                    break;
                case ResourceType.Electronics:
                    electronics = Mathf.Max(0, electronics + amount);
                    break;
                case ResourceType.EnergyCells:
                    energyCells = Mathf.Max(0, energyCells + amount);
                    break;
            }

            OnInventoryChanged?.Invoke();
        }

        public bool TryCraft(CraftingRecipe recipe)
        {
            if (!HasResources(recipe))
            {
                return false;
            }

            SpendResources(recipe);
            ApplyRecipe(recipe);
            OnInventoryChanged?.Invoke();
            return true;
        }

        private bool HasResources(CraftingRecipe recipe)
        {
            foreach (KeyValuePair<ResourceType, int> pair in recipeCosts[recipe])
            {
                if (GetAmount(pair.Key) < pair.Value)
                {
                    return false;
                }
            }

            return true;
        }

        private void SpendResources(CraftingRecipe recipe)
        {
            foreach (KeyValuePair<ResourceType, int> pair in recipeCosts[recipe])
            {
                AddResource(pair.Key, -pair.Value);
            }
        }

        private void ApplyRecipe(CraftingRecipe recipe)
        {
            switch (recipe)
            {
                case CraftingRecipe.ShieldPack:
                    healthShieldSystem.AddShield(35f);
                    break;
                case CraftingRecipe.AmmoPack:
                    weaponController.AddAmmoToCurrentWeapon(45);
                    break;
                case CraftingRecipe.Grenade:
                    // Placeholder for grenade inventory hookup.
                    break;
                case CraftingRecipe.ArmorUpgrade:
                    healthShieldSystem.RestoreHealth(20f);
                    break;
            }
        }
    }
}
