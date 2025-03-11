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

    public class RollAnimation : AnimationState {
        public RollAnimation(Animator animator) : base("PlayerRoll", animator) {
        }
    }

    public class SlideLAnimation : AnimationState {
        public SlideLAnimation(Animator animator) : base("PlayerSlideL", animator) {
        }
    }

    public class SlideRAnimation : AnimationState {
        public SlideRAnimation(Animator animator) : base("PlayerSlideR", animator) {
        }
    }

    public class MoveBackwardAnimation : AnimationState {
        public MoveBackwardAnimation(Animator animator) : base("PlayerMoveBackward", animator) {
        }
    }

    public abstract class AnimationState : IState {
        private readonly int name;
        private readonly Animator animator;

        protected AnimationState(string name, Animator animator) {
            this.animator = animator;
            this.name = Animator.StringToHash(name);
        }

        public void OnEnter() {
            animator.Play(name);
        }

        public void Update() {
        }

        public void OnExit() {
        }

        public void FixedUpdate() {
        }
    }
}
