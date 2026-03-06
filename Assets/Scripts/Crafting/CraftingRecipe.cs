using System;
using UnityEngine;

namespace FPSPrototype.Crafting
{
    public enum CraftableType
    {
        ShieldPack,
        AmmoPack,
        Grenade,
        ArmorUpgrade
    }

    [CreateAssetMenu(fileName = "CraftingRecipe", menuName = "FPS/Crafting Recipe")]
    public class CraftingRecipe : ScriptableObject
    {
        [Serializable]
        public struct Cost
        {
            public ResourceType type;
            public int amount;
        }

        public string recipeName;
        public CraftableType craftableType;
        public Cost[] costs;
        public int outputAmount = 1;
    }
}
