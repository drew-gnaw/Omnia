using Omnia.Utils;
using System;
using UnityEngine;

namespace Puzzle {
    public class Lever : MonoBehaviour, ISignal {
        [SerializeField]
        [TypeFilter(typeof(SignalColor))]
        private SerializableType signalColor;
        [SerializeField] private float maxChargeDuration; // Max charge time in seconds
        [SerializeField] private Collider2D Collider2D;
        [SerializeField] private LayerMask enemyLayer;
        [SerializeField] private LayerMask playerLayer;
        [SerializeField] private SpriteRenderer handle;
        [SerializeField] private SpriteRenderer baseRenderer;
        [SerializeField] private Sprite ticks;
        [SerializeField] private Transform pivotPoint; 
#nullable enable
        public event ISignal.SignalFired? SignalEvent;

        public bool IsActive { get; set; }
        public SignalColor SignalColor => SignalColor.Parse(signalColor);
        private CountdownTimer leverUptime = new(0);
        private int objectsInside = 0;
        private readonly float chargeRate = 180f; //(degrees per second)
        private readonly float radius = 0.2f; // Fixed distance from pivot
        private float startAngle = 150f;
        private float endAngle = 30f;
        private float currentHandleAngle;
        private bool isCharging = false;

        void Start() {
            // Account for parent rotation
            //startAngle = startAngle + transform.rotation.z;
            //endAngle = endAngle + transform.rotation.z;
            currentHandleAngle = startAngle;
            Debug.Log($"The Current start ${startAngle} end {endAngle} z angle {transform.localRotation}");
            Draw();
        }

        void Update() {
            UpdateCountdownTick();
            AnimateHandle();
        }

        void OnTriggerEnter2D(Collider2D other) {
            if (Collider2D.IsTouchingLayers(playerLayer) || Collider2D.IsTouchingLayers(enemyLayer)) {
                objectsInside++;
                isCharging = true;
                IsActive = true;
                SignalEvent?.Invoke(this);
                leverUptime.Stop();
            }
        }

        void OnTriggerExit2D(Collider2D other) {
            objectsInside--;

            if (objectsInside <= 0) {
                isCharging = false;
                float chargeRatio = Mathf.InverseLerp(endAngle, startAngle, currentHandleAngle);
                leverUptime = new CountdownTimer(chargeRatio * maxChargeDuration);
                leverUptime.Start();
            }
        }

        private void Draw() {
            handle.color = SignalColor.Color;
            baseRenderer.color = SignalColor.Color;
        }

        private void UpdateCountdownTick() {
            if (isCharging) return; 

            leverUptime.Tick(Time.deltaTime);

            if (!leverUptime.IsRunning) {
                IsActive = false;
                SignalEvent?.Invoke(this);
            }
        }

        private void AnimateHandle() {
            if (isCharging) {
                currentHandleAngle = Mathf.MoveTowards(currentHandleAngle, startAngle, chargeRate * Time.deltaTime);
            } else {
                currentHandleAngle = Mathf.Lerp(startAngle, endAngle, (1 - leverUptime.TimeRemaining / maxChargeDuration));
            }

            UpdateHandlePosition();
        }

        private void UpdateHandlePosition() {
            float angleRad = currentHandleAngle * Mathf.Deg2Rad;

            // Calculate new position relative to the pivot
            Vector2 pivotPos = pivotPoint.localPosition;
            Vector2 offset = new Vector2(Mathf.Cos(angleRad), Mathf.Sin(angleRad)) * radius;
            handle.transform.localPosition = pivotPos + offset;

            // Rotate the handle to always face the pivot
            Vector2 directionToPivot = (pivotPos - (Vector2)handle.transform.localPosition).normalized;
            float handleAngle = Mathf.Atan2(directionToPivot.y, directionToPivot.x) * Mathf.Rad2Deg;
            handle.transform.localRotation = Quaternion.Euler(0, 0, handleAngle + 90f); // +90 to align properly
        }
    }
}
