using UnityEngine;

namespace Enemies.Armadillo.Animation {
    public class IdleAnimation : AnimationState {
        public IdleAnimation(Animator animator) : base("ArmadilloIdle", animator) {
        }
    }

    public class MoveAnimation : AnimationState {
        public MoveAnimation(Animator animator) : base("ArmadilloMove", animator) {
        }
    }

    public class AlertAnimation : AnimationState {
        public AlertAnimation(Animator animator) : base("ArmadilloAlert", animator) {
        }
    }

    public class RollAnimation : AnimationState {
        public RollAnimation(Animator animator) : base("ArmadilloRoll", animator) {
        }
    }

    public class RecoilAnimation : AnimationState {
        public RecoilAnimation(Animator animator) : base("ArmadilloRecoil", animator) {
        }
    }

    public class UncurlAnimation : AnimationState {
        public UncurlAnimation(Animator animator) : base("ArmadilloUncurl", animator) {
        }
    }
}
