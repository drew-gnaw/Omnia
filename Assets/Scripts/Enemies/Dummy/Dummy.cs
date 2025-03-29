using System;
using Omnia.State;

namespace Enemies.Dummy {
    public class Dummy : Enemy {
        public event Action<Dummy> OnHurt;
        public bool canBeHurt = true;
        protected override void UseAnimation(StateMachine stateMachine) {
            // nothing...
        }

        public override void Hurt(float damage, bool stagger = true) {
            if (!canBeHurt) return;
            base.Hurt(damage);
            OnHurt?.Invoke(this);
        }

    }
}
