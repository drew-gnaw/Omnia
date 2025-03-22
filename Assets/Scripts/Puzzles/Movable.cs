using NPC;
using Players;
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
        [SerializeField] private Transform startPosOverride;
        [SerializeField] private float moveSpeed = 10f;
        [SerializeField] private List<HingeJoint2D> boundToThis;
        [SerializeField] private LayerMask playerMask;
        public ReceiverBehaviour ReceiverBehaviour => ReceiverBehaviour.Parse(behaviour);
        private int positionIndex = 0;
        private List<ISignal> signalList = new();
        private Rigidbody2D rb;
        private List<Vector3> checkPointLocation = new();
        private Vector3 targetPosition;
        private bool shouldMove;

        private PlayerFeetBehaviour playerFeet;
        private bool isOnPlatform;


        void Awake() {
            signalList = signals?.Unbox() ?? new();
        }
        public float GetPlatformSpeed() {
            return moveSpeed;
        }

        public Vector2 GetDestination() {
            return targetPosition;
        }
        private void OnTriggerEnter2D(Collider2D collision) {
            if (collision.GetComponent<PlayerFeetBehaviour>() != null) {
                playerFeet = collision.GetComponent<PlayerFeetBehaviour>();
                isOnPlatform = true;
            }
        }

        private void OnTriggerExit2D(Collider2D collision) {
            if (collision.GetComponent<PlayerFeetBehaviour>() != null) {
                playerFeet = null;
                isOnPlatform = false;
            }
        }

        private void Start() {
            rb = GetComponent<Rigidbody2D>();
            checkPointLocation.Add(startPosOverride != null ? startPosOverride.position :  gameObject.transform.position);
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
                if (isOnPlatform && playerFeet != null) {
                    playerFeet.SetPlayerTransform(Vector2.MoveTowards(playerFeet.GetPlayerTransform(), targetPosition, Time.fixedDeltaTime * moveSpeed));
                }

                if (Vector2.Distance(rb.position, targetPosition) < 0.01f) {
                    getNextPosition();
                }

            }
        }

    }
}
