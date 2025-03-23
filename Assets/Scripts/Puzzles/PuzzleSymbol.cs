using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace Puzzle {
    public class PuzzleSymbol : MonoBehaviour {
        [SerializeField] private SpriteRenderer symbolSpriteRenderer;
        public void SetSymbol(Sprite sprite) => symbolSpriteRenderer.sprite = sprite;
    }
}
