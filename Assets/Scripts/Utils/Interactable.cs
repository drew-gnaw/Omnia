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
        if (collision.gameObject.CompareTag("Player")) {
            inRange = true;
            buttonIcon.enabled = true;
            //Debug.Log("can now interact");
            //Debug.Log("null check:" + mainScript == null);
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            inRange = false;
            buttonIcon.enabled = false;
            //Debug.Log("can no longer interact");
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
