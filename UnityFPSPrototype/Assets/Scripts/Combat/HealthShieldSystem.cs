using System;
using UnityEngine;

namespace FPSPrototype.Combat
{
    public class HealthShieldSystem : MonoBehaviour
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float maxShield = 100f;
        [SerializeField] private float shieldRegenRate = 18f;
        [SerializeField] private float shieldRegenDelay = 4f;

        public float CurrentHealth { get; private set; }
        public float CurrentShield { get; private set; }
        public float MaxHealth => maxHealth;
        public float MaxShield => maxShield;

        public event Action<float, float> OnHealthChanged;
        public event Action<float, float> OnShieldChanged;
        public event Action OnDied;

        private float lastDamageTime;

        private void Awake()
        {
            CurrentHealth = maxHealth;
            CurrentShield = maxShield;
        }

        private void Update()
        {
            if (Time.time - lastDamageTime >= shieldRegenDelay && CurrentShield < maxShield)
            {
                CurrentShield = Mathf.Min(maxShield, CurrentShield + shieldRegenRate * Time.deltaTime);
                OnShieldChanged?.Invoke(CurrentShield, maxShield);
            }
        }

        public void ApplyDamage(float amount)
        {
            if (amount <= 0f || CurrentHealth <= 0f)
            {
                return;
            }

            lastDamageTime = Time.time;
            float remaining = amount;

            if (CurrentShield > 0f)
            {
                float shieldDamage = Mathf.Min(CurrentShield, remaining);
                CurrentShield -= shieldDamage;
                remaining -= shieldDamage;
                OnShieldChanged?.Invoke(CurrentShield, maxShield);
            }

            if (remaining > 0f)
            {
                CurrentHealth = Mathf.Max(0f, CurrentHealth - remaining);
                OnHealthChanged?.Invoke(CurrentHealth, maxHealth);

                if (CurrentHealth <= 0f)
                {
                    OnDied?.Invoke();
                }
            }
        }

        public void RestoreHealth(float amount)
        {
            CurrentHealth = Mathf.Min(maxHealth, CurrentHealth + amount);
            OnHealthChanged?.Invoke(CurrentHealth, maxHealth);
        }

        public void AddShield(float amount)
        {
            CurrentShield = Mathf.Min(maxShield, CurrentShield + amount);
            OnShieldChanged?.Invoke(CurrentShield, maxShield);
        }
    }
}
