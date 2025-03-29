using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UncleRoomManager : MonoBehaviour {
    [SerializeField] private DialogueWrapper flowUnlockedDialogue;
    private int dialogueProgress = 0;

    void OnEnable() {
        DialogueBox.DialogueBoxEvent += HandleDialogueEvent;
    }

    void OnDisable() {
        DialogueBox.DialogueBoxEvent -= HandleDialogueEvent;
    }

    private void HandleDialogueEvent() {
        dialogueProgress++;
    }

    private void Start() {
        StartCoroutine(ProgressDialogue());
    }

    IEnumerator ProgressDialogue() {
        yield return new WaitUntil(() => dialogueProgress == 2);
        yield return new WaitUntil(() => !DialogueManager.Instance.IsInDialogue());
        yield return DialogueManager.Instance.StartDialogue(flowUnlockedDialogue.Dialogue);
    }
}
