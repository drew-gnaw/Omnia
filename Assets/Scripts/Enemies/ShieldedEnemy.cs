using Enemies;
using UnityEngine;

namespace Enemies
{
    public abstract class ShieldedEnemy : Enemy
    {
        [SerializeField] internal float maxShieldHealth;
        public float shieldHealth
        {
            get;
            protected set;
        }
        
        override public void Start()
        {
            shieldHealth = maxShieldHealth;
            base.Start();
        }

        override public void Hurt(float damage, bool stagger = true, bool crit = false)
        {
            if (shieldHealth <= 0)
            {
                base.Hurt(damage, crit: crit);
            } else {
                shieldHealth = Mathf.Max(shieldHealth - damage, 0);
                // This is a workaround to prevent the actual health from being hurt
                // while applying the other effects of Enemy's hurt
                var health = currentHealth;
                base.Hurt(damage, stagger: false, crit: crit);
                currentHealth = health;
            }
        }
    }
}