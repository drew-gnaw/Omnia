using System;
using System.Collections.Generic;

namespace Puzzle {
    public interface IReceiver {
        public ReceiverBehaviour ReceiverBehaviour { get; }
    }

    public abstract class ReceiverBehaviour : Enum<ReceiverBehaviour> {
        public abstract bool Accept(List<ISignal> signals);

        public class And : ReceiverBehaviour { public override bool Accept(List<ISignal> signals) => signals.TrueForAll(it => it.IsActive); }
        public class Or : ReceiverBehaviour { public override bool Accept(List<ISignal> signals) => signals.Exists(it => it.IsActive); }

        public static ReceiverBehaviour Get<T>() where T : ReceiverBehaviour => ParseFromType(typeof(T));
        public static ReceiverBehaviour Parse(Type type) => ParseFromType(type);
        public static readonly IEnumerable<ReceiverBehaviour> ReceiverValues = Values;
    }
}
