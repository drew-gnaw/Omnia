using System;
using Omnia.State;
using Player.Alt.Animation;
using Player.Alt.Behaviour;
using UnityEngine;

namespace Player.Alt {
    public class Player : MonoBehaviour {
        [SerializeField] internal SpriteRenderer sprite;
        [SerializeField] internal Animator animator;
        [SerializeField] internal Rigidbody2D rb;
        [SerializeField] internal LayerMask ground;
        [SerializeField] internal BoxCollider2D[] checks;

        [SerializeField] internal float maximumHealth;
        [SerializeField] internal float currentHealth;
        [SerializeField] internal float flow;
        [SerializeField] internal WeaponClass[] weapons;
        [SerializeField] internal int selectedWeapon;

        /* TODO: Is this bad? */
        [SerializeField] internal Camera cam;

        [SerializeField] internal Vector2 facing;
        [SerializeField] internal Vector2 moving;
        [SerializeField] internal bool jump;
        [SerializeField] internal bool held;
        [SerializeField] internal bool attack;
        [SerializeField] internal bool grounded;
        [SerializeField] internal Vector2 slide;

        [SerializeField] private string behaviourName;

        public event Action Spawn;
        public event Action Death;

        private IBehaviour behaviour;
        private StateMachine animationStateMachine;

        public void Awake() {
            UseBehaviour(new Move(this));
            UseAnimation(new StateMachine());
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
            animationStateMachine?.Update();
        }

        public void FixedUpdate() {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(10 * -1, rb.velocity.y));
            rb.gravityScale = held && rb.velocity.y > 0 ? 1 : 2;

            DoAttack();
            behaviour?.OnTick();
            animationStateMachine?.FixedUpdate();
        }

        public void Hurt(float damage) {
            currentHealth = Math.Clamp(currentHealth - damage, 0, maximumHealth);

            if (currentHealth == 0) Die();
        }

        public void UseBehaviour(IBehaviour it) {
            behaviour?.OnExit();
            behaviour = it;
            behaviour?.OnEnter();

            behaviourName = behaviour?.GetType().Name;
        }

        private void Die() {
            Death?.Invoke();
        }

        private void DoAttack() {
            if (attack) {
                weapons[selectedWeapon].Attack();
                attack = false;
            }
        }

        private void UseAnimation(StateMachine stateMachine) {
            var initial = new IdleAnimation(animator);

            stateMachine.AddAnyTransition(initial, new FuncPredicate(() => behaviour is Move && moving.x == 0));
            stateMachine.AddAnyTransition(new MoveAnimation(animator), new FuncPredicate(() => behaviour is Move && moving.x != 0));
            stateMachine.AddAnyTransition(new JumpAnimation(animator), new FuncPredicate(() => behaviour is Jump or WallJump));
            stateMachine.AddAnyTransition(new FallAnimation(animator), new FuncPredicate(() => behaviour is Fall));
            stateMachine.AddAnyTransition(new SlideAnimation(animator), new FuncPredicate(() => behaviour is Slide));

            stateMachine.SetState(initial);
            animationStateMachine = stateMachine;
        }
    }
}