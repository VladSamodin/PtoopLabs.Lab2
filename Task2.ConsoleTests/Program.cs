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

            var tree = new LeftLeaningRedBlackTree<int>(Comparer<int>.Default);
            var added = tree.Add(10);
            added = tree.Add(10);
            added = tree.Add(15);
            added = tree.Add(15);
            added = tree.Add(20);
            added = tree.Add(20);
            added = tree.Add(13);
            added = tree.Add(13);
            added = tree.Add(6);
            added = tree.Add(6);
            added = tree.Add(4);
            added = tree.Add(4);
            added = tree.Add(8);
            added = tree.Add(8);
            added = tree.Add(9);
            tree.Remove(8);
            foreach (var item in tree.GetKeys())
            {
                Console.WriteLine(item);
            } 


            return;

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
    }
}
