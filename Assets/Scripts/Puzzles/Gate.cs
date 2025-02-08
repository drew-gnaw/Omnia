using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle {
    public class Gate : MonoBehaviour, IReceiver {
        [TypeFilter(typeof(ReceiverBehaviour))]
        [SerializeField] private SerializableType behaviour;
        [SerializeField] private GateType gateType = GateType.Horizontal;
        [SerializeField] private List<InterfaceReference<ISignal>> signals;
        [SerializeField] private List<SpriteRenderer> gears;
        [SerializeField] private float moveDistance = 3.0f;
        [SerializeField] private float moveDuration = 0.5f;
        [SerializeField] private AnimationCurve easeCurve;
        public ReceiverBehaviour ReceiverBehaviour => ReceiverBehaviour.Parse(behaviour);

        private Vector3 startPos;
        private Vector3 targetPos;
        private bool isSliding = false;
        private bool desiredOpenState = false; // The final state the gate should reach

        public enum GateType { Horizontal, Vertical }

        private void Start() {
            startPos = transform.position;
            Vector3 direction = (gateType == GateType.Horizontal) ? Vector3.right : Vector3.up;
            targetPos = startPos + direction * moveDistance;
            Redraw();
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

        private void Redraw() {
            for (int i = 0; i < gears.Count; i++) {
                if (i < signals.Count) {
                    gears[i].color = signals.Unbox()[i].SignalColour.Color;
                }
            }
        }

        private void SignalReceived(ISignal signal) {
            bool newState = ReceiverBehaviour.Accept(signals.Unbox());
            if (newState == desiredOpenState) return;

            desiredOpenState = newState;

            if (!isSliding) {
                StartCoroutine(SlideToState(desiredOpenState));
            }
        }

        private IEnumerator SlideToState(bool open) {
            isSliding = true;
            Vector3 initialPos = transform.position;
            Vector3 finalPos = open ? targetPos : startPos;
            float rotationSpeed = 10f; 
            float timer = 0f;
            bool movingRight = open;

            while (timer < moveDuration) {
                timer += Time.deltaTime;
                float t = timer / moveDuration;
                float easedT = easeCurve.Evaluate(t);

                transform.position = Vector3.Lerp(initialPos, finalPos, easedT);
                RotateGears(movingRight, easedT * rotationSpeed);
                yield return null;
            }

            transform.position = finalPos;
            isSliding = false;

            if (desiredOpenState != open) {
                StartCoroutine(SlideToState(desiredOpenState));
            }
        }

        private void RotateGears(bool movingRight, float rotationAmount) {
            for (int i = 0; i < gears.Count; i++) {
                bool evenIndex = i % 2 == 0;
                float directionMultiplier = evenIndex ? 1f : -1f; 

                // Even indexed gear follows gate movement direction
                float gearRotation = movingRight ? rotationAmount : -rotationAmount;


                gears[i].transform.Rotate(0, 0, gearRotation * directionMultiplier);
            }
        }
    }
}
