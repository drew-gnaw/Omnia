using System;
using Omnia.State;

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
