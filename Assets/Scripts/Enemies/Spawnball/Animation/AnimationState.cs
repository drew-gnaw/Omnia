using UnityEngine;

namespace Enemies.Spawnball.Animation {
    public class MoveAnimation : AnimationState {
        public MoveAnimation(Animator animator) : base("SpawnballMove", animator) {
        }
    }

    public class ActivateAnimation : AnimationState {
        public ActivateAnimation(Animator animator) : base("SpawnballActivate", animator) {
        }
    }
}
