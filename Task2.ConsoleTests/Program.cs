using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task2.CustomQueues;

namespace Task2.ConsoleTests
{
    class Program
    {
        static void Main(string[] args)
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
    }
}
