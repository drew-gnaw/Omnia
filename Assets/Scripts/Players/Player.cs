using System;
using Omnia.Utils;
using Players.Behaviour;
using UI;
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
        [SerializeField] internal float maximumFlow;
        [SerializeField] internal float currentFlow;
        [SerializeField] internal float moveSpeed;
        [SerializeField] internal float jumpSpeed;
        [SerializeField] internal float fallSpeed;
        [SerializeField] internal float moveAccel;
        [SerializeField] internal float fallAccel;
        [SerializeField] internal float wallJumpLockoutTime;
        [SerializeField] internal float combatCooldown;
        [SerializeField] internal float flowDrainRate;


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
        [SerializeField] internal bool inCombat;
        [SerializeField] internal Vector2 slide;

        [SerializeField] internal string debugBehaviour;

        // Describes the ratio at which flow is converted into HP.
        public const float FLOW_TO_HP_RATIO = 0.2f;

        public event Action Spawn;
        public event Action Death;

        private CountdownTimer combatTimer;

        private IBehaviour behaviour;

        public void Awake() {
            UseBehaviour(Idle.AsDefaultOf(this));
        }

        public void Start() {
            currentHealth = maximumHealth;
            UIController.Instance.UpdatePlayerHealth(currentHealth, maximumHealth);

            currentFlow = 0;

            combatTimer = new CountdownTimer(combatCooldown);

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
            UpdateCombatTimer();
            Debug.Log("Flow: " + currentFlow);
        }

        public void FixedUpdate() {
            rb.velocity = new Vector2(rb.velocity.x, Mathf.Max(fallSpeed * -1, rb.velocity.y));
            rb.gravityScale = held && rb.velocity.y > 0 ? 1 : 2;

            DoAttack();
            behaviour?.OnTick();
        }

        // ***** Methods for handling player stats (HP, Flow) ***** //

        public void Hurt(float damage) {
            combatTimer.Start();
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maximumHealth);
            UIController.Instance.UpdatePlayerHealth(currentHealth, maximumHealth);

            if (currentHealth == 0) Die();
        }

        public void OnHit(float flowAmount) {
            combatTimer.Start();
            currentFlow = Mathf.Min(currentFlow + flowAmount, maximumFlow);
            UIController.Instance.UpdatePlayerFlow(currentFlow, maximumFlow);
        }

        public void ConsumeAllFlow() {
            if (currentFlow > 0) {
                float healthGain = currentFlow * FLOW_TO_HP_RATIO;
                currentHealth = Mathf.Clamp(currentHealth + healthGain, 0, maximumHealth);
                currentFlow = 0;
                UIController.Instance.UpdatePlayerFlow(currentFlow, maximumFlow);
            }
        }

        public void DrainFlowOverTime(float drainRate) {
            if (currentFlow > 0) {
                float flowToDrain = Mathf.Min(currentFlow, drainRate * Time.deltaTime);
                currentFlow -= flowToDrain;

                float healthGain = flowToDrain * FLOW_TO_HP_RATIO;
                currentHealth = Mathf.Clamp(currentHealth + healthGain, 0, maximumHealth);

                UIController.Instance.UpdatePlayerHealth(currentHealth, maximumHealth);
                UIController.Instance.UpdatePlayerFlow(currentFlow, maximumFlow);
            }
        }

        private void Die() {
            Death?.Invoke();
        }

        // ***** Methods for behaviour + animation ***** //

        private void DoAttack() {
            if (!fire) return;
            fire = false;
            weapons[selectedWeapon].Attack();
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

        // ***** Helpers ***** //

        private void UpdateCombatTimer() {
            combatTimer.Tick(Time.deltaTime);

            if (!combatTimer.IsRunning) {
                DrainFlowOverTime(flowDrainRate);
            }
        }
    }
}
