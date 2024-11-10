using System.Collections;
using UnityEngine;

namespace Enemies.Charger
{
    public partial class Charger
    {
        private const float Speed = 50f;
        private const float Range = 2f;
        private const float WaitTime = 2f;

        private class PatrolState : IState
        {
            private readonly Charger charger;
            private float target;
            private Coroutine coroutine;

            public PatrolState(Charger charger)
            {
                this.charger = charger;
                target = charger.transform.position.x;
            }

            public void OnEnter()
            {
                coroutine = charger.StartCoroutine(HandlePatrol());
            }

            public void OnExit()
            {
                charger.StopCoroutine(coroutine);
            }

            public void Update()
            {
            }

            public void FixedUpdate()
            {
                var delta = target - charger.transform.position.x;
                var speed = Mathf.Abs(delta) < 0.1 ? 0 : Mathf.Sign(delta) * Speed * Time.fixedDeltaTime;

                charger.rb.velocity = new Vector2(speed, charger.rb.velocity.y);
            }

            private IEnumerator HandlePatrol()
            {
                while (true)
                {
                    yield return new WaitUntil(() => Mathf.Abs(target - charger.transform.position.x) < 0.1);
                    yield return new WaitForSeconds(WaitTime);

                    target = charger.territory.x + Random.Range(-1 * Range, Range);
                }
            }
        }
    }
}