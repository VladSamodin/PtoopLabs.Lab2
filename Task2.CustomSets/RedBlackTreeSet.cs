using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Task2.CustomSets
{
    public class RedBlackTreeSet<T> : IEnumerable<T>
    {
        private readonly LeftLeaningRedBlackTree<T> tree;
        private readonly IComparer<T> comparer;

        public int Count => tree.Count;

        public bool Empty => tree.Count == 0;

        public RedBlackTreeSet()
        {
            this.comparer = Comparer<T>.Default;
            tree = new LeftLeaningRedBlackTree<T>(this.comparer);
        }

        public RedBlackTreeSet(IComparer<T> comparer)
        {
            this.comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            tree = new LeftLeaningRedBlackTree<T>(this.comparer);
        }

        public RedBlackTreeSet(RedBlackTreeSet<T> anotherSet)
        {
            var sortedArray = anotherSet.ToArray();
            this.comparer = anotherSet.comparer;
            tree = new LeftLeaningRedBlackTree<T>(sortedArray, 0, sortedArray.Length - 1, this.comparer);
        }

        private RedBlackTreeSet(LeftLeaningRedBlackTree<T> tree, IComparer<T> comparer)
        {
            this.tree = tree ?? throw new ArgumentNullException(nameof(tree));
            this.comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        public bool Add(T value)
        {
            return tree.Add(value);
        }

        public void Clear()
        {
            tree.Clear();
        }

        public bool Remove(T value)
        {
            return tree.Remove(value);
        }

        public bool Contains(T value)
        {
            return tree.Contains(value);
        }

        public RedBlackTreeSet<T> Union(RedBlackTreeSet<T> anotherSet)
        {
            if (anotherSet == null)
            {
                throw new ArgumentNullException(nameof(anotherSet));
            }

            if (anotherSet.Empty)
            {
                return new RedBlackTreeSet<T>(this);
            }

            int unionCount = 0;
            var unionArray = new T[this.Count + anotherSet.Count];
            Action<T> handler = item => unionArray[unionCount++] = item;
            MergeInOrderTraversal(anotherSet, handler, handler, handler);
            var unionTree = new LeftLeaningRedBlackTree<T>(unionArray, 0, unionCount - 1, comparer);

            return new RedBlackTreeSet<T>(unionTree, comparer);
        }

        public RedBlackTreeSet<T> Intersection(RedBlackTreeSet<T> anotherSet)
        {
            if (anotherSet == null)
            {
                throw new ArgumentNullException(nameof(anotherSet));
            }

            if (anotherSet.Empty)
            {
                return new RedBlackTreeSet<T>();
            }

            int intersectionCount = 0;
            var minSize = this.Count > anotherSet.Count ? anotherSet.Count : this.Count;
            var intersectionArray = new T[minSize];
            MergeInOrderTraversal(anotherSet, null, null, item => intersectionArray[intersectionCount++] = item);
            var intersectionTree = new LeftLeaningRedBlackTree<T>(intersectionArray, 0, intersectionCount - 1, comparer);

            return new RedBlackTreeSet<T>(intersectionTree, comparer);
        }

        private void MergeInOrderTraversal(RedBlackTreeSet<T> anotherSet, Action<T> thisHandler, Action<T> anotherHandler, Action<T> equalHandler)
        {
            var thisEnumerator = GetEnumerator();
            var anotherEnumerator = anotherSet.GetEnumerator();

            var thisEnded = !thisEnumerator.MoveNext();
            var anotherEnded = !anotherEnumerator.MoveNext();
            while (!thisEnded && !anotherEnded)
            {
                int compareResult = comparer.Compare(thisEnumerator.Current, anotherEnumerator.Current);
                if (compareResult < 0)
                {
                    thisHandler?.Invoke(thisEnumerator.Current);
                    thisEnded = !thisEnumerator.MoveNext();
                }
                else if (compareResult > 0)
                {
                    anotherHandler?.Invoke(anotherEnumerator.Current);
                    anotherEnded = !anotherEnumerator.MoveNext();
                }
                else
                {
                    equalHandler?.Invoke(anotherEnumerator.Current);
                    thisEnded = !thisEnumerator.MoveNext();
                    anotherEnded = !anotherEnumerator.MoveNext();
                }
            }

            if (!thisEnded)
            {
                do
                {
                    thisHandler?.Invoke(thisEnumerator.Current);
                } while (thisEnumerator.MoveNext());
            }

            if (!anotherEnded)
            {
                do
                {
                    anotherHandler?.Invoke(anotherEnumerator.Current);
                } while (anotherEnumerator.MoveNext());
            }
        }

        public RedBlackTreeSet<T> Difference(RedBlackTreeSet<T> anotherSet)
        {
            if (anotherSet == null)
            {
                throw new ArgumentNullException(nameof(anotherSet));
            }

            if (anotherSet.Empty)
            {
                return new RedBlackTreeSet<T>(this);
            }

            int differenceCount = 0;
            var differenceArray = new T[this.Count];
            MergeInOrderTraversal(anotherSet, item => differenceArray[differenceCount++] = item, null, null);

            var differenceTree = new LeftLeaningRedBlackTree<T>(differenceArray, 0, differenceCount - 1, comparer);

            return new RedBlackTreeSet<T>(differenceTree, comparer);
        }

        public bool IsSubset(RedBlackTreeSet<T> anotherSet)
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

            var thisEnumerator = GetEnumerator();
            var anotherEnumerator = anotherSet.GetEnumerator();

            var thisEnded = !thisEnumerator.MoveNext();
            var anotherEnded = !anotherEnumerator.MoveNext();
            while (!thisEnded && !anotherEnded)
            {
                int compareResult = comparer.Compare(thisEnumerator.Current, anotherEnumerator.Current);
                if (compareResult < 0)
                {
                    return false;
                }
                else if (compareResult > 0)
                {
                    anotherEnded = !anotherEnumerator.MoveNext();
                }
                else
                {
                    thisEnded = !thisEnumerator.MoveNext();
                    anotherEnded = !anotherEnumerator.MoveNext();
                }
            }

            return thisEnded;
        }

        public IEnumerator<T> GetEnumerator()
        {
            foreach (var item in tree.GetKeys())
            {
                yield return item;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
