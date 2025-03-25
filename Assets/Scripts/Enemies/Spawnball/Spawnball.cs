using System;
using System.Linq;
using Enemies.Spawnball.Animation;
using Enemies.Spawnball.Behaviour;
using Omnia.State;
using Players;
using UnityEngine;
using StateMachine = Omnia.State.StateMachine;

namespace Enemies.Spawnball {
    public class Spawnball : Enemy {
        [SerializeField] internal float triggerDistance;
        [SerializeField] internal float delay;
        [SerializeField] internal float speed;
        [SerializeField] internal float lifespan;
        [SerializeField] internal float airAcceleration;

        [SerializeField] internal SpriteRenderer sprite;
        [SerializeField] internal Animator animator;
        [SerializeField] internal Rigidbody2D rb;

        [SerializeField] internal Transform target;
        [SerializeField] internal GameObject spawn;
        [SerializeField] internal GameObject explosion;

        public Action<Spawnball> NotifyOnDestroy;

        public void OnDestroy() => NotifyOnDestroy?.Invoke(this);

        public void Awake() {
            UseBehaviour(new Move(this));
        }

        public override void Update() {
            base.Update();
            sprite.transform.rotation = Quaternion.Euler(0, 0, Mathf.Atan2(rb.velocity.y, rb.velocity.x) * Mathf.Rad2Deg);
        }

        public void OnSpawnEnemy() {
            Instantiate(spawn, target.position, Quaternion.identity);
            Destroy(gameObject);

            Instantiate(explosion, rb.worldCenterOfMass, Quaternion.identity);
        }

        protected override void UseAnimation(StateMachine stateMachine) {
            var moveAnim = new MoveAnimation(animator);
            stateMachine.AddAnyTransition(moveAnim, new FuncPredicate(() => behaviour is Move));
            stateMachine.AddAnyTransition(new ActivateAnimation(animator), new FuncPredicate(() => behaviour is Activate));

            stateMachine.SetState(moveAnim);
            animationStateMachine = stateMachine;
        }
    }
}
