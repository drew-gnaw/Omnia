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
        [SerializeField] internal Animator animator;
        [SerializeField] internal Rigidbody2D rb;
        [SerializeField] internal LayerMask ground;
        [SerializeField] internal LayerMask player;
        [SerializeField] internal LayerMask bg;
        [SerializeField] internal Vector2 facing = Vector2.zero;

        [SerializeField] internal float detectionRange;
        [SerializeField] internal float windupTime;
        [SerializeField] internal float vulnerableTime;
        [SerializeField] internal float reloadTime;
        [SerializeField] internal float attackRadius;
        [SerializeField] internal Transform[] attackPositions;

        public void Awake() {
            UseBehaviour(new Idle(this));
        }

        public Vector2 IsTargetDetectedWithDirection() {
            var hit = Sweep(sprite.transform.position, Vector2.up, 180, detectionRange, 17, ground | player).FirstOrDefault(hit => IsOnLayer(hit, player));
            if (!hit) return default;
            return hit.transform.position.x > transform.position.x ? Vector2.right : Vector2.left;
        }

        public void SetLayer(LayerMask layer) {
            gameObject.layer = MathUtils.LayerIndexOf(layer);
        }

        public void Attack(Player it) {
            var direction = new Vector2(facing.x * Mathf.Cos(knockbackAngle * Mathf.Deg2Rad), Mathf.Sin(knockbackAngle * Mathf.Deg2Rad));
            it.Hurt(attack, direction.normalized * knockbackAngle, 1);
        }

        protected override void UseAnimation(StateMachine stateMachine) {
            var idleAnim = new IdleAnimation(animator);
            stateMachine.AddAnyTransition(idleAnim, new FuncPredicate(() => behaviour is Idle));
            stateMachine.AddAnyTransition(new HideLAnimation(animator), new FuncPredicate(() => behaviour is Hide && facing.x < 0));
            stateMachine.AddAnyTransition(new HideRAnimation(animator), new FuncPredicate(() => behaviour is Hide && facing.x > 0));
            stateMachine.AddAnyTransition(new PopOutLAnimation(animator), new FuncPredicate(() => behaviour is Alert or Pinch && facing.x < 0));
            stateMachine.AddAnyTransition(new PopOutRAnimation(animator), new FuncPredicate(() => behaviour is Alert or Pinch && facing.x > 0));

            stateMachine.SetState(idleAnim);
            animationStateMachine = stateMachine;
        }
    }
}
