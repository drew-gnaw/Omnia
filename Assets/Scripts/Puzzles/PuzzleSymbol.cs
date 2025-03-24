using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Puzzle {
    public class PuzzleSymbol : MonoBehaviour {
        [SerializeField] private SpriteRenderer symbolSpriteRenderer;
        [SerializeField] private SpriteRenderer mainSpriteRenderer;
        public void SetSymbol(Sprite sprite) => symbolSpriteRenderer.sprite = sprite;
        public void SetColor(Color c) => mainSpriteRenderer.color = c;
    }
}
