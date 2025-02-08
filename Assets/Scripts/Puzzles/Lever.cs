using Omnia.Utils;
using UnityEngine;

namespace Puzzle {
    public class Lever : MonoBehaviour, ISignal {
        [field: SerializeField] public SignalColor SignalColour { get; set; }
        [SerializeField] private float COUNTDOWN_TIME;
        [SerializeField] private Collider2D Collider2D;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private LayerMask playerLayer;
        #nullable enable

        public bool IsActive { get; set; }
        private CountdownTimer? leverUptime;
        public event ISignal.SignalFired? SignalEvent;

        void OnTriggerEnter2D(Collider2D other) {
            if (Collider2D.IsTouchingLayers(playerLayer) || Collider2D.IsTouchingLayers(enemyLayer)) {
                IsActive = true;
                SignalEvent?.Invoke(this);
                leverUptime = new CountdownTimer(COUNTDOWN_TIME);
            }
        }

        void OnTriggerExit2D(Collider2D other) {
            leverUptime?.Start();
        }

        void Update() {
            UpdateCountdownTick();
            Redraw();
        }

        private void UpdateCountdownTick() {
            if (leverUptime == null) return;

            leverUptime.Tick(Time.deltaTime);

            if (!leverUptime.IsRunning) {
                IsActive = false;
                SignalEvent?.Invoke(this);
            }
        }

        private void Redraw() {
            if (leverUptime != null) Debug.Log($"The current timer position I am at is ${1 - leverUptime.Progress}");
        }
    }

}
