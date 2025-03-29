using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Dinky : MonoBehaviour, IInteractable
{
    [SerializeField] private Interactable interactable;
    [SerializeField] private GameObject graphics;
    [SerializeField] private Animator animator;
    [FormerlySerializedAs("tempDialogue")] [SerializeField] private DialogueWrapper interactDialogue;
    [SerializeField] private List<Transform> locations;

    private static readonly int AppearTrigger = Animator.StringToHash("Appear");
    private static readonly int DisappearTrigger = Animator.StringToHash("Disappear");
    private static readonly int IdleTrigger = Animator.StringToHash("Idle");

    public static event Action OnInteract;

    private void Awake()
    {
        if (!animator && graphics)
        {
            animator = graphics.GetComponent<Animator>();
        }
    }

    public void Appear(Transform t)
    {
        if (!animator) return;

        gameObject.transform.position = t.position;
        setVisible(true);
        animator.SetTrigger(AppearTrigger);
    }

    public void Disappear()
    {
        if (!animator) return;

        interactable?.SetEnable(false);
        animator.SetTrigger(DisappearTrigger);
    }

    public void Interact()
    {
        StartCoroutine(StartDialogue());
    }

    private IEnumerator StartDialogue()
    {
        yield return StartCoroutine(DialogueManager.Instance.StartDialogue(interactDialogue.Dialogue));
        OnInteract?.Invoke();
    }


    private void setVisible(bool visible)
    {
        if (graphics)
        {
            Renderer renderer = graphics.GetComponentInChildren<Renderer>();
            if (renderer) renderer.enabled = visible;
        }
    }
}
