using UnityEngine;

namespace Enemies.Bird.Animation {
    public class IdleAnimation : AnimationState {
        public IdleAnimation(Animator animator) : base("BirdIdle", animator) {
        }
    }

    public class DiveAnimation : AnimationState {
        public DiveAnimation(Animator animator) : base("BirdDive", animator) {
        }
    }

    public class BombAnimation : AnimationState {
        public BombAnimation(Animator animator) : base("BirdBomb", animator) {
        }
    }
}
