using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Task2.CustomSets
{
    public class ArraySet<T> : IEnumerable<T> 
    {
        public const int DefaultCapacity = 10;

        private T[] items;
        private int count;
        private readonly IComparer<T> comparer;

        public int Count => count;

        public bool Empty => count == 0;

        public int Capacity => items.Length;

        private ArraySet(T[] sortedArray, int count, IComparer<T> comparer)
        {
            items = sortedArray;
            this.count = count;
            this.comparer = comparer;
        }

        public ArraySet()
        {
            count = 0;
            items = new T[DefaultCapacity];
            this.comparer = Comparer<T>.Default;
        }

        public ArraySet(IComparer<T> comparer)
        {
            count = 0;
            items = new T[DefaultCapacity];
            this.comparer = comparer;
        }

        public ArraySet(ArraySet<T> set)
        {
            count = set.count;
            items = new T[set.Capacity];
            this.comparer = set.comparer;

            Array.Copy(set.items, items, set.Count);
        }

        public ArraySet(IEnumerable<T> collection)
        {
            this.comparer = Comparer<T>.Default;
            items = collection.ToArray();
            Array.Sort(items, comparer);
            count = items.Length;
        }

        public ArraySet(IEnumerable<T> collection, IComparer<T> comparer)
        {
            this.comparer = comparer;
            items = collection.ToArray();
            Array.Sort(items, comparer);
            count = items.Length;
        }

        public bool Add(T value)
        {
            if (ReferenceEquals(value, null))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var index = BinarySearch(value);
            if (index >= 0)
            {
                return false;
            }

            var insertIndex = -index - 1;
            InsertValue(insertIndex, value);
            count++;

            return true;
        }

        private int BinarySearch(T value)
        {
            return Array.BinarySearch(items, 0, count, value, comparer);
        }

        private void InsertValue(int index, T value)
        {
            if (count == Capacity)
            {
                Array.Resize(ref items, Capacity * 2);
            }

            if (index < count)
            {
                Array.Copy(items, index, items, index + 1, count - index);
            }

            items[index] = value;
        }

        public void Clear()
        {
            Array.Clear(items, 0, count);
            count = 0;
        }

        public bool Remove(T value)
        {
            if (ReferenceEquals(value, null))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var index = BinarySearch(value);
            if (index < 0)
            {
                return false;
            }

            count--;
            if (index < count)
            {
                Array.Copy(items, index + 1, items, index, count - index);
            }
            items[count] = default(T);

            return true;
        }

        public bool Contains(T value)
        {
            if (ReferenceEquals(value, null))
            {
                throw new ArgumentNullException(nameof(value));
            }

            var index = BinarySearch(value);
            return index >= 0;
        }

        public ArraySet<T> Union(ArraySet<T> anotherSet)
        {
            if (anotherSet == null)
            {
                throw new ArgumentNullException(nameof(anotherSet));
            }

            if (this.Empty)
            {
                return new ArraySet<T>(anotherSet);
            }

            if (anotherSet.Empty)
            {
                return new ArraySet<T>(this);
            }

            int unionCount;
            var unionArray = GenerateUnionArray(anotherSet, out unionCount);

            var union = new ArraySet<T>(unionArray, unionCount, comparer);

            return union;
        }

        private T[] GenerateUnionArray(ArraySet<T> anotherSet, out int count)
        {
            var unionArray = new T[this.Count + anotherSet.Count];
            var nextIndex = 0;

            var thisSetIndex = 0;
            var anotherSetIndex = 0;

            while (thisSetIndex < Count && anotherSetIndex < anotherSet.Count)
            {
                var compareResult = comparer.Compare(items[thisSetIndex], anotherSet.items[anotherSetIndex]);

                if (compareResult < 0)
                {
                    unionArray[nextIndex++] = items[thisSetIndex++];
                }
                else if (compareResult > 0)
                {
                    unionArray[nextIndex++] = anotherSet.items[anotherSetIndex++];
                }
                else
                {
                    unionArray[nextIndex++] = items[thisSetIndex];
                    thisSetIndex++;
                    anotherSetIndex++;
                }
            }

            if (thisSetIndex < Count)
            {
                var remainItems = Count - thisSetIndex;
                Array.Copy(items, thisSetIndex, unionArray, nextIndex, remainItems);
                count = nextIndex + remainItems;
                return unionArray;
            }

            if (anotherSetIndex < anotherSet.Count)
            {
                var remainItems = anotherSet.Count - anotherSetIndex;
                Array.Copy(anotherSet.items, anotherSetIndex, unionArray, nextIndex, remainItems);
                count = nextIndex + remainItems;
                return unionArray;
            }

            count = nextIndex;
            return unionArray;
        }

        public ArraySet<T> Intersect(ArraySet<T> anotherSet)
        {
            if (anotherSet == null)
            {
                throw new ArgumentNullException(nameof(anotherSet));
            }

            if (anotherSet.Empty)
            {
                return new ArraySet<T>();
            }
            
            var intersectionList = new List<T>();
            var startIndex = 0;
            foreach (var item in this)
            {
                var index = Array.BinarySearch(anotherSet.items, startIndex, anotherSet.Count - startIndex, item, comparer);
                if (index >= 0)
                {
                    intersectionList.Add(item);
                    startIndex = index;
                }
                else
                {
                    startIndex = -index - 1;
                }
            }

            var intersection = new ArraySet<T>(intersectionList.ToArray(), intersectionList.Count, comparer);

            return intersection;
        }

        public ArraySet<T> Difference(ArraySet<T> anotherSet)
        {
            if (anotherSet == null)
            {
                throw new ArgumentNullException(nameof(anotherSet));
            }

            if (anotherSet.Empty)
            {
                return new ArraySet<T>(this);
            }

            var differenceList = new List<T>();
            var startIndex = 0;
            foreach (var item in this)
            {
                var index = Array.BinarySearch(anotherSet.items, startIndex, anotherSet.Count - startIndex, item, comparer);
                if (index >= 0)
                {
                    startIndex = index;
                }
                else
                {
                    differenceList.Add(item);
                    startIndex = -index - 1;
                }
            }

            var difference = new ArraySet<T>(differenceList.ToArray(), differenceList.Count, comparer);

            return difference;
        }

        public bool IsSubset(ArraySet<T> anotherSet)
        {
            if (anotherSet == null)
            {
                throw new ArgumentNullException(nameof(anotherSet));
            }

            if (this.Empty)
            {
                return true;
            }

            if (this.Count > anotherSet.Count)
            {
                return false;
            }

            var startIndex = 0;
            foreach (var item in this)
            {
                var index = Array.BinarySearch(anotherSet.items, startIndex, anotherSet.Count - startIndex, item, comparer);
                if (index >= 0)
                {
                    startIndex = index;
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        public IEnumerator<T> GetEnumerator()
        {
            for (int i = 0; i < count; i++)
            {
                yield return items[i];
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
