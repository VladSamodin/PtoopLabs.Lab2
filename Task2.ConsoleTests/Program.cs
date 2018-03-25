using System;
using System.Collections.Generic;
using System.Linq;
using Task2.CustomQueues;
using Task2.CustomSets;

namespace Task2.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            QueueTests();

            SetTests();
        }

        private static void QueueTests()
        {
            var arrayValueTypeQueue = new ArrayQueue<int>();
            Console.WriteLine("ArrayQueue<int> test:");
            TestValueTypeQueue(arrayValueTypeQueue);

            Console.WriteLine("=======================================");
            Console.WriteLine();

            var linkedListValueTypeQueue = new LinkedListQueue<int>();
            Console.WriteLine("LinkedListQueue<int> test:");
            TestValueTypeQueue(linkedListValueTypeQueue);

            Console.WriteLine("=======================================");
            Console.WriteLine();

            var arrayReferenceTypeQueue = new ArrayQueue<string>();
            Console.WriteLine("ArrayQueue<string> test:");
            TestReferenceTypeQueue(arrayReferenceTypeQueue);

            Console.WriteLine("=======================================");
            Console.WriteLine();

            var linkedListReferenceTypeQueue = new LinkedListQueue<string>();
            Console.WriteLine("LinkedListQueue<string> test:");
            TestReferenceTypeQueue(linkedListReferenceTypeQueue);

            Console.WriteLine("=======================================");
            Console.WriteLine();
        }

        private static void TestValueTypeQueue(IEnumerableQueue<int> queue)
        {
            for (int i = 0; i < 18; i++)
            {
                queue.Enqueue(i);
            }

            Console.WriteLine("Add range 0-17.");
            PrintCollection(queue);

            queue.Dequeue();
            queue.Dequeue();
            queue.Dequeue();

            Console.WriteLine("Dequeued 3 items.");
            PrintCollection(queue);

            for (int i = 8; i < 12; i++)
            {
                queue.Enqueue(i);
            }

            Console.WriteLine("Enqueued range 8-11.");
            PrintCollection(queue);

            for (int i = 0; i < 15; i++)
            {
                queue.Dequeue();
            }

            Console.WriteLine("Dequeued 15 items.");
            PrintCollection(queue);

            for (int i = 8; i < 12; i++)
            {
                queue.Enqueue(i);
            }

            Console.WriteLine("Enqueued range 8-11.");
            PrintCollection(queue);

            for (int i = 0; i < 5; i++)
            {
                queue.Dequeue();
            }

            Console.WriteLine("Dequeued 5 items.");
            PrintCollection(queue);
        }

        private static void TestReferenceTypeQueue(IEnumerableQueue<string> queue)
        {
            for (char i = 'a'; i < 'a' + 18; i++)
            {
                queue.Enqueue(i.ToString());
            }

            Console.WriteLine("Add range a-r.");
            PrintCollection(queue);

            queue.Dequeue();
            queue.Dequeue();
            queue.Dequeue();

            Console.WriteLine("Dequeued 3 items.");
            PrintCollection(queue);

            for (char i = 'o'; i < 'o' + 4; i++)
            {
                queue.Enqueue(i.ToString());
            }

            Console.WriteLine("Enqueued range o-r.");
            PrintCollection(queue);

            for (int i = 0; i < 15; i++)
            {
                queue.Dequeue();
            }

            Console.WriteLine("Dequeued 15 items.");
            PrintCollection(queue);

            for (char i = 'o'; i < 'o' + 4; i++)
            {
                queue.Enqueue(i.ToString());
            }

            Console.WriteLine("Enqueued range o-r.");
            PrintCollection(queue);

            for (int i = 0; i < 5; i++)
            {
                queue.Dequeue();
            }

            Console.WriteLine("Dequeued 5 items.");
            PrintCollection(queue);
        }

        private static void SetTests()
        {
            var rbtIntSet = new RedBlackTreeSet<int>();
            var anotherRbtIntSet = new RedBlackTreeSet<int>();
            var emptyRbtIntSet = new RedBlackTreeSet<int>();

            Console.WriteLine("RedBlackTreeSet<int> tests:");
            TestValueTypeSet(rbtIntSet, anotherRbtIntSet, emptyRbtIntSet);

            Console.WriteLine("=======================================");
            Console.WriteLine();

            var arrayIntSet = new ArraySet<int>();
            var anotherArrayIntSet = new ArraySet<int>();
            var emptyArrayIntSet = new ArraySet<int>();

            Console.WriteLine("ArraySet<int> tests:");
            TestValueTypeSet(arrayIntSet, anotherArrayIntSet, emptyArrayIntSet);

            Console.WriteLine("=======================================");
            Console.WriteLine();

            var rbtStringSet = new RedBlackTreeSet<string>();
            var anotherRbtStringSet = new RedBlackTreeSet<string>();
            var emptyRbtStringSet = new RedBlackTreeSet<string>();

            Console.WriteLine("RedBlackTreeSet<string> tests:");
            TestReferenceTypeSet(rbtStringSet, anotherRbtStringSet, emptyRbtStringSet);

            Console.WriteLine("=======================================");
            Console.WriteLine();

            var arrayStringSet = new ArraySet<string>();
            var anotherArrayStringSet = new ArraySet<string>();
            var emptyArrayStringSet = new ArraySet<string>();

            Console.WriteLine("ArraySet<string> tests:");
            TestReferenceTypeSet(arrayStringSet, anotherArrayStringSet, emptyArrayStringSet);

            Console.WriteLine("=======================================");
            Console.WriteLine();
        }

        private static void TestValueTypeSet(dynamic set, dynamic anotherSet, dynamic emptySet)
        {
            set.Add(1);
            set.Add(3);
            set.Add(5);
            set.Add(6);
            set.Add(7);

            anotherSet.Add(2);
            anotherSet.Add(3);
            anotherSet.Add(4);
            anotherSet.Add(6);
            anotherSet.Add(8);

            Console.WriteLine("set:");
            PrintCollection(set);

            Console.WriteLine("set.Union(emptySet):");
            PrintCollection(set.Union(emptySet));

            Console.WriteLine("set.Intersect(emptySet):");
            PrintCollection(set.Intersect(emptySet));

            Console.WriteLine("set.Difference(emptySet):");
            PrintCollection(set.Difference(emptySet));

            Console.WriteLine("emptySet.IsSubset(set): " + emptySet.IsSubset(set));
            Console.WriteLine();

            Console.WriteLine("anotherSet:");
            PrintCollection(anotherSet);

            Console.WriteLine("set.Union(anotherSet):");
            PrintCollection(set.Union(anotherSet));

            Console.WriteLine("set.Intersect(anotherSet):");
            PrintCollection(set.Intersect(anotherSet));

            Console.WriteLine("set.Difference(anotherSet):");
            PrintCollection(set.Difference(anotherSet));

            Console.WriteLine("set.IsSubset(anotherSet): " + set.IsSubset(anotherSet));
            Console.WriteLine();

            set.Remove(1);
            set.Remove(5);
            set.Remove(7);

            Console.WriteLine("Removed 1, 5, 7 from set:");
            PrintCollection(set);

            Console.WriteLine("set.IsSubset(anotherSet): " + set.IsSubset(anotherSet));
            Console.WriteLine();
        }

        private static void TestReferenceTypeSet(dynamic set, dynamic anotherSet, dynamic emptySet)
        {
            set.Add("One");
            set.Add("Three");
            set.Add("Five");
            set.Add("Six");
            set.Add("Seven");

            anotherSet.Add("Two");
            anotherSet.Add("Three");
            anotherSet.Add("Four");
            anotherSet.Add("Six");
            anotherSet.Add("Eight");

            Console.WriteLine("set:");
            PrintCollection(set);

            Console.WriteLine("set.Union(emptySet):");
            PrintCollection(set.Union(emptySet));

            Console.WriteLine("set.Intersect(emptySet):");
            PrintCollection(set.Intersect(emptySet));

            Console.WriteLine("set.Difference(emptySet):");
            PrintCollection(set.Difference(emptySet));

            Console.WriteLine("emptySet.IsSubset(set): " + emptySet.IsSubset(set));
            Console.WriteLine();

            Console.WriteLine("anotherSet:");
            PrintCollection(anotherSet);

            Console.WriteLine("set.Union(anotherSet):");
            PrintCollection(set.Union(anotherSet));

            Console.WriteLine("set.Intersect(anotherSet):");
            PrintCollection(set.Intersect(anotherSet));

            Console.WriteLine("set.Difference(anotherSet):");
            PrintCollection(set.Difference(anotherSet));

            Console.WriteLine("set.IsSubset(anotherSet): " + set.IsSubset(anotherSet));
            Console.WriteLine();

            set.Remove("One");
            set.Remove("Five");
            set.Remove("Seven");

            Console.WriteLine("Removed 1, 5, 7 from set:");
            PrintCollection(set);

            Console.WriteLine("set.IsSubset(anotherSet): " + set.IsSubset(anotherSet));
            Console.WriteLine();
        }

        private static void PrintCollection<T>(IEnumerable<T> collection)
        {
            var result = string.Join(", ", collection);
            Console.WriteLine(result);
            Console.WriteLine();
        }

        private static void PrintCollection<T>(IEnumerable<T> collection, Func<T, string> toString)
        {
            var result = string.Join(", ", collection.Select(i => toString(i)));
            Console.WriteLine(result);
        }
    }
}
