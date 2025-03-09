using Enemies.BirdSpawner.Animation;
using Enemies.BirdSpawner.Behaviour;
using Omnia.State;
using UnityEngine;
using Utils;
using StateMachine = Omnia.State.StateMachine;

namespace Enemies.BirdSpawner {
    public class BirdSpawner : Enemy {
        [SerializeField] internal float cooldown;
        [SerializeField] internal int spawns;

        [SerializeField] internal SpriteRenderer sprite;
        [SerializeField] internal Animator animator;
        [SerializeField] internal LayerMask bg;
        [SerializeField] internal GameObject bird;

        [SerializeField] internal string debugBehaviour;

        public void Awake() {
            UseBehaviour(new Idle(this));
        }

        public override void UseBehaviour(IBehaviour it) {
            base.UseBehaviour(it);
            debugBehaviour = it.GetType().Name;
        }

        public override void Hurt(float damage) {
            if (currentHealth == 0) return;
            currentHealth = Mathf.Clamp(currentHealth - damage, 0, maximumHealth);

            /* Become inactive on "death" without being destroyed. */
            if (currentHealth == 0) UseBehaviour(new Broken(this));
        }

        public void SetLayer(LayerMask it) {
            gameObject.layer = MathUtils.LayerIndexOf(it);
        }

        public void SpawnBird() {
            var instance = Instantiate(bird, sprite.transform.position, Quaternion.identity).GetComponent<Bird.Bird>().Of(() => spawns++);
            spawns--;
            instance.rb.velocity = Random.insideUnitCircle.normalized * instance.speed;
        }

        protected override void UseAnimation(StateMachine stateMachine) {
            return;
            var idle = new IdleAnimation(animator);
            var barf = new BarfAnimation(animator);
            var dead = new DeadAnimation(animator);

            stateMachine.AddAnyTransition(idle, new FuncPredicate(() => behaviour is Idle));
            stateMachine.AddAnyTransition(barf, new FuncPredicate(() => behaviour is Attack));
            stateMachine.AddAnyTransition(dead, new FuncPredicate(() => behaviour is Broken));

            stateMachine.SetState(idle);
            animationStateMachine = stateMachine;
        }
    }
}
