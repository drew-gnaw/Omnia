using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Enemies.Common;
using Players;
using UnityEngine;
using Utils;

namespace UI {
    public class GameplayAssistance : PersistentSingleton<GameplayAssistance> {
        [SerializeField] internal GameObject pathHint;
        [SerializeField] internal Player player;

        [SerializeField] internal int skip;
        [SerializeField] internal int take;
        [SerializeField] internal float step;
        [SerializeField] internal float interval;
        [SerializeField] internal bool repeat = true;
        [SerializeField] internal Transform debugTransform;

        private Coroutine co;

        public void Start() {
            player = GameObject.FindGameObjectWithTag("Player")?.GetComponent<Player>();
            if (debugTransform) SetTarget(debugTransform);
        }

        private IEnumerator DoPathHint(Transform target) {
            do {
                foreach (var next in Pathfinder.FindPath(player.rb.worldCenterOfMass, target.position).Where((_, i) => i % skip == skip - 1).Take(take)) {
                    Instantiate(pathHint, next, Quaternion.Euler(0, 0, Random.Range(0, 360)));
                    yield return new WaitForSeconds(step);
                }

                yield return new WaitForSeconds(interval);
            }
            while (repeat);
        }

        private void SetTarget(Transform target) {
            if (co != null) StopCoroutine(co);
            if (target) co = StartCoroutine(DoPathHint(target));
        }

        public static void SetPathHintTarget(Transform target) => Instance.SetTarget(target);
    }
}
