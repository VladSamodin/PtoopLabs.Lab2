using System;
using System.Collections.Generic;

namespace Task2.CustomQueues
{
    public class ArrayQueue<T> : IEnumerable<T>
    {
        private const int MinCapacity = 10;
        private T[] items;
        private int head;
        private int tail;

        public int Count
        {
            get
            {
                if (this.Empty)
                {
                    return 0;
                }
                if (head <= tail)
                {
                    return tail - head + 1;
                }
                return items.Length - (head - tail - 1);
            }
        }

        public int Capacity => items.Length;

        public bool Empty => head == -1;

        public ArrayQueue(int initialCapacity = 10)
        {
            if (initialCapacity <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(initialCapacity), "Capacity must be > 0");
            }
            if (initialCapacity < MinCapacity)
            {
                initialCapacity = MinCapacity;
            }
            items = new T[initialCapacity];
            head = -1;
            tail = -1;
        }


        public void Enqueue(T toAdd)
        {
            if (this.Empty)
            {
                head = 0;
            }
            if ((tail + 1) % items.Length == head)
            {
                GrowArray();
            }
            tail = (tail + 1) % items.Length;
            items[tail] = toAdd;
        }

        public T Dequeue()
        {
            if (this.Empty)
            {
                throw new InvalidOperationException("Queue is empty");
            }
            T dequeueValue = items[head];
            if (head == tail)
            {
                head = -1;
                tail = -1;
            }
            else
            {
                head = (head + 1) % items.Length;
            }
            return dequeueValue;
        }

        public T Peek()
        {
            if (this.Empty)
            {
                throw new InvalidOperationException("Queue is empty");
            }
            return items[head];
        }

        public void Clear()
        {
            head = -1;
        }

        public IEnumerator<T> GetEnumerator() => new CustomIterator(this);

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        private void GrowArray()
        {
            Array.Resize<T>(ref items, items.Length * 2);
        }

        private class CustomIterator : IEnumerator<T>
        {
            private readonly ArrayQueue<T> queue;
            private int currentShift;

            public CustomIterator(ArrayQueue<T> queue)
            {
                currentShift = -1;
                this.queue = queue;
            }

            public T Current
            {
                get
                {
                    if (currentShift == -1)
                    {
                        throw new InvalidOperationException();
                    }
                    var currentItemIndex = (queue.head + currentShift) % queue.items.Length;
                    return queue.items[currentItemIndex];
                }
            }

            public void Dispose() { }

            object System.Collections.IEnumerator.Current => Current;

            public bool MoveNext()
            {
                if (currentShift + 1 == queue.Count)
                {
                    return false;
                }
                currentShift++;
                return true;
            }

            public void Reset()
            {
                currentShift = -1;
            }
        }
    }
}
