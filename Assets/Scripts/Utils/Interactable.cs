using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactable : MonoBehaviour
{
    private bool inRange = false;
    private IInteractable mainScript;
    [SerializeField] private SpriteRenderer buttonIcon;

    void Start() {
        buttonIcon.enabled = false;
        mainScript = GetComponentInParent<IInteractable>();
    }
    private void OnTriggerEnter2D(Collider2D collision) {
        Rigidbody2D rb = collision.attachedRigidbody;
        if (rb != null && rb.gameObject.CompareTag("Player")) {
            inRange = true;
            buttonIcon.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        Rigidbody2D rb = collision.attachedRigidbody;
        if (rb != null && rb.gameObject.CompareTag("Player")) {
            inRange = false;
            buttonIcon.enabled = false;
        }
    }

    public bool GetPlayerRange() {
        return inRange;
    }

    void Update() {
        if (mainScript == null) return;
        if (inRange && Input.GetKeyDown(KeyCode.E)) {
            inRange = false;
            buttonIcon.enabled = false;
            mainScript.Interact();
        }
    }

    void OnDestroy() {
        mainScript = null;
        buttonIcon = null;
    }
}
