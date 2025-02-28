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

    public class HideAnimation : AnimationState {
        public HideAnimation(Animator animator) : base("SundewHide", animator) {
        }
    }

    public class RevealAnimation : AnimationState {
        public RevealAnimation(Animator animator) : base("SundewReveal", animator) {
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
