using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle {
    public interface ISignal {
        public delegate void SignalFired(ISignal signal);
        public event SignalFired SignalEvent;
        public bool IsActive { get; }
        public SignalColor SignalColour { get; }

    }
    public abstract class SignalColor : Enum<SignalColor> {
        public abstract Color Color { get; }
        public class Red : SignalColor { public override Color Color => Color.red; }
        public class Green : SignalColor { public override Color Color => Color.green; }
        public class Blue : SignalColor { public override Color Color => Color.blue; }
        public static SignalColor Parse(Type type) => ParseFromType(type);
    }

    public interface IReceiver {
        public ReceiverBehaviour ReceiverBehaviour { get; }
    }

    public abstract class ReceiverBehaviour : Enum<ReceiverBehaviour> {
        public abstract bool Accept(List<ISignal> signals);

        public class And : ReceiverBehaviour { public override bool Accept(List<ISignal> signals) => signals.TrueForAll(it => it.IsActive); }
        public class Or : ReceiverBehaviour { public override bool Accept(List<ISignal> signals) => signals.Exists(it => it.IsActive); }

        
        public static readonly IEnumerable<ReceiverBehaviour> ReceiverValues = Values;
        public static ReceiverBehaviour Parse(Type type) => ParseFromType(type);
        public static ReceiverBehaviour Get<T>() where T: ReceiverBehaviour => ParseFromType(typeof(T));
    }

    
}
