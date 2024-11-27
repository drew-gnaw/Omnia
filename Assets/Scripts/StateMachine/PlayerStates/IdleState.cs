using Omnia.Player;
using UnityEngine;

namespace Omnia.State {
    public class IdleState : BaseState {
        public IdleState(PlayerController player, Animator animator) : base(player, animator) { }

        public override void OnEnter() {
            animator.CrossFade(Animator.StringToHash("RightJamieIdle"), 0.1f);
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
