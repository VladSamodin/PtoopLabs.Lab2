using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task2.CustomQueues;
using Task2.CustomSets;

namespace Task2.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
        {
            TestRedBlackTreeSet();

            Console.WriteLine();

            TestArraySet();
        }

        private static void TestQueue()
        {
            var arrayQueue = new ArrayQueue<int>();

            for (int i = 0; i < 18; i++)
            {
                arrayQueue.Enqueue(i);
            }

            Console.WriteLine(string.Join(", ", arrayQueue.Select(i => i.ToString()).ToArray()));

            arrayQueue.Dequeue();
            arrayQueue.Dequeue();
            arrayQueue.Dequeue();

            Console.WriteLine(string.Join(", ", arrayQueue.Select(i => i.ToString()).ToArray()));

            for (int i = 8; i < 12; i++)
            {
                arrayQueue.Enqueue(i);
            }

            Console.WriteLine(string.Join(", ", arrayQueue.Select(i => i.ToString()).ToArray()));

            for (int i = 0; i < 15; i++)
            {
                arrayQueue.Dequeue();
            }

            Console.WriteLine(string.Join(", ", arrayQueue.Select(i => i.ToString()).ToArray()));

            for (int i = 8; i < 12; i++)
            {
                arrayQueue.Enqueue(i);
            }

            Console.WriteLine(string.Join(", ", arrayQueue.Select(i => i.ToString()).ToArray()));

            for (int i = 0; i < 5; i++)
            {
                arrayQueue.Dequeue();
            }

            Console.WriteLine(string.Join(", ", arrayQueue.Select(i => i.ToString()).ToArray()));
        }

        private static void TestRedBlackTreeSet()
        {
            var set = new RedBlackTreeSet<int>(Comparer<int>.Default);

            set.Add(1);
            set.Add(3);
            set.Add(5);
            set.Add(6);
            set.Add(7);

            var anotherSet = new RedBlackTreeSet<int>(Comparer<int>.Default);

            anotherSet.Add(2);
            anotherSet.Add(3);
            anotherSet.Add(4);
            anotherSet.Add(6);
            anotherSet.Add(8);

            PrintCollection(set, i => i.ToString());

            PrintCollection(set.Union(new RedBlackTreeSet<int>()), i => i.ToString());

            PrintCollection(set.Intersect(new RedBlackTreeSet<int>()), i => i.ToString());

            PrintCollection(set.Difference(new RedBlackTreeSet<int>()), i => i.ToString());

            Console.WriteLine(new RedBlackTreeSet<int>().IsSubset(set));

            PrintCollection(anotherSet, i => i.ToString());

            PrintCollection(set.Union(anotherSet), i => i.ToString());

            PrintCollection(set.Intersect(anotherSet), i => i.ToString());

            PrintCollection(set.Difference(anotherSet), i => i.ToString());

            Console.WriteLine(set.IsSubset(anotherSet));

            set.Remove(1);
            set.Remove(5);
            set.Remove(7);

            Console.WriteLine(set.IsSubset(anotherSet));
        }

        private static void TestArraySet()
        {
            var set = new ArraySet<int>(Comparer<int>.Default);

            set.Add(1);
            set.Add(3);
            set.Add(5);
            set.Add(6);
            set.Add(7);

            var anotherSet = new ArraySet<int>(Comparer<int>.Default);

            anotherSet.Add(2);
            anotherSet.Add(3);
            anotherSet.Add(4);
            anotherSet.Add(6);
            anotherSet.Add(8);

            PrintCollection(set, i => i.ToString());

            PrintCollection(set.Union(new ArraySet<int>()), i => i.ToString());

            PrintCollection(set.Intersect(new ArraySet<int>()), i => i.ToString());

            PrintCollection(set.Difference(new ArraySet<int>()), i => i.ToString());

            Console.WriteLine(new ArraySet<int>().IsSubset(set));

            PrintCollection(anotherSet, i => i.ToString());

            PrintCollection(set.Union(anotherSet), i => i.ToString());

            PrintCollection(set.Intersect(anotherSet), i => i.ToString());

            PrintCollection(set.Difference(anotherSet), i => i.ToString());

            Console.WriteLine(set.IsSubset(anotherSet));

            set.Remove(1);
            set.Remove(5);
            set.Remove(7);

            Console.WriteLine(set.IsSubset(anotherSet));
        }

        private static void PrintCollection<T>(IEnumerable<T> collection, Func<T, string> toString)
        {
            var result = string.Join(", ", collection.Select(i => toString(i)));
            Console.WriteLine(result);
        }
    }
}
