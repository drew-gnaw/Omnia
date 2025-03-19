using System.Collections;
using Initializers;
using UnityEngine;
using Utils;

public class UncleRoomDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueWrapper notPickedUpDialogue;

    public virtual void Interact() {
        StartCoroutine(InteractCoroutine());
    }

    private IEnumerator InteractCoroutine() {
        if (!ShotgunPickup.pickedUp) {
            yield return DialogueManager.Instance.StartDialogue(notPickedUpDialogue.Dialogue);
        } else {
            LevelManager.Instance.NextLevel();
        }
    }
}
