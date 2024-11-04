using Omnia.Player;
using UnityEngine;

namespace Omnia.State {
    public class SlideState : BaseState
    {
        public SlideState(PlayerController player, Animator animator) : base(player, animator) { }
        public override void OnEnter()
        {

        }

        public override void OnExit()
        {

        }

        public override void Update()
        {
            // Check if player is sliding left or right and adjust behavior accordingly
        }

        public override void FixedUpdate()
        {
            player.HandleSlide();
        }
    }
}

