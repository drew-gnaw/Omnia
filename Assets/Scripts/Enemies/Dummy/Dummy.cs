using Omnia.State;
using UnityEngine;

namespace Enemies.Dummy {
    public class Dummy : Enemy, IInteractable {
        [SerializeField] private DialogueWrapper dummyDialogue;
        public override void Hurt(float damage) {
            base.Hurt(damage);
            Debug.Log(this + " took damage " + damage);
            LevelManager.Instance.NextLevel(); //Test Scene changes by killing this dude
        }

        protected override void UseAnimation(StateMachine stateMachine) {
            // nothing...
        }

        public void Interact() {
            StartCoroutine(DialogueManager.Instance.StartDialogue(dummyDialogue.Dialogue));
        }
    }
}
