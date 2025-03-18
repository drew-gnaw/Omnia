using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Dinky : MonoBehaviour, IInteractable
{
    [SerializeField] private Interactable interactable;
    [SerializeField] private GameObject graphics;
    [SerializeField] private string idleState;
    [SerializeField] private string appearState;
    [SerializeField] private string disappearState;
    [FormerlySerializedAs("tempDialogue")] [SerializeField] private DialogueWrapper interactDialogue;

    // Not sure this is how it should be handled, could stay this simple if Dinky's interactions are totally linear
    [SerializeField] private List<Transform> locations;

    private Animator animator;

    void Start() {
       animator = graphics.GetComponent<Animator>();
    }

    private bool animating = false;

    public void Appear(Transform t) {
        gameObject.transform.position = t.position;
        animator.Play(appearState);
        setVisible(true);
        animating = true;
    }

    public void Disappear() {
        interactable?.SetEnable(false);
        animator.Play(disappearState);
        animating = true;
    }

    public void Interact() {
        // This is a demo of how Dinky could interact
        StartCoroutine(DialogueManager.Instance.StartDialogue(interactDialogue.Dialogue));
        // I think a list of observers in DialogueManager is worth looking into if other classes are interested
        // in listening to the end of the dialogue, otherwise this will probably be fine

        //StartCoroutine(dinkyReappearElsewhere());
    }

    // demo of dinky behaviour
    private IEnumerator dinkyReappearElsewhere() {
        yield return new WaitUntil(DialogueManager.Instance.IsInDialogue);
        Disappear();
        yield return new WaitUntil(() => !animating);
        Appear(locations[Random.Range(0, locations.Count)]);
    }

    private void appeared() {
        animating = false;
        interactable?.SetEnable(true);
        animator.Play(idleState);
    }

    private void disappeared() {
        animating = false;
        setVisible(false);
    }

    private void setVisible(bool visible) {
        graphics.GetComponentInChildren<Renderer>().enabled = visible;
    }

    void Update()
    {
        AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

        // This is a pretty jank way to check if animation has completed, but probably quickest and easiest
        if (animating && stateInfo.normalizedTime >= 1.0f)
        {
            if (stateInfo.IsName(appearState)) {
                appeared();
            } else if (stateInfo.IsName(disappearState)) {
                disappeared();
            }
        }
    }
}
