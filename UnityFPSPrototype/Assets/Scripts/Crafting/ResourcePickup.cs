using UnityEngine;

namespace FPSPrototype.Crafting
{
    public class ResourcePickup : MonoBehaviour
    {
        [SerializeField] private ResourceType resourceType;
        [SerializeField] private int amount = 2;

        private void OnTriggerEnter(Collider other)
        {
            if (!other.CompareTag("Player"))
            {
                return;
            }

            CraftingSystem crafting = other.GetComponent<CraftingSystem>();
            if (crafting != null)
            {
                crafting.AddResource(resourceType, amount);
                Destroy(gameObject);
            }
        }
    }
}
