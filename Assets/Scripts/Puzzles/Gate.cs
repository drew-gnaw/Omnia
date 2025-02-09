using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
            List<float> gearInitial = CalculateFinalPosition(false);
            for (int i = 0; i < gears.Count; i++) {
                gears[i].transform.rotation = Quaternion.Euler(0, 0, gearInitial[i]);
            }
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
                    gears[i].color = signals.Unbox()[i].SignalColor.Color;
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

            List<float> initialRotations = gears.Select(it => it.transform.rotation.z).ToList();
            List<float> finalRotations = CalculateFinalPosition(open);
            float timer = 0f;
            bool movingRight = open;

            while (timer < moveDuration) {
                timer += Time.deltaTime;
                float t = timer / moveDuration;
                float easedT = easeCurve.Evaluate(t);

                transform.position = Vector3.Lerp(initialPos, finalPos, easedT);
                for (int i = 0; i < gears.Count; i++) {
                    float angle = Mathf.Lerp(initialRotations[i], finalRotations[i], easedT);
                    gears[i].transform.rotation = Quaternion.Euler(0, 0, angle);
                }
                yield return null;
            }

            transform.position = finalPos;
            isSliding = false;

            if (desiredOpenState != open) {
                StartCoroutine(SlideToState(desiredOpenState));
            }
        }

        private List<float> CalculateFinalPosition(bool movingRight) {
            float rotationAmount = 180f; // Between [0, 180] (no support for more than 360 degree turn for now)
            List<float> rotations = new();
            for (int i = 0; i < gears.Count; i++) {
                float directionMultiplier = (i % 2 == 0) ? 1f : -1f;

                // Even indexed gear follows gate movement direction
                float gearRotation = movingRight ? rotationAmount : -rotationAmount;
                rotations.Add(gearRotation * directionMultiplier);
            }
            return rotations;
        }
    }
}
