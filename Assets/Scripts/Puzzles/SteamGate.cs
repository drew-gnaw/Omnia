using System.Collections.Generic;
using System.Linq;
using Enemies;
using Omnia.Utils;
using Players;
using Puzzle;
using UnityEngine;

namespace Puzzles {
    public class SteamGate : MonoBehaviour, IReceiver {
        [SerializeField] internal Animator animator;
        [SerializeField] internal bool isReverse;
        [SerializeField] internal LayerMask target;
        [SerializeField] internal LayerMask ground;

        [TypeFilter(typeof(ReceiverBehaviour))] //
        [SerializeField]
        private SerializableType behaviour;

        [SerializeField] private List<InterfaceReference<ISignal>> signals;
        [SerializeField] private PuzzleSymbol[] pipes;
        [SerializeField] private PuzzleAssets symbols;

        private List<ISignal> dereferencedSignals;
        private bool powered;
        private bool IsActivated => isReverse ? !powered : powered;

        public ReceiverBehaviour ReceiverBehaviour => ReceiverBehaviour.Parse(behaviour);

        public void Awake() {
            dereferencedSignals = signals?.Unbox() ?? new List<ISignal>();
        }

        public void Start() {
            Redraw();
            animator.Play(IsActivated ? "SteamGateActivated" : "SteamGateDeactivated");
        }

        public void OnTriggerEnter2D(Collider2D other) {
            if (!IsActivated || !CollisionUtils.IsLayerInMask(other.gameObject.layer, target)) return;
            var direction = other.transform.position - transform.position;
            var fail = Physics2D.Raycast(transform.position, direction, direction.magnitude, ground);
            if (fail) return;
            if (other.TryGetComponent<Enemy>(out var enemy)) enemy.Die();
            if (!other.TryGetComponent<Player>(out var it)) return;
            it.Hurt(it.CurrentHealth - float.Epsilon, Vector2.up * it.jumpSpeed, 5);
            it.Die();
        }

        private void OnEnable() {
            foreach (var signal in dereferencedSignals) {
                signal.SignalEvent += OnSignal;
            }
        }

        private void OnDisable() {
            foreach (var signal in dereferencedSignals) {
                signal.SignalEvent -= OnSignal;
            }
        }

        private void OnSignal(ISignal signal) {
            var it = ReceiverBehaviour.Accept(dereferencedSignals);
            if (it == powered) return;
            powered = it;
            animator.Play(IsActivated ? "SteamGateActivating" : "SteamGateDeactivating");
        }

        private void Redraw() {
            if (dereferencedSignals.Count == 0) return;

            foreach (var pipe in pipes) {
                pipe.SetColor(dereferencedSignals.First().SignalColor.Color);
                pipe.SetSymbol(dereferencedSignals.First().SignalColor.GetSymbol(symbols));
            }
        }
    }
}
