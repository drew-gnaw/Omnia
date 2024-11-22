using System;
using UnityEngine;

namespace Omnia.Player
{
    public class PlayerBase
    {
        // Health is a float internally
        public float MaxHealth { get; private set; }
        public float CurrentHealth { get; private set; }

        public float MaxFlow { get; private set; }
        public float CurrentFlow { get; private set; }
        public bool IsAlive => CurrentHealth > 0;

        // The conversion ratio for flow to health: E.g. a ratio of 0.2 means 100 flow heals 20 HP
        private float flowToHealth = 0.2f;
        
        public event Action<float> OnHealthChanged;           // Triggered when health changes
        public event Action<float> OnFlowChanged;           // Triggered when flow meter changes
        public event Action OnPlayerDeath;                  // Triggered when the player dies
        
        public PlayerBase(int maxHealth = 100, float maxFlow = 100f)
        {
            MaxHealth = maxHealth;
            CurrentHealth = maxHealth;
            MaxFlow = maxFlow;
            CurrentFlow = 0;
        }
        
        public void TakeDamage(int damage)
        {
            if (!IsAlive) return;

            CurrentHealth -= damage;
            if (CurrentHealth < 0) CurrentHealth = 0;

            OnHealthChanged?.Invoke(CurrentHealth);

            if (CurrentHealth == 0)
            {
                Die();
            }
        }

        public void Heal(float amount)
        {
            if (!IsAlive) return;

            CurrentHealth += amount;
            if (CurrentHealth > MaxHealth) CurrentHealth = MaxHealth;

            OnHealthChanged?.Invoke(CurrentHealth);
        }
        
        public void UseALlFlow()
        {
            CurrentFlow = 0;
            Heal(flowToHealth * MaxFlow);

            OnFlowChanged?.Invoke(CurrentFlow);
        }

        public void DrainFlow(float amount)
        {
            if (CurrentFlow > 0)
            {
                CurrentFlow -= amount;
                CurrentHealth += flowToHealth * amount; 
            }

            
        }

        public void RegenerateFlow(float amount)
        {
            CurrentFlow += amount;
            if (CurrentFlow > MaxFlow) CurrentFlow = MaxFlow;

            OnFlowChanged?.Invoke(CurrentFlow);
        }
        
        private void Die()
        {
            OnPlayerDeath?.Invoke();
        }

        public void ResetStats()
        {
            CurrentHealth = MaxHealth;
            CurrentFlow = MaxFlow;

            OnHealthChanged?.Invoke(CurrentHealth);
            OnFlowChanged?.Invoke(CurrentFlow);
        }
    }
}
