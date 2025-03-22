using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Puzzle {
    public class SteamGate : MonoBehaviour, IReceiver {
        [TypeFilter(typeof(ReceiverBehaviour))]
        [SerializeField] private SerializableType behaviour;
        [SerializeField] private List<InterfaceReference<ISignal>> signals;
        [SerializeField] private List<SpriteRenderer> gears;
        [SerializeField] private SpriteRenderer steamEffect;
        [SerializeField] private BoxCollider2D hazardCollider;
        [SerializeField] private AnimationCurve easeCurve;

        [SerializeField] private bool opposite;
        private List<ISignal> signalList = new();

        public ReceiverBehaviour ReceiverBehaviour => ReceiverBehaviour.Parse(behaviour);

        void Awake() {
            signalList = signals?.Unbox() ?? new();
        }

        private void Start() {
            steamEffect.enabled = !opposite;
            hazardCollider.enabled = !opposite;
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
            bool newState = ReceiverBehaviour.Parse(behaviour).Accept(signalList);
            Debug.Log("my name is " + gameObject.name + " and i will transition into " + newState + " state");

            ToggleSteam(newState);
        }

        private void ToggleSteam(bool activate) {
            steamEffect.enabled = activate != opposite;
            hazardCollider.enabled = activate != opposite;
        }
    }
}
