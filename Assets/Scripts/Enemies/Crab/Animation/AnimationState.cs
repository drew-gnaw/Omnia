using UnityEngine;

namespace Enemies.Crab.Animation {
    public class IdleAnimation : AnimationState {
        public IdleAnimation(Animator animator) : base("CrabIdle", animator) {
        }
    }

    public class PopOutLAnimation : AnimationState {
        public PopOutLAnimation(Animator animator) : base("CrabPopOutL", animator) {
        }
    }

    public class PopOutRAnimation : AnimationState {
        public PopOutRAnimation(Animator animator) : base("CrabPopOutR", animator) {
        }
    }

    public class HideLAnimation : AnimationState {
        public HideLAnimation(Animator animator) : base("CrabHideL", animator) {
        }
    }

    public class HideRAnimation : AnimationState {
        public HideRAnimation(Animator animator) : base("CrabHideR", animator) {
        }
    }
}
