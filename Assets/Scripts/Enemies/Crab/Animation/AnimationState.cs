using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemies.Crab.Animation {
    public class IdleAnimation : AnimationState {
        public IdleAnimation(Animator animator) : base("CrabIdle", animator) {
        }
    }

    public class CenterPopOutAnimation : AnimationState {
        public CenterPopOutAnimation(Animator animator) : base("CrabCenterPopOut", animator) {
        }
    }

    public class DirectionalPopOutAnimation : AnimationState {
        public DirectionalPopOutAnimation(Animator animator) : base("CrabDirectionalPopOut", animator) {
        }
    }

    public class DirectionalHideAnimation : AnimationState {
        public DirectionalHideAnimation(Animator animator) : base("CrabDirectionalHide", animator) {
        }
    }
}
