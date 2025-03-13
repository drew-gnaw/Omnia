using System.Linq;
using Enemies.Crab.Animation;
using Enemies.Crab.Behaviour;
using Omnia.State;
using Players;
using UnityEngine;
using Utils;

namespace Enemies.Crab {
    public class Crab : Enemy {
        [SerializeField] internal SpriteRenderer sprite;
        [SerializeField] internal Vector2 direction; // Where the player has been detected
        [SerializeField] private Animator animator;
        [SerializeField] private float playerDetectionDistance;
        [SerializeField] private LayerMask player;

        [Header("Crab Properties")]
        [SerializeField] internal float windup; // the time that the alert phase lasts
        [SerializeField] internal float vulnerableTime; //  the time after the attack that the crab is still vulnerable.
        [SerializeField] internal float reload; // the minimum time in between retreating and popping back out.
        [SerializeField] internal BoxCollider2D crabAttackArea;
        [SerializeField] private Transform leftAttackPosition;
        [SerializeField] private Transform rightAttackPosition;

        private bool invulnerable;

        public void Awake() {
            UseBehaviour(new Idle(this));
            UseAnimation(new StateMachine());
        }

        public override void Hurt(float damage, bool stagger = true) {
            if (invulnerable) damage = 0;
            base.Hurt(damage);
        }

        // Returns the direction of the player relative to the enemy given that a detection raycast hit the player
        public Vector2 CheckPlayer() {
            RaycastHit2D[] hits = Enemy.Sweep(sprite.transform.position, Vector2.up, 180, playerDetectionDistance, 10, player)
                .Where(hit => Enemy.IsOnLayer(hit, player)).ToArray();

            if (hits.Length == 0) return Vector2.zero;
            return (hits[0].point - (Vector2)sprite.transform.position).normalized;
        }

        // Returns the Player if the player is in the crab's attack area collider, null if not.
        public Player GetPlayerWithinAttackArea() {
            Vector2 attackPosition = crabAttackArea.transform.position;
            Vector2 attackSize = crabAttackArea.size;

            Collider2D c = Physics2D.OverlapBox(attackPosition, attackSize, 0, player);
            if (c == null) return null;

            Player p = c.GetComponent<Player>();
            return p;
        }

        public void Attack(Player it) {
            float dir = Mathf.Sign(it.transform.position.x - transform.position.x);

            float radians = knockbackAngle * Mathf.Deg2Rad;
            Vector2 knockback = new Vector2(Mathf.Cos(radians), Mathf.Sin(radians)) * knockbackForce;

            knockback.x *= dir;

            it.Hurt(attack, knockback, 1);
        }

        public void SetInvulnerable(bool i) {
            invulnerable = i;
        }

        public void SetDirection(Vector2 dir) {
            direction = new Vector2(MathUtils.RoundX(dir.x), 0);
            sprite.flipX = dir.x < 0;
            crabAttackArea.transform.position = dir.x < 0 ? leftAttackPosition.position : dir.x > 0 ? rightAttackPosition.position : Vector3.zero;
        }

        protected override void UseAnimation(StateMachine stateMachine) {
            var idle = new IdleAnimation(animator);
            var center = new CenterPopOutAnimation(animator); // Temporarily unused
            var directional = new DirectionalPopOutAnimation(animator);
            var directionalHide = new DirectionalHideAnimation(animator);

            stateMachine.AddAnyTransition(idle, new FuncPredicate(() => behaviour is Idle));
            stateMachine.AddAnyTransition(directional, new FuncPredicate(() => behaviour is Alert));
            stateMachine.AddAnyTransition(directional, new FuncPredicate(() => behaviour is Attack)); // No attack animation implemented
            stateMachine.AddAnyTransition(directionalHide, new FuncPredicate(() => behaviour is Hide));

            stateMachine.SetState(idle);
            animationStateMachine = stateMachine;
        }


    }
}
