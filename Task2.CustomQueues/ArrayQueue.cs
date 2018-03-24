using System;
using System.Collections;
using System.Collections.Generic;

namespace Task2.CustomQueues
{
    public class ArrayQueue<T> : IEnumerable<T>
    {
        private const int MinCapacity = 10;
        private T[] items;
        private int head;
        private int tail;
        private int version;

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


        public void Enqueue(T value)
        {
            if (this.Empty)
            {
                head = 0;
            }
            else if ((tail + 1) % items.Length == head)
            {
                GrowArray();
            }
            tail = (tail + 1) % items.Length;
            items[tail] = value;
            version++;
        }

        private void GrowArray()
        {
            var newItemArray = new T[items.Length * 2];
            if (head == 0)
            {
                Array.Copy(items, head, newItemArray, 0, items.Length);
            }
            else
            {
                Array.Copy(items, head, newItemArray, 0, items.Length - head);
                Array.Copy(items, 0, newItemArray, items.Length - head, head);
            }
            head = 0;
            tail = items.Length - 1;
            items = newItemArray;
        }

        public T Dequeue()
        {
            if (this.Empty)
            {
                throw new InvalidOperationException("Queue is empty");
            }
            T dequeueValue = items[head];
            items[head] = default(T);
            if (head == tail)
            {
                head = -1;
                tail = -1;
            }
            else
            {
                head = (head + 1) % items.Length;
            }
            version++;
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
            Array.Clear(items, 0, items.Length);
            head = -1;
            tail = -1;
            version++;
        }

        public IEnumerator<T> GetEnumerator() => new CustomIterator(this);

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

        private class CustomIterator : IEnumerator<T>
        {
            private readonly ArrayQueue<T> queue;
            private int currentShift;
            private int version;

            public CustomIterator(ArrayQueue<T> queue)
            {
                currentShift = -1;
                version = queue.version;
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
                if (version != queue.version)
                {
                    throw new InvalidOperationException("The queue was modified.");
                }

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
