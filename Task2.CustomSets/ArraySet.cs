using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Task2.CustomSets
{
    public class ArraySet<T> : IEnumerable<T> 
        where T : class, IComparable<T>
    {
        public const int DefaultCapacity = 10;

        private T[] items;
        private int count;

        public int Count => count;

        public int Capacity => items.Length;

        public ArraySet()
        {
            count = 0;
            items = new T[DefaultCapacity];
        }

        public ArraySet(ArraySet<T> set)
        {
            count = set.count;
            items = new T[set.Capacity];

            Array.Copy(set.items, items, set.Count);
        }

        public ArraySet(IEnumerable<T> collection)
        {
            items = collection.ToArray();
            Array.Sort(items);
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
            return Array.BinarySearch(items, 0, count, value);
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

            if (this.Count == 0)
            {
                return new ArraySet<T>(anotherSet);
            }

            if (anotherSet.Count == 0)
            {
                return new ArraySet<T>(this);
            }

            var union = new ArraySet<T>(); 
            union.items = GenerateUnionArray(anotherSet, out union.count);

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
                var compareResult = items[thisSetIndex].CompareTo(anotherSet.items[anotherSetIndex]);

                if (compareResult > 0)
                {
                    unionArray[nextIndex++] = items[thisSetIndex++];
                    
                }
                else if (compareResult < 0)
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

        public ArraySet<T> Intersection(ArraySet<T> anotherSet)
        {
            if (anotherSet == null)
            {
                throw new ArgumentNullException(nameof(anotherSet));
            }

            if (anotherSet.Count == 0)
            {
                return new ArraySet<T>();
            }
            
            var intersectionList = new List<T>();
            var startIndex = 0;
            foreach (var item in this)
            {
                var index = Array.BinarySearch(anotherSet.items, startIndex, anotherSet.Count - startIndex, item);
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

            var intersection = new ArraySet<T>();
            intersection.items = intersectionList.ToArray();
            intersection.count = intersectionList.Count;

            return intersection;
        }

        public ArraySet<T> Difference(ArraySet<T> anotherSet)
        {
            if (anotherSet == null)
            {
                throw new ArgumentNullException(nameof(anotherSet));
            }

            if (anotherSet.Count == 0)
            {
                return new ArraySet<T>(this);
            }

            var differenceList = new List<T>();
            var startIndex = 0;
            foreach (var item in this)
            {
                var index = Array.BinarySearch(anotherSet.items, startIndex, anotherSet.Count - startIndex, item);
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

            var difference = new ArraySet<T>();
            difference.items = differenceList.ToArray();
            difference.count = differenceList.Count;

            return difference;
        }

        public bool IsSubset(ArraySet<T> anotherSet)
        {
            if (anotherSet == null)
            {
                throw new ArgumentNullException(nameof(anotherSet));
            }

            if (Count == 0)
            {
                return true;
            }

            var startIndex = 0;
            foreach (var item in this)
            {
                var index = Array.BinarySearch(anotherSet.items, startIndex, anotherSet.Count - startIndex, item);
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
