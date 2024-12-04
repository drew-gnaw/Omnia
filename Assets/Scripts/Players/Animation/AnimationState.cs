using UnityEngine;

namespace Players.Animation {
    public class IdleAnimation : AnimationState {
        public IdleAnimation(Animator animator) : base("PlayerIdle", animator) {
        }
    }

    public class MoveAnimation : AnimationState {
        public MoveAnimation(Animator animator) : base("PlayerMove", animator) {
        }
    }

    public class JumpAnimation : AnimationState {
        public JumpAnimation(Animator animator) : base("PlayerJump", animator) {
        }
    }

    public class FallAnimation : AnimationState {
        public FallAnimation(Animator animator) : base("PlayerFall", animator) {
        }
    }

    public class SlideAnimation : AnimationState {
        public SlideAnimation(Animator animator) : base("PlayerSlide", animator) {
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
