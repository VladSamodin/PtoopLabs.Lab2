using System;
using System.Collections.Generic;

namespace Task2.CustomQueues
{
    public class LinkedListQueue<T> : IEnumerable<T>
    {
        private Node head;

        private Node tail;

        private int count;

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
        }

        public IEnumerator<T> GetEnumerator() => new CustomIterator(head);

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

        private class Node
        {
            public T Data { get; set; }

            public Node Next { get; set; }
        }

        private class CustomIterator : IEnumerator<T>
        {
            private Node head;
            private Node currentNode;

            public CustomIterator(Node head)
            {
                this.head = head;
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
                if (currentNode == null)
                {
                    if (head == null)
                    {
                        return false;
                    }
                    currentNode = head;
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
