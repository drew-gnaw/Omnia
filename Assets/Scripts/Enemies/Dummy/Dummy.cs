using Omnia.State;
using UnityEngine;

namespace Enemies.Dummy {
    public class Dummy : Enemy {
        public override void Hurt(float damage) {
            base.Hurt(damage);
            Debug.Log(this + " took damage " + damage);
        }

        public override void UseBehaviour(IBehaviour it) {
            // wit nothin'?
        }

        protected override void UseAnimation(StateMachine stateMachine) {
            // wit nothin'?
        }
    }
}
