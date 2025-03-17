using UnityEngine;

namespace Enemies.Bird.Animation {
    public class IdleAnimation : AnimationState {
        public IdleAnimation(Animator animator) : base("BirdIdle", animator) {
        }
    }

    public class AlertAnimation : AnimationState {
        public AlertAnimation(Animator animator) : base("BirdAlert", animator) {
        }
    }

    public class AttackAnimation : AnimationState {
        public AttackAnimation(Animator animator) : base("BirdAttack", animator) {
        }
    }
}
