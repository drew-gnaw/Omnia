using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Puzzle {
    public class Movabale : MonoBehaviour, IReceiver {
        [TypeFilter(typeof(ReceiverBehaviour))]
        [SerializeField] private SerializableType behaviour;
        [SerializeField] private List<InterfaceReference<ISignal>> signals;
        [SerializeField] private List<Vector2> positions;
        [SerializeField] private float moveSpeed = 10f;
        public ReceiverBehaviour ReceiverBehaviour => ReceiverBehaviour.Parse(behaviour);
        private int positionIndex = 0;
        private List<ISignal> signalList = new();
        private Rigidbody2D rb;
        private Vector2 targetPosition;
        private Coroutine movingCoroutine;
        
        void Awake() {
            signalList = signals?.Unbox() ?? new();
        }

        private void Start() {
            rb = GetComponent<Rigidbody2D>();
            targetPosition = positions.First();
        }

        private void OnEnable() {
            foreach (ISignal signal in signalList) {
                signal.SignalEvent += SignalReceived;
            }
        }

        private void OnDisable() {
            foreach (ISignal signal in signalList) {
                signal.SignalEvent -= SignalReceived;
            }
        }

        private void SignalReceived(ISignal signal) {
            Debug.Log("Signal received: " + ReceiverBehaviour.Accept(signalList));
            bool move = ReceiverBehaviour.Accept(signalList);
            if (movingCoroutine != null) {
                StopCoroutine(movingCoroutine);
            }

            if (move) {
                movingCoroutine = StartCoroutine(Move());
            }
        }

        private void getNextPosition() {
            positionIndex = (positionIndex + 1) % positions.Count;
            targetPosition = positions[positionIndex];
        }

        private IEnumerator Move() {
            Debug.Log("move");
            rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime));
            yield return null;
            if (rb.position == targetPosition) {
                getNextPosition();
            }
            movingCoroutine = StartCoroutine(Move());
        }
    }
}
