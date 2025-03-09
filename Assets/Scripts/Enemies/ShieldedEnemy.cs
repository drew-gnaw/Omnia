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

        override public void Hurt(float damage)
        {
            if (shieldHealth <= 0)
            {
                base.Hurt(damage);
            } else {
                shieldHealth = Mathf.Max(shieldHealth - damage, 0);
                base.Hurt(0f);
            }
        }
    }
}