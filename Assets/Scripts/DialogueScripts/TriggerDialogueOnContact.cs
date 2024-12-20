using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerDialogueOnContact : MonoBehaviour
{
    [SerializeField] private DialogueWrapper dialogueWrapper;
    private bool triggered = false;
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!triggered && other.gameObject.layer == LayerMask.NameToLayer("Player"))
        {
            triggered = true;
            StartCoroutine(DialogueManager.Instance.StartDialogue(dialogueWrapper.Dialogue));
        }
    }
}
