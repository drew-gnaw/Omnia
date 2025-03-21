using NPC;
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
        [SerializeField] private Transform destPosition;
        [SerializeField] private float moveSpeed = 10f;
        public ReceiverBehaviour ReceiverBehaviour => ReceiverBehaviour.Parse(behaviour);
        private int positionIndex = 0;
        private List<ISignal> signalList = new();
        private Rigidbody2D rb;
        private List<Vector3> checkPointLocation = new();
        private Vector3 targetPosition;
        private bool shouldMove;


        void Awake() {
            signalList = signals?.Unbox() ?? new();
        }

        private void Start() {
            rb = GetComponent<Rigidbody2D>();
            checkPointLocation.Add(gameObject.transform.position);
            checkPointLocation.Add(destPosition.position);
            targetPosition = checkPointLocation.First();
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
            shouldMove = ReceiverBehaviour.Accept(signalList);
        }

        private void getNextPosition() {
            positionIndex = (positionIndex + 1) % checkPointLocation.Count;
            targetPosition = checkPointLocation[positionIndex];
        }
        private void FixedUpdate() {
            if (shouldMove) {
                rb.MovePosition(Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.fixedDeltaTime));

                if (Vector2.Distance(rb.position, targetPosition) < 0.01f) {
                    getNextPosition();
                }
            }
        }

    }
}
