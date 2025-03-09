using System;
using Enemies.Bird.Animation;
using Enemies.Bird.Behaviour;
using Omnia.State;
using UnityEngine;
using StateMachine = Omnia.State.StateMachine;

namespace Enemies.Bird {
    public class Bird : Enemy {
        [SerializeField] internal float detectionRadius;
        [SerializeField] internal float fuse;
        [SerializeField] internal float triggerDistance;
        [SerializeField] internal float explosionRadius;

        [SerializeField] internal float delay;
        [SerializeField] internal float speed;

        [SerializeField] internal SpriteRenderer sprite;
        [SerializeField] internal Animator animator;
        [SerializeField] internal Rigidbody2D rb;
        [SerializeField] internal LayerMask ground;
        [SerializeField] internal LayerMask player;
        [SerializeField] internal Vector2 facing = Vector2.zero;

        [SerializeField] internal string debugBehaviour;

        private Action action;

        public Bird Of(Action it) {
            action = it;
            return this;
        }

        public void Awake() {
            UseBehaviour(new Idle(this));
        }

        public void OnDestroy() {
            action();
        }

        public override void UseBehaviour(IBehaviour it) {
            base.UseBehaviour(it);
            debugBehaviour = it.GetType().Name;
        }

        protected override void UseAnimation(StateMachine stateMachine) {
            return;
            var idle = new IdleAnimation(animator);
            var dive = new DiveAnimation(animator);
            var bomb = new BombAnimation(animator);

            stateMachine.AddAnyTransition(idle, new FuncPredicate(() => behaviour is Idle));
            stateMachine.AddAnyTransition(dive, new FuncPredicate(() => behaviour is Dive));
            stateMachine.AddAnyTransition(bomb, new FuncPredicate(() => behaviour is Bomb));

            stateMachine.SetState(idle);
            animationStateMachine = stateMachine;
        }
    }
}
