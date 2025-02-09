using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Puzzle.ISignal;

namespace Puzzle {
    public class Button : MonoBehaviour, ISignal {
        [SerializeField]
        [TypeFilter(typeof(SignalColor))]
        private SerializableType signalColour;
        [SerializeField] BoxCollider2D boxCollider;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] Sprite downState;
        [SerializeField] Sprite upState;
#nullable enable
        public event SignalFired? SignalEvent;
        public bool IsActive { get; set; } = false;
        public SignalColor SignalColor { get => SignalColor.Parse(signalColour); }
        private int objectsInside = 0;

        void Start() {
            Redraw();
        }

        void OnTriggerEnter2D(Collider2D other) {
            objectsInside++;
            IsActive = true;
            Redraw();
            SignalEvent?.Invoke(this);
        }

        void OnTriggerExit2D(Collider2D other) {
            --objectsInside;
            if (objectsInside <= 0) {
                IsActive = false;
                Redraw();
                SignalEvent?.Invoke(this);
            }
        }

        private void Redraw() {
            spriteRenderer.sprite = (IsActive) ? downState : upState;
            spriteRenderer.color = SignalColor.Color;
        }
    }
}
