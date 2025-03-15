using System.Collections;
using Initializers;
using UnityEngine;
using Utils;

public class UncleRoomDoor : MonoBehaviour, IInteractable
{
    [SerializeField] private DialogueWrapper notPickedUpDialogue;
    [SerializeField] private FadeScreenHandler fadeScreen;



    public void Start() {
        fadeScreen.SetLightScreen();
    }
    public virtual void Interact() {
        StartCoroutine(InteractCoroutine());
    }

    private IEnumerator InteractCoroutine() {
        if (!ShotgunPickup.pickedUp) {
            yield return DialogueManager.Instance.StartDialogue(notPickedUpDialogue.Dialogue);
        } else {
            SceneInitializer.LoadScene("6_City");
        }
    }
}
