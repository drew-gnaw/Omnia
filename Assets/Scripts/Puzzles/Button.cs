using UnityEngine;
using static Puzzle.ISignal;
using Omnia.Utils;
using System.Collections.Generic;

namespace Puzzle {
    public class Button : MonoBehaviour, ISignal {
        [SerializeField]
        [TypeFilter(typeof(SignalColor))]
        private SerializableType signalColour;
        [SerializeField] BoxCollider2D boxCollider;
        [SerializeField] SpriteRenderer spriteRenderer;
        [SerializeField] SpriteRenderer symbolRenderer;
        [SerializeField] PuzzleAssets assets;
        [SerializeField] Sprite downState;
        [SerializeField] Sprite upState;
        [SerializeField] LayerMask excludedLayers;
#nullable enable
        public event SignalFired? SignalEvent;
        public bool IsActive { get; set; } = false;
        public SignalColor SignalColor { get => SignalColor.Parse(signalColour); }
        private int objectsInside = 0;

        void Start() {
            Redraw();
        }

        void OnTriggerEnter2D(Collider2D other) {
            Debug.Log("Other is colldiingn button" + other.gameObject.name);
            if (CollisionUtils.IsLayerInMask(other.gameObject.layer, excludedLayers)) {
                return;
            }
            objectsInside++;
            IsActive = true;
            Redraw();
            SignalEvent?.Invoke(this);
        }

        void OnTriggerExit2D(Collider2D other) {
            if (CollisionUtils.IsLayerInMask(other.gameObject.layer, excludedLayers)) {
                return;
            }
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
            symbolRenderer.gameObject.SetActive(!IsActive);
            symbolRenderer.sprite = SignalColor.GetSymbol(assets);
            symbolRenderer.gameObject.transform.rotation = Quaternion.identity;
        }
    }
}
