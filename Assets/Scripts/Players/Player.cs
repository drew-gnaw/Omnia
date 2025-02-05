using System;
using System.Collections;
using System.Collections.Generic;
using Omnia.Utils;
using Players.Behaviour;
using UI;
using UnityEngine;
using Utils;

namespace Players {
    public class Player : MonoBehaviour {
        [SerializeField] internal SpriteRenderer sprite;
        [SerializeField] internal Animator animator;
        [SerializeField] internal Rigidbody2D rb;
        [SerializeField] internal CapsuleCollider2D cc;
        [SerializeField] internal LayerMask ground;
        [SerializeField] internal LayerMask semisolid;
        [SerializeField] internal BoxCollider2D[] checks;

        [SerializeField] internal float maximumHealth;
        [SerializeField] internal float currentHealth;
        [SerializeField] internal float maximumFlow;
        [SerializeField] internal float currentFlow;
        [SerializeField] internal float moveSpeed;
        [SerializeField] internal float jumpSpeed;
        [SerializeField] internal float pullSpeed;
        [SerializeField] internal float rollSpeed;
        [SerializeField] internal float rollDuration;
        [SerializeField] internal float rollCooldown;
        [SerializeField] internal float rollThreshold;
        [SerializeField] internal float moveAccel;
        [SerializeField] internal float fallAccel;
        [SerializeField] internal float jumpLockoutTime;
        [SerializeField] internal float flow;
        [SerializeField] internal float weaponRecoilLockoutTime;
        [SerializeField] internal float wallJumpLockoutTime;
        [SerializeField] internal float combatCooldown;
        [SerializeField] internal float flowDrainRate;
        [SerializeField] internal float hurtInvulnerabilityTime;

        [SerializeField] internal WeaponClass[] weapons;
        [SerializeField] internal int selectedWeapon;

        /* TODO: Is this bad? */
        [SerializeField] internal Camera cam;

        [SerializeField] internal Vector2 facing;
        [SerializeField] internal Vector2 moving;
        [SerializeField] internal bool jump;
        [SerializeField] internal bool roll;
        [SerializeField] internal bool held;
        [SerializeField] internal bool fire;
        [SerializeField] internal bool skill;
        [SerializeField] internal bool grounded;
        [SerializeField] internal bool canRoll;
        [SerializeField] internal bool invulnerable;
        [SerializeField] internal bool inCombat;
        [SerializeField] internal Vector2 slide;

        [SerializeField] internal string debugBehaviour;
        [SerializeField] internal Transform debugPullToTargetTransform;

        // Describes the ratio at which flow is converted into HP.
        public const float FLOW_TO_HP_RATIO = 0.2f;

        public event Action Spawn;
        public event Action Death;

        private float currentLockout;
        private float maximumLockout;
        private float currentHurtInvulnerability;

        private CountdownTimer combatTimer;
        private CountdownTimer rollCooldownTimer;

        private IBehaviour behaviour;

        public void Awake() {
            UseBehaviour(Idle.AdHoc(this));
        }

        public void Start() {
            currentHealth = maximumHealth;
            UIController.Instance.UpdatePlayerHealth(currentHealth, maximumHealth);

            currentFlow = 0;

            combatTimer = new CountdownTimer(combatCooldown);

            rollCooldownTimer = new CountdownTimer(rollCooldown);
            canRoll = true;

            Transform weaponsTransform = transform.Find("Weapons");
            if (weaponsTransform != null) {
                weapons = weaponsTransform.GetComponentsInChildren<WeaponClass>();
            } else {
                Debug.LogError("Weapons object not found as a child of the player!");
            }

            Spawn?.Invoke();
        }

        public void Update() {
            currentLockout = Mathf.Clamp(currentLockout - Time.deltaTime, 0, maximumLockout);
            behaviour?.OnUpdate();
            UpdateCombatTimer();
            UpdateRollCooldownTimer();

            sprite.flipX = facing.x == 0 ? sprite.flipX : facing.x < 0;
            currentHurtInvulnerability = Mathf.Max(0, currentHurtInvulnerability - Time.deltaTime);
        }

