using UnityEngine;

namespace Enemies.Armadillo.Animation {
    public class IdleAnimation : AnimationState {
        public IdleAnimation(Animator animator) : base("ArmadilloIdle", animator) {
        }
    }

    public class WalkAnimation : AnimationState {
        public WalkAnimation(Animator animator) : base("ArmadilloWalk", animator) {
        }
    }

    public class RushAnimation : AnimationState {
        public RushAnimation(Animator animator) : base("ArmadilloRush", animator) {
        }
    }

    public class StunAnimation : AnimationState {
        public StunAnimation(Animator animator) : base("ArmadilloStun", animator) {
        }
    }

    /** TODO: This is a common utility class and should be moved. */
    public abstract class AnimationState : IState {
        private readonly string name;
        private readonly Animator animator;

        protected AnimationState(string name, Animator animator) {
            this.name = name;
            this.animator = animator;
        }

        public void OnEnter() {
            animator.CrossFade(name, 0.1f);
        }

        public void Update() {
        }

        public void FixedUpdate() {
        }

        public void OnExit() {
        }
    }
}
