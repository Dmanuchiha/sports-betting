using UnityEngine;

namespace FPSPrototype.Crafting
{
    public class ResourceNode : MonoBehaviour
    {
        [SerializeField] private ResourceType resourceType;
        [SerializeField] private int amount = 10;
        [SerializeField] private KeyCode interactKey = KeyCode.E;

        private bool playerInRange;
        private ResourceInventory playerInventory;

        private void Update()
        {
            if (playerInRange && Input.GetKeyDown(interactKey) && playerInventory != null)
            {
                playerInventory.Add(resourceType, amount);
                Destroy(gameObject);
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out ResourceInventory inventory))
            {
                playerInventory = inventory;
                playerInRange = true;
            }
        }

        private void OnTriggerExit(Collider other)
        {
            if (other.TryGetComponent(out ResourceInventory inventory) && inventory == playerInventory)
            {
                playerInventory = null;
                playerInRange = false;
            }
        }
    }
}
