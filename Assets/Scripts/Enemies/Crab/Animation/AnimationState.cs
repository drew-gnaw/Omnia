using UnityEngine;

namespace Enemies.Crab.Animation {
    public class IdleAnimation : AnimationState {
        public IdleAnimation(Animator animator) : base("CrabIdle", animator) {
        }
    }

    public class PopOutAnimation : AnimationState {
        public PopOutAnimation(Animator animator) : base("CrabPopOut", animator) {
        }
    }

    public class HideAnimation : AnimationState {
        public HideAnimation(Animator animator) : base("CrabHide", animator) {
        }
    }
}
