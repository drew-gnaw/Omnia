using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle {
    public interface ISignal {
        public bool IsActive { get; }
        public SignalColor SignalColor { get; }
        public event SignalFired SignalEvent;
        public delegate void SignalFired(ISignal signal);
    }

    public abstract class SignalColor : Enum<SignalColor> {
        public abstract Color Color { get; }
        public abstract Sprite GetSymbol(PuzzleAssets assets);
        public static SignalColor Parse(Type type) => ParseFromType(type);
    }
    public class Red : SignalColor {
        public override Color Color => Color.red;

        public override Sprite GetSymbol(PuzzleAssets assets) => assets.xSymbol;
    }
    public class Green : SignalColor {
        public override Color Color => Color.green;

        public override Sprite GetSymbol(PuzzleAssets assets) => assets.triangleSymbol;
    }
    public class Blue : SignalColor {
        public override Color Color => Color.blue;

        public override Sprite GetSymbol(PuzzleAssets assets) => assets.squareSymbol;
    }
    public class Yellow : SignalColor {
        public override Color Color => Color.yellow;

        public override Sprite GetSymbol(PuzzleAssets assets) => assets.circleSymbol;
    }
    public class Black : SignalColor {
        public override Color Color => new Color(.5f,.5f,.5f);
        public override Sprite GetSymbol(PuzzleAssets assets) => assets.xSymbol;
    }
}
