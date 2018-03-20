using System;
using System.Collections.Generic;

namespace Task2.CustomQueues
{
    public class LinkedListQueue<T> : IEnumerable<T>
    {
        private Node head;
        private Node tail;
        private int count;
        private int version;

        public int Count => count;

        public bool Empty => Count == 0;

        public LinkedListQueue()
        {
            head = null;
            tail = null;
            count = 0;
        }

        public void Enqueue(T toAdd)
        {
            var newNode = new Node
            {
                 Data = toAdd,
                 Next = null
            };

            if (this.Empty)
            {
                head = newNode;
                tail = newNode;
            }
            else
            {
                tail.Next = newNode;
                tail = newNode;
            }
            count++;
            version++;
        }

        public T Dequeue()
        {
            if (this.Empty)
            {
                throw new InvalidOperationException("Queue is empty");
            }

            T dequeueValue = head.Data;

            head = head.Next;
            count--;
            version++;

            return dequeueValue;
        }

        public T Peek()
        {
            if (this.Empty)
            {
                throw new InvalidOperationException("Queue is empty");
            }
            return head.Data;
        }

        public void Clear()
        {
            head = null;
            tail = null;
            count = 0;
            version++;
        }

        public IEnumerator<T> GetEnumerator() => new CustomIterator(this);

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        private class Node
        {
            public T Data { get; set; }

            public Node Next { get; set; }
        }

        private class CustomIterator : IEnumerator<T>
        {
            private LinkedListQueue<T> queue;
            private Node currentNode;
            private int version;

            public CustomIterator(LinkedListQueue<T> queue)
            {
                this.queue = queue;
                version = queue.version;
                currentNode = null;
            }

            public T Current
            {
                get
                {
                    if (currentNode == null)
                    {
                        throw new InvalidOperationException();
                    }
                    return currentNode.Data;
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

                if (queue.head == null)
                {
                    return false;
                }

                if (currentNode == null)
                {
                    currentNode = queue.head;
                    return true;
                }

                if (currentNode.Next == null)
                {
                    return false;
                }
                currentNode = currentNode.Next;
                return true;
            }

            public void Reset()
            {
                currentNode = null;
            }
        }
    }
}
