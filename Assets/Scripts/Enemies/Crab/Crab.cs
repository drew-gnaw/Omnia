using System.Linq;
using Enemies.Crab.Animation;
using Enemies.Crab.Behaviour;
using Omnia.State;
using Players;
using UnityEngine;
using Utils;

namespace Enemies.Crab {
    public class Crab : Enemy, HiddenEnemy {
        [SerializeField] internal SpriteRenderer sprite;
        [SerializeField] internal Animator animator;
        [SerializeField] internal Rigidbody2D rb;
        [SerializeField] internal LayerMask ground;
        [SerializeField] internal LayerMask player;
        [SerializeField] internal LayerMask bg;
        [SerializeField] internal Player targetInstance;

        [SerializeField] internal float detectionRange;
        [SerializeField] internal float windupTime;
        [SerializeField] internal float vulnerableTime;
        [SerializeField] internal float reloadTime;
        [SerializeField] internal float attackRadius;

        bool HiddenEnemy.hidden { get => behaviour is Hide || behaviour is Idle; set { } }

        public void Awake() {
            targetInstance ??= FindObjectsOfType<Player>().FirstOrDefault();
            UseBehaviour(new Idle(this));
        }

        public override void Update() {
            base.Update();
            sprite.flipX = targetInstance.rb.position.x < transform.position.x;
        }

        public void Attack(Player it) {
            var side = sprite.flipX ? -1 : 1;
            it.Hurt(attack, knockbackAngle * new Vector2(side * Mathf.Cos(knockbackAngle * Mathf.Deg2Rad), Mathf.Sin(knockbackAngle * Mathf.Deg2Rad)), 1);
        }

        public void SetLayer(LayerMask layer) {
            gameObject.layer = MathUtils.LayerIndexOf(layer);
        }

        public bool IsTargetDetected() => Sweep(sprite.transform.position, Vector2.up, 180, detectionRange, 17, ground | player).Any(hit => IsOnLayer(hit, player));

        protected override void UseAnimation(StateMachine stateMachine) {
            var idleAnim = new IdleAnimation(animator);
            stateMachine.AddAnyTransition(idleAnim, new FuncPredicate(() => behaviour is Idle));
            stateMachine.AddAnyTransition(new HideAnimation(animator), new FuncPredicate(() => behaviour is Hide));
            stateMachine.AddAnyTransition(new PopOutAnimation(animator), new FuncPredicate(() => behaviour is Alert or Pinch));

            stateMachine.SetState(idleAnim);
            animationStateMachine = stateMachine;
        }
    }
}
