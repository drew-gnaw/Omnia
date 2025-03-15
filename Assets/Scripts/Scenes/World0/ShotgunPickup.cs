using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShotgunPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueWrapper pickupDialogue;
    public virtual void Interact() {
        StartCoroutine(InteractCoroutine());
    }

    private IEnumerator InteractCoroutine() {
        yield return DialogueManager.Instance.StartDialogue(pickupDialogue.Dialogue);
    }
}
