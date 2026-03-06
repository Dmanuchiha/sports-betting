using System;
using FPSPrototype.Core;
using UnityEngine;

namespace FPSPrototype.Combat
{
    public class HealthShieldSystem : MonoBehaviour, IDamageable
    {
        [SerializeField] private float maxHealth = 100f;
        [SerializeField] private float maxShield = 100f;
        [SerializeField] private float shieldRegenPerSecond = 20f;
        [SerializeField] private float regenDelay = 4f;

        public float Health { get; private set; }
        public float Shield { get; private set; }
        public float MaxHealth => maxHealth;
        public float MaxShield => maxShield;

        public event Action<float, float> OnVitalsChanged;
        public event Action OnDied;

        private float lastDamageTime = -999f;

        private void Awake()
        {
            Health = maxHealth;
            Shield = maxShield;
            OnVitalsChanged?.Invoke(Health, Shield);
        }

        private void Update()
        {
            if (Time.time - lastDamageTime >= regenDelay && Shield < maxShield)
            {
                Shield = Mathf.Min(maxShield, Shield + shieldRegenPerSecond * Time.deltaTime);
                OnVitalsChanged?.Invoke(Health, Shield);
            }
        }

        public void TakeDamage(DamageInfo info)
        {
            lastDamageTime = Time.time;
            float remaining = info.Amount;

            if (Shield > 0f)
            {
                float shieldDamage = Mathf.Min(Shield, remaining);
                Shield -= shieldDamage;
                remaining -= shieldDamage;
            }

            if (remaining > 0f)
            {
                Health -= remaining;
            }

            Health = Mathf.Max(0f, Health);
            OnVitalsChanged?.Invoke(Health, Shield);

            if (Health <= 0f)
            {
                OnDied?.Invoke();
                gameObject.SetActive(false);
            }
        }

        public void RestoreHealth(float amount)
        {
            Health = Mathf.Clamp(Health + amount, 0f, maxHealth);
            OnVitalsChanged?.Invoke(Health, Shield);
        }

        public void RestoreShield(float amount)
        {
            Shield = Mathf.Clamp(Shield + amount, 0f, maxShield);
            OnVitalsChanged?.Invoke(Health, Shield);
        }
    }
}
