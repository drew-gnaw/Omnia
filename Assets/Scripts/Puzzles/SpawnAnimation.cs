using System.Collections;
using UnityEngine;

public class SpawnAnimation : MonoBehaviour {
    [SerializeField] private Animator animator;

    void Start() {
        PlayAnimation();
    }

    void PlayAnimation() {
        StartCoroutine(DestroyAfterAnimation());
    }

    IEnumerator DestroyAfterAnimation() {
        yield return new WaitForSeconds(animator.GetCurrentAnimatorStateInfo(0).length);
        Destroy(gameObject);
    }
}

