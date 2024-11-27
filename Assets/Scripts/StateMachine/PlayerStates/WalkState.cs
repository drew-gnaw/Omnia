using Omnia.Player;
using UnityEngine;

namespace Omnia.State {
    public class WalkState : BaseState {
        public WalkState(PlayerController player, Animator animator) : base(player, animator) { }

        public override void OnEnter() {
            player.ZeroYVelocity();
            animator.CrossFade(Animator.StringToHash("RightJamieRun"), 0.1f);
        }

        public override void Update() {
            
        }

        public override void FixedUpdate()
        {
            float moveInput = Input.GetAxis("Horizontal");
            player.HandleMovement(moveInput);
        }
        
        public override void OnExit() {
            // Cleanup when exiting the walking state (e.g., reset animations)
        }

        
    }
}
