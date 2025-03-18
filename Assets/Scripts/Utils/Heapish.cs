using System;
using System.Collections.Generic;
using Unity.Collections;

namespace Utils {
    public class Heapish<T> {
        private readonly List<T> heap = new();
        private readonly Func<T, T, bool> cmp;
        public int Size => heap.Count;

        public Heapish(Func<T, T, bool> cmp) {
            this.cmp = cmp;
        }

        public void Push(T it) {
            heap.Add(it);
            HeapifyUp(Size - 1);
        }

        public T Pop() {
            var head = heap[0];

            if (Size > 1) {
                heap.RemoveAtSwapBack(0);
                HeapifyDown(0);
            } else {
                heap.RemoveAt(0);
            }

            return head;
        }

        private void HeapifyUp(int i) {
            while (i > 0) {
                var j = (i - 1) / 2;
                if (!cmp(heap[i], heap[j])) break;
                (heap[i], heap[j], i) = (heap[j], heap[i], j);
            }
        }

        private void HeapifyDown(int i) {
            while (true) {
                var l = 2 * i + 1;
                var r = 2 * i + 2;
                var j = i;
                if (l < Size && cmp(heap[l], heap[j])) j = l;
                if (r < Size && cmp(heap[r], heap[j])) j = r;
                if (j == i) break;
                (heap[i], heap[j], i) = (heap[j], heap[i], j);
            }
        }
    }
}
