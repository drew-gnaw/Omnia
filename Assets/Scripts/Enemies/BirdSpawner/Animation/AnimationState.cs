using UnityEngine;

namespace Enemies.BirdSpawner.Animation {
    public class IdleAnimation : AnimationState {
        public IdleAnimation(Animator animator) : base("BirdSpawnerIdle", animator) {
        }
    }

    public class BarfAnimation : AnimationState {
        public BarfAnimation(Animator animator) : base("BirdSpawnerBarf", animator) {
        }
    }

    public class DeadAnimation : AnimationState {
        public DeadAnimation(Animator animator) : base("BirdSpawnerDead", animator) {
        }
    }
}
