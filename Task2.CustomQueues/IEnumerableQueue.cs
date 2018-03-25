using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task2.CustomQueues
{
    public interface IEnumerableQueue<T> : IQueue<T>, IEnumerable<T>
    {
    }
}
