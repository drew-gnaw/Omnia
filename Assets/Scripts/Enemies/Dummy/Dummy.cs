using System;
using Omnia.State;
using UnityEngine;

namespace Enemies.Dummy {
    public class Dummy : Enemy {
        public event Action OnHurt;
        protected override void UseAnimation(StateMachine stateMachine) {
            // nothing...
        }

        public override void Hurt(float damage, bool stagger = true) {
            base.Hurt(damage);
            OnHurt?.Invoke();
        }

    }
}
