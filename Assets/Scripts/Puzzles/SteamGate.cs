using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Puzzle {
    public class SteamGate : MonoBehaviour {
        [SerializeField] private List<PuzzleSymbol> pipes;
        [SerializeField] private Steam steamEffect;
        [SerializeField] private Gate gate;
        [SerializeField] private PuzzleAssets symbols;

        private void Start() {
            Redraw();
        }

        private void Redraw() {
            if (gate.SignalList.Count == 0) return;

            foreach (var pipe in pipes) {
                pipe.SetColor(gate.SignalList.First().SignalColor.Color);
                pipe.SetSymbol(gate.SignalList.First().SignalColor.GetSymbol(symbols));
            }
        }
    }
}
