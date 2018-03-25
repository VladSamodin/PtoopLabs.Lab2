namespace Task2.CustomQueues
{
    public interface IQueue<T>
    {
        int Count { get; }

        bool Empty { get; }

        void Enqueue(T value);

        T Dequeue();

        T Peek();

        void Clear();
    }
}
