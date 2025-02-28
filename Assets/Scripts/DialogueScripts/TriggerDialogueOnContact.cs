using System.Collections;
using System.Collections.Generic;
using Initializers;
using UnityEngine;

public class TriggerDialogueOnContact : MonoBehaviour
{
    [SerializeField] private DialogueWrapper dialogueWrapper;
    private bool triggered = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            if (!triggered) {
                triggered = true;
                StartCoroutine(DialogueManager.Instance.StartDialogue(dialogueWrapper.Dialogue));
            } else {
                // test scene changes
                SceneInitializer.LoadScene("MainScene");
            }
        }
    }
}
