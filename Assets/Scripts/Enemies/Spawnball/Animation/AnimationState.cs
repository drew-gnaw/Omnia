using UnityEngine;

namespace Enemies.Spawnball.Animation {
    public class IdleAnimation : AnimationState {
        public IdleAnimation(Animator animator) : base("SpawnballIdle", animator) {
        }
    }

    public class ActivateAnimation : AnimationState {
        public ActivateAnimation(Animator animator) : base("SpawnballActivate", animator) {
        }
    }
}
