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
        public override Color Color => new Color(218f / 255f, 66f / 255f, 66f / 255f);

        public override Sprite GetSymbol(PuzzleAssets assets) => assets.xSymbol;
    }
    public class Green : SignalColor {
        public override Color Color => new Color(93f / 255f, 166f / 255f, 69f / 255f);

        public override Sprite GetSymbol(PuzzleAssets assets) => assets.triangleSymbol;
    }
    public class Blue : SignalColor {
        public override Color Color => new Color(58f / 255f, 139f / 255f, 144f / 255f);

        public override Sprite GetSymbol(PuzzleAssets assets) => assets.squareSymbol;
    }
    public class Yellow : SignalColor {
        public override Color Color => new Color(237f / 255f, 193f / 255f, 70f / 255f);

        public override Sprite GetSymbol(PuzzleAssets assets) => assets.xSymbol;
    }
    public class Black : SignalColor {
        public override Color Color => new Color(.4f,.4f,.4f);
        public override Sprite GetSymbol(PuzzleAssets assets) => assets.xSymbol;
    }
}
