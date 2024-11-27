using Omnia.Player;
using UnityEngine;

namespace Omnia.State {
    public class JumpState : BaseState {
        public JumpState(PlayerController player, Animator animator) : base(player, animator) { }

        public override void OnEnter() {
            // Set jump parameters, play jump animation, etc.
        }

        public override void Update() {
            // Any logic to run while in the jump state
            // You might want to handle some jump-specific behavior here
        }

        public override void FixedUpdate() {
            player.HandleJump(); // Handle jumping logic here
        }

        public override void OnExit() {
            // Reset jump parameters or perform any cleanup needed
        }
    }
}
