using UnityEngine;

namespace Enemies.Sundew.Animation {
    public class IdleAnimation : AnimationState {
        public IdleAnimation(Animator animator) : base("SundewIdle", animator) {
        }
    }

    public class AttackAnimation : AnimationState {
        public AttackAnimation(Animator animator) : base("SundewAttack", animator) {
        }
    }

    public class ReloadAnimation : AnimationState {
        public ReloadAnimation(Animator animator) : base("SundewReload", animator) {
        }
    }

    public class WindUpAnimation : AnimationState {
        public WindUpAnimation(Animator animator) : base("SundewWindUp", animator) {
        }
    }

    public class StaggerAnimation : AnimationState {
        // TODO replace with proper stagger animation
        public StaggerAnimation(Animator animator) : base("SundewIdle", animator) {
        }
    }
}