        public void FixedUpdate() {
            rb.gravityScale = !held || rb.velocity.y < 0 ? 2 : 1;
            DoAttack();
            DoSkill();
            behaviour?.OnTick();
        }

        public void UsePull(Transform target) {
            UseBehaviour(new Pull(this, target));
        }

        public void UseRecoil(float speed) {
            var recoil = -1 * speed * facing.normalized;
            UseExternalVelocity(new Vector2(rb.velocity.x + recoil.x, recoil.y), weaponRecoilLockoutTime);
        }

        // ***** Methods for handling player stats (HP, Flow) ***** //

        public void Hurt(float damage, Vector2 velocity = default, float lockout = 0) {
            if (invulnerable || currentHurtInvulnerability > 0) return;

            // Apply any buffs that reduce incoming damage
            foreach (var modifier in Buff.Buff.OnDamageTaken) {
                damage = modifier(damage);
            }

            combatTimer.Start();
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maximumHealth);
            UIController.Instance.UpdatePlayerHealth(currentHealth, maximumHealth);

            currentHurtInvulnerability = hurtInvulnerabilityTime;
            UseExternalVelocity(velocity, lockout);
            StartCoroutine(DoHurtInvincibilityFlicker());

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
                UIController.Instance.UpdatePlayerHealth(currentHealth, maximumHealth);
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

        public void UseExternalVelocity(Vector2 velocity, float lockout) {
            rb.velocity = velocity;
            currentLockout = maximumLockout = lockout;
        }

        public float HorizontalVelocityOf(float x, float acceleration) {
            if (maximumLockout == 0) return MathUtils.Lerpish(rb.velocity.x, x, acceleration);

            var control = 1 - currentLockout / maximumLockout;
            return MathUtils.Lerpish(rb.velocity.x, x, control * acceleration);
        }

        internal bool IsPhoon() {
            return Math.Abs(rb.velocity.x) > moveSpeed && Math.Sign(rb.velocity.x) == Math.Sign(moving.x);
        }

        // Called by Behaviour.Roll to handle the cooldown timer
        internal void OnRoll() {
            canRoll = false;
            rollCooldownTimer.Start();
        }

        private void DoAttack() {
            if (!fire) return;
            fire = false;
            Debug.Log("attacking");
            weapons[selectedWeapon].Attack();
        }

        private void DoSkill() {
            if (!skill) return;
            skill = false;
            weapons[selectedWeapon].UseSkill();
        }

        public void DoSwap(int targetWeapon) {
            if (selectedWeapon != targetWeapon) {
                weapons[selectedWeapon].SetSpriteActive(false);

                selectedWeapon = targetWeapon;

                weapons[selectedWeapon].SetSpriteActive(true);

                if (Mathf.Approximately(currentFlow, maximumFlow)) {
                    ConsumeAllFlow();
                    weapons[selectedWeapon].IntroSkill();
                }

                Debug.Log($"Swapped to weapon {targetWeapon}");
            }
        }


        // ***** Helpers ***** //

        private void UpdateCombatTimer() {
            combatTimer.Tick(Time.deltaTime);

            if (!combatTimer.IsRunning) {
                DrainFlowOverTime(flowDrainRate);
            }
        }

        private void UpdateRollCooldownTimer() {
            rollCooldownTimer.Tick(Time.deltaTime);

            if (!rollCooldownTimer.IsRunning) {
                canRoll = true;
            }
        }

        /* This is kind of lazy but it works. */
        private IEnumerator DoHurtInvincibilityFlicker() {
            while (currentHurtInvulnerability > 0) {
                sprite.enabled = false;
                yield return new WaitForSeconds(0.02f);

                sprite.enabled = true;
                yield return new WaitForSeconds(0.10f);
            }
        }
    }
}
