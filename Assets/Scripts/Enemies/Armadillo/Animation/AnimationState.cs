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
    
    public class StaggerAnimation : AnimationState {
        // TODO replace with proper stagger animation
        public StaggerAnimation(Animator animator) : base("ArmadilloIdle", animator) {
        }
    }
}
