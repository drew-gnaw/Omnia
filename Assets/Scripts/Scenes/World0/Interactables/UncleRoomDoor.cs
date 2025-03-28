using System.Collections;
using Initializers;
using Inventory;
using Players;
using UnityEngine;
using Utils;

public class UncleRoomDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueWrapper notPickedUpDialogue;
    [SerializeField] private Trinket musicBox;

    public virtual void Interact() {
        StartCoroutine(InteractCoroutine());
    }

    private IEnumerator InteractCoroutine() {
        if (!ShotgunPickup.pickedUp || (musicBox && musicBox.isLocked)) {
            yield return DialogueManager.Instance.StartDialogue(notPickedUpDialogue.Dialogue);
        } else {
            LevelManager.Instance.NextLevel();
        }
    }
}
