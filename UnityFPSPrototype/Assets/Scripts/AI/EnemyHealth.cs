using FPSPrototype.Weapons;
using UnityEngine;

namespace FPSPrototype.AI
{
    public class EnemyHealth : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;

        private float currentHealth;

        private void Awake()
        {
            currentHealth = maxHealth;
        }

        public void ReceiveDamage(float amount)
        {
            currentHealth -= amount;
            if (currentHealth <= 0f)
            {
                Die();
            }
        }

        private void Die()
        {
            Destroy(gameObject);
        }
    }
}
