using System.Collections;
using UnityEngine;

public class GenericNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueWrapper npcDialogue;
    public virtual void Interact() {
        StartCoroutine(InteractCoroutine());
    }

    private IEnumerator InteractCoroutine() {
        yield return DialogueManager.Instance.StartDialogue(npcDialogue.Dialogue);
    }

}
