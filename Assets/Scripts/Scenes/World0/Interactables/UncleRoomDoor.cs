using Inventory;
using System.Collections;
using UnityEngine;

public class UncleRoomDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueWrapper notPickedUpDialogue;
    [SerializeField] private Trinket musicBox;

    public virtual void Interact() {
        StartCoroutine(InteractCoroutine());
    }

    private IEnumerator InteractCoroutine() {
        if (!ShotgunPickup.pickedUp || (musicBox && musicBox.IsLocked)) {
            yield return DialogueManager.Instance.StartDialogue(notPickedUpDialogue.Dialogue);
        } else {
            LevelManager.Instance.NextLevel();
        }
    }
}
