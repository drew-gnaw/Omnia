using UnityEngine;

namespace FX {
    public class TransientVisualEffect : MonoBehaviour {
        [SerializeField] internal Animator animator;

        public void Start() => Destroy(gameObject, animator.GetCurrentAnimatorStateInfo(0).length);
    }
}
