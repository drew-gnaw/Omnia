using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle {
    public class Gate : MonoBehaviour, IReceiver {
        [SerializeField] private List<InterfaceReference<ISignal>> signals;
        [TypeFilter(typeof(ReceiverBehaviour))]
        [SerializeField] private SerializableType behaviour;

        public ReceiverBehaviour ReceiverBehaviour => ReceiverBehaviour.Parse(behaviour);

        [SerializeField] private float moveDistance = 2.0f; // How far the gate moves
        [SerializeField] private float moveDuration = 1.5f; // Time to slide fully
        [SerializeField] private AnimationCurve easeCurve;  // Optional smoothing

        private Vector3 startPos;
        private Vector3 targetPos;
        private bool isSliding = false;
        private bool desiredOpenState = false; // The final state the gate should reach
        public enum GateType { Horizontal, Vertical }
        [SerializeField] private GateType gateType = GateType.Horizontal;

        private void Start() {
            startPos = transform.position;
            Vector3 direction = (gateType == GateType.Horizontal) ? Vector3.right : Vector3.up;
            targetPos = startPos + direction * moveDistance;
        }

        private void OnEnable() {
            if (signals != null) {
                foreach (ISignal signal in signals.Unbox()) {
                    signal.SignalEvent += SignalReceived;
                }
            }
        }

        private void OnDisable() {
            if (signals != null) {
                foreach (ISignal signal in signals.Unbox()) {
                    signal.SignalEvent -= SignalReceived;
                }
            }
        }

        private void SignalReceived(ISignal signal) {
            // Get the latest state that the gate *should* be in
            bool newState = ReceiverBehaviour.Accept(signals.Unbox());

            // If a new state is received while sliding, we store it but don't interrupt
            desiredOpenState = newState;

            // If not sliding, start moving; otherwise, let the current slide finish
            if (!isSliding) {
                StartCoroutine(SlideToState(desiredOpenState));
            }
        }

        private IEnumerator SlideToState(bool open) {
            isSliding = true;
            Vector3 initialPos = transform.position;
            Vector3 finalPos = open ? targetPos : startPos;

            float timer = 0f;
            while (timer < moveDuration) {
                timer += Time.deltaTime;
                float t = timer / moveDuration;
                float easedT = easeCurve.Evaluate(t); // Use easing curve

                transform.position = Vector3.Lerp(initialPos, finalPos, easedT);
                yield return null;
            }

            transform.position = finalPos;
            isSliding = false;

            // After completing movement, check if the state is still correct
            if (desiredOpenState != open) {
                StartCoroutine(SlideToState(desiredOpenState)); // Re-slide if needed
            }
        }
    }

}

