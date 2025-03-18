using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class ShotgunPickup : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueWrapper pickupDialogue;
    [SerializeField] private GameObject makeInteractable;
    [SerializeField] private SpriteRenderer shotgunSprite;

    public static bool pickedUp = false;
    public virtual void Interact() {
        StartCoroutine(InteractCoroutine());
    }

    private IEnumerator InteractCoroutine() {
        yield return DialogueManager.Instance.StartDialogue(pickupDialogue.Dialogue);
        pickedUp = true;
        shotgunSprite.enabled = false;
    }
}
