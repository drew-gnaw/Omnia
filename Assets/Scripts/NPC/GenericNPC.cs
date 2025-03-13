using UnityEngine;

public class GenericNPC : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueWrapper npcDialogue;
    public void Interact() {
        StartCoroutine(DialogueManager.Instance.StartDialogue(npcDialogue.Dialogue));
    }
}
