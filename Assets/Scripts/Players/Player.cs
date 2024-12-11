using System;
using Players.Behaviour;
using UnityEngine;
using UnityEngine.Serialization;

namespace Players {
    public class Player : MonoBehaviour {
        [SerializeField] internal SpriteRenderer sprite;
        [SerializeField] internal Animator animator;
        [SerializeField] internal Rigidbody2D rb;
        [SerializeField] internal LayerMask ground;
        [SerializeField] internal BoxCollider2D[] checks;

        [SerializeField] internal float maximumHealth;
        [SerializeField] internal float currentHealth;
        [SerializeField] internal float moveSpeed;
        [SerializeField] internal float jumpSpeed;
        [SerializeField] internal float pullSpeed;
        [SerializeField] internal float moveAccel;
        [SerializeField] internal float jumpAccel;
        [SerializeField] internal float jumpLockoutTime;
        [SerializeField] internal float flow;

        [SerializeField] internal WeaponClass[] weapons;
        [SerializeField] internal int selectedWeapon;

        /* TODO: Is this bad? */
        [SerializeField] internal Camera cam;

        [SerializeField] internal Vector2 facing;
        [SerializeField] internal Vector2 moving;
        [SerializeField] internal bool jump;
        [SerializeField] internal bool held;
        [SerializeField] internal bool fire;
        [SerializeField] internal bool grounded;
        [SerializeField] internal Vector2 slide;

        [SerializeField] internal string debugBehaviour;

        [SerializeField] internal Transform debugPullToTargetTransform;

        public event Action Spawn;
        public event Action Death;

        private IBehaviour behaviour;

        public void Awake() {
            UseBehaviour(new Idle(this));
        }

        public void Start() {
            currentHealth = maximumHealth;
            flow = 0;

            Transform weaponsTransform = transform.Find("Weapons");
            if (weaponsTransform != null) {
                weapons = weaponsTransform.GetComponentsInChildren<WeaponClass>();
            } else {
                Debug.LogError("Weapons object not found as a child of the player!");
            }

            Spawn?.Invoke();
        }

        public void Update() {
            sprite.flipX = facing.x == 0 ? sprite.flipX : facing.x < 0;
            behaviour?.OnUpdate();

            if (Input.GetKeyDown("e")) {
                /* TODO: Remove. */
                UsePull(debugPullToTargetTransform);
            }
        }

        public void FixedUpdate() {
            rb.gravityScale = held && rb.velocity.y >= 0 ? 1 : 2;

            DoAttack();
            behaviour?.OnTick();
        }

        public void UsePull(Transform target) {
            UseBehaviour(new Pull(this, target));
        }

        public void Hurt(float damage) {
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maximumHealth);

            if (currentHealth == 0) Die();
        }

        public void UseBehaviour(IBehaviour it) {
            if (it == null) return;
            debugBehaviour = it.GetType().Name;

            behaviour?.OnExit();
            behaviour = it;
            behaviour?.OnEnter();
        }

        public void UseAnimation(string it) {
            animator.Play(it);
        }

        public bool IsPhoon() {
            return Math.Abs(rb.velocity.x) > moveSpeed && Math.Sign(rb.velocity.x) == Math.Sign(moving.x);
        }

        private void Die() {
            Death?.Invoke();
        }

        private void DoAttack() {
            if (!fire) return;
            fire = false;
            weapons[selectedWeapon].Attack();
        }
    }
}
