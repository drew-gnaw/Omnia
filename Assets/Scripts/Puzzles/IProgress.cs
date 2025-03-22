using Puzzle;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Puzzle {
    public interface IProgress {
        // A float from 0 to 1 representing the current progress of something.
        public float Progress { get; }
        public event ProgressFired ProgressEvent;
        public delegate void ProgressFired(IProgress progress);
    }
}
