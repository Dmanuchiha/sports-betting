using System;
using System.Collections.Generic;
using UnityEngine;

namespace FPSPrototype.Crafting
{
    public class ResourceInventory : MonoBehaviour
    {
        [Serializable]
        public struct ResourceStack
        {
            public ResourceType type;
            public int amount;
        }

        [SerializeField] private ResourceStack[] startingResources;

        private readonly Dictionary<ResourceType, int> resources = new();

        public event Action OnInventoryChanged;

        private void Awake()
        {
            foreach (ResourceType type in Enum.GetValues(typeof(ResourceType)))
            {
                resources[type] = 0;
            }

            foreach (ResourceStack stack in startingResources)
            {
                Add(stack.type, stack.amount);
            }
        }

        public int Get(ResourceType type) => resources.TryGetValue(type, out int amount) ? amount : 0;

        public void Add(ResourceType type, int amount)
        {
            resources[type] = Mathf.Max(0, Get(type) + amount);
            OnInventoryChanged?.Invoke();
        }

        public bool Has(ResourceType type, int amount) => Get(type) >= amount;

        public bool Spend(ResourceType type, int amount)
        {
            if (!Has(type, amount)) return false;
            resources[type] -= amount;
            OnInventoryChanged?.Invoke();
            return true;
        }
    }
}
