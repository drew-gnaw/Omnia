using Omnia.State;
using UnityEngine;

namespace Enemies.Charger
{
    public partial class Charger : EnemyBase
    {
        [SerializeField] private SpriteRenderer sprite;
        [SerializeField] private Animator animator;
        [SerializeField] private Rigidbody2D rb;

        private BehaviourState behaviourState;
        private Vector3 territory;

        private StateMachine behaviourStateMachine;
        private StateMachine animationStateMachine;

        public void Awake()
        {
            behaviourState = BehaviourState.Patrol;
            territory = transform.position;

            behaviourStateMachine = ApplyBehaviourStates(new StateMachine());
            animationStateMachine = ApplyAnimationStates(new StateMachine());
        }

        public void Update()
        {
            behaviourStateMachine.Update();
            animationStateMachine.Update();
        }

        public void FixedUpdate()
        {
            behaviourStateMachine.FixedUpdate();
            animationStateMachine.FixedUpdate();
        }

        private enum BehaviourState
        {
            Patrol,
            Charge,
            Attack,
        }

        private StateMachine ApplyBehaviourStates(StateMachine stateMachine)
        {
            var patrolState = new PatrolState(this);
            // var chargeState = new ChargeState(this);
            // var attackState = new AttackState(this);

            var patrolCondition = new FuncPredicate(() => behaviourState == BehaviourState.Patrol);
            var chargeCondition = new FuncPredicate(() => behaviourState == BehaviourState.Charge);
            var attackCondition = new FuncPredicate(() => behaviourState == BehaviourState.Attack);

            stateMachine.AddAnyTransition(patrolState, patrolCondition);
            stateMachine.SetState(patrolState);

            return stateMachine;
        }

        private StateMachine ApplyAnimationStates(StateMachine stateMachine)
        {
            var idleState = new IdleAnimation(this);
            var walkState = new WalkAnimation(this);

            var idleCondition = new FuncPredicate(() => Mathf.Abs(rb.velocity.x) < 0.1);
            var walkCondition = new FuncPredicate(() => Mathf.Abs(rb.velocity.x) > 0.1);

            stateMachine.AddTransition(idleState, walkState, walkCondition);
            stateMachine.AddTransition(walkState, idleState, idleCondition);
            stateMachine.SetState(idleState);

            return stateMachine;
        }
    }
}