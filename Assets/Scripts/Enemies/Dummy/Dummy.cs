using UnityEngine;

namespace Enemies.Dummy {
    public class Dummy : Enemy, IInteractable {
        [SerializeField] private DialogueWrapper dummyDialogue;
        public override void Hurt(float damage) {
            base.Hurt(damage);
            Debug.Log(this + " took damage " + damage);
        }

        public void Interact() {
            StartCoroutine(DialogueManager.Instance.StartDialogue(dummyDialogue.Dialogue));
        }
    }
}
