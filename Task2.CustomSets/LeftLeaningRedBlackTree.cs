using System;
using System.Collections.Generic;

namespace Task2.CustomSets
{
    /// <summary>
    /// Implements a left-leaning red-black tree.
    /// Based on http://dlaa.me/blog/post/9686081
    /// </summary>
    /// <remarks>
    /// Based on the research paper "Left-leaning Red-Black Trees"
    /// by Robert Sedgewick. More information available at:
    /// http://www.cs.princeton.edu/~rs/talks/LLRB/RedBlack.pdf
    /// http://www.cs.princeton.edu/~rs/talks/LLRB/08Penn.pdf
    /// </remarks>
    /// <typeparam name="TKey">Type of keys.</typeparam>
    internal class LeftLeaningRedBlackTree<TKey>
    {
        /// <summary>
        /// Stores the key comparison function.
        /// </summary>
        private IComparer<TKey> comparer;

        /// <summary>
        /// Stores the root node of the tree.
        /// </summary>
        private Node root;

        /// <summary>
        /// Represents a node of the tree.
        /// </summary>
        /// <remarks>
        /// Using fields instead of properties drops execution time by about 40%.
        /// </remarks>
        private class Node
        {
            /// <summary>
            /// Gets or sets the node's key.
            /// </summary>
            public TKey Key;

            /// <summary>
            /// Gets or sets the left node.
            /// </summary>
            public Node Left;

            /// <summary>
            /// Gets or sets the right node.
            /// </summary>
            public Node Right;

            /// <summary>
            /// Gets or sets the color of the node.
            /// </summary>
            public bool IsBlack;

            public Node(TKey key)
            {
                this.Key = key;
            }

            public Node(TKey key, bool isRed)
            {
                this.Key = key;
                this.IsBlack = !isRed;
            }
        }

        /// <summary>
        /// Initializes a new instance of the LeftLeaningRedBlackTree class implementing a normal dictionary.
        /// </summary>
        public LeftLeaningRedBlackTree()
        {
            this.comparer = Comparer<TKey>.Default;
        }

        /// <summary>
        /// Initializes a new instance of the LeftLeaningRedBlackTree class implementing a normal dictionary.
        /// </summary>
        /// <param name="comparer">The key comparer.</param>
        public LeftLeaningRedBlackTree(IComparer<TKey> comparer)
        {
            this.comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
        }

        public LeftLeaningRedBlackTree(TKey[] array, int startIndex, int endIndex, IComparer<TKey> comparer)
        {
            if (array == null)
            {
                throw new ArgumentNullException(nameof(array));
            }
            this.comparer = comparer ?? throw new ArgumentNullException(nameof(comparer));
            root = ConstructTreeFromSortedArray(array, startIndex, endIndex, null);
        }

        private Node ConstructTreeFromSortedArray(TKey[] array, int startIndex, int endIndex, Node redNode)
        {
            int size = endIndex - startIndex + 1;
            if (size == 0)
            {
                return null;
            }
            Node root = null;
            if (size == 1)
            {
                root = new Node(array[startIndex], false);
                if (redNode != null)
                {
                    root.Left = redNode;
                }
            }
            else if (size == 2)
            {
                root = new Node(array[startIndex], false);
                root.Right = new Node(array[endIndex], false);
                root.Right.IsBlack = false;
                if (redNode != null)
                {
                    root.Left = redNode;
                }
            }
            else if (size == 3)
            {
                root = new Node(array[startIndex + 1], false);
                root.Left = new Node(array[startIndex], false);
                root.Right = new Node(array[endIndex], false);
                if (redNode != null)
                {
                    root.Left.Left = redNode;
                }
            }
            else
            {
                int middle = (startIndex + endIndex) / 2;
                root = new Node(array[middle], false);
                root.Left = ConstructTreeFromSortedArray(array, startIndex, middle - 1, redNode);
                if (size % 2 == 0)
                {
                    root.Right = ConstructTreeFromSortedArray(array, middle + 2, endIndex, new Node(array[middle + 1], true));
                }
                else
                {
                    root.Right = ConstructTreeFromSortedArray(array, middle + 1, endIndex, null);
                }
            }
            return root;

        }

        /// <summary>
        /// Adds a key/value pair to the tree.
        /// </summary>
        /// <param name="key">Key to add.</param>
        public bool Add(TKey key)
        {
            int initialCount = Count;
            root = Add(root, key);
            root.IsBlack = true;
            return initialCount != Count;
        }

        /// <summary>
        /// Removes a key (and its associated value) from a normal (non-multi) dictionary.
        /// </summary>
        /// <param name="key">Key to remove.</param>
        /// <returns>True if key present and removed.</returns>
        public bool Remove(TKey key)
        {
            int initialCount = Count;
            if (root != null)
            {
                root = Remove(root, key);
                if (root != null)
                {
                    root.IsBlack = true;
                }
            }
            return initialCount != Count;
        }

        /// <summary>
        /// Removes all nodes in the tree.
        /// </summary>
        public void Clear()
        {
            root = null;
            Count = 0;
        }

        /// <summary>
        /// Gets a sorted list of keys in the tree.
        /// </summary>
        /// <returns>Sorted list of keys.</returns>
        public IEnumerable<TKey> GetKeys()
        {
            TKey lastKey = default(TKey);
            bool lastKeyValid = false;
            return Traverse(
                root,
                node => !lastKeyValid || !object.Equals(lastKey, node.Key),
                node =>
                {
                    lastKey = node.Key;
                    lastKeyValid = true;
                    return lastKey;
                });
        }

        /// <summary>
        /// Gets the count of key/value pairs in the tree.
        /// </summary>
        public int Count { get; private set; }

        /// <summary>
        /// Gets the minimum key in the tree.
        /// </summary>
        public TKey MinimumKey
        {
            get { return GetExtreme(root, node => node.Left, node => node.Key); }
        }

        /// <summary>
        /// Gets the maximum key in the tree.
        /// </summary>
        public TKey MaximumKey
        {
            get { return GetExtreme(root, node => node.Right, node => node.Key); }
        }

        /// <summary>
        /// Returns true if the specified node is red.
        /// </summary>
        /// <param name="node">Specified node.</param>
        /// <returns>True if specified node is red.</returns>
        private static bool IsRed(Node node)
        {
            if (null == node)
            {
                // "Virtual" leaf nodes are always black
                return false;
            }
            return !node.IsBlack;
        }

        /// <summary>
        /// Adds the specified key/value pair below the specified root node.
        /// </summary>
        /// <param name="node">Specified node.</param>
        /// <param name="key">Key to add.</param>
        /// <returns>New root node.</returns>
        private Node Add(Node node, TKey key)
        {
            if (node == null)
            {
                // Insert new node
                Count++;
                return new Node(key);
            }

            if (IsRed(node.Left) && IsRed(node.Right))
            {
                // Split node with two red children
                FlipColor(node);
            }

            // Find right place for new node
            int comparisonResult = comparer.Compare(key, node.Key);
            if (comparisonResult < 0)
            {
                node.Left = Add(node.Left, key);
            }
            else if (comparisonResult > 0)
            {
                node.Right = Add(node.Right, key);
            }
            else
            {
                // Key alredy exists, do nothing
                return node;
            }

            if (IsRed(node.Right))
            {
                // Rotate to prevent red node on right
                node = RotateLeft(node);
            }

            if (IsRed(node.Left) && IsRed(node.Left.Left))
            {
                // Rotate to prevent consecutive red nodes
                node = RotateRight(node);
            }

            return node;
        }

        /// <summary>
        /// Removes the specified key/value pair from below the specified node.
        /// </summary>
        /// <param name="node">Specified node.</param>
        /// <param name="key">Key to remove.</param>
        /// <returns>True if key/value present and removed.</returns>
        private Node Remove(Node node, TKey key)
        {
            int comparisonResult = comparer.Compare(key, node.Key);
            if (comparisonResult < 0)
            {
                // * Continue search if left is present
                if (node.Left != null)
                {
                    if (!IsRed(node.Left) && !IsRed(node.Left.Left))
                    {
                        // Move a red node over
                        node = MoveRedLeft(node);
                    }

                    // Remove from left
                    node.Left = Remove(node.Left, key);
                }
            }
            else
            {
                if (IsRed(node.Left))
                {
                    // Flip a 3 node or unbalance a 4 node
                    node = RotateRight(node);
                }
                if ((comparer.Compare(key, node.Key) == 0) && (node.Right == null))
                {
                    // Remove leaf node
                    Count--;
                    // Leaf node is gone
                    return null;
                }
                // * Continue search if right is present
                if (null != node.Right)
                {
                    if (!IsRed(node.Right) && !IsRed(node.Right.Left))
                    {
                        // Move a red node over
                        node = MoveRedRight(node);
                    }
                    if (comparer.Compare(key, node.Key) == 0)
                    {
                        // Remove leaf node
                        Count--;
                        // Find the smallest node on the right, swap, and remove it
                        Node minimum = GetExtreme(node.Right, n => n.Left, n => n);
                        node.Key = minimum.Key;
                        node.Right = DeleteMinimum(node.Right);
                    }
                    else
                    {
                        // Remove from right
                        node.Right = Remove(node.Right, key);
                    }
                }
            }

            // Maintain invariants
            return FixUp(node);
        }

        public bool Contains(TKey key)
        {
            return GetNodeForKey(key) != null;
        }

        /// <summary>
        /// Flip the colors of the specified node and its direct children.
        /// </summary>
        /// <param name="node">Specified node.</param>
        private static void FlipColor(Node node)
        {
            node.IsBlack = !node.IsBlack;
            node.Left.IsBlack = !node.Left.IsBlack;
            node.Right.IsBlack = !node.Right.IsBlack;
        }

        /// <summary>
        /// Rotate the specified node "left".
        /// </summary>
        /// <param name="node">Specified node.</param>
        /// <returns>New root node.</returns>
        private static Node RotateLeft(Node node)
        {
            Node x = node.Right;
            node.Right = x.Left;
            x.Left = node;
            x.IsBlack = node.IsBlack;
            node.IsBlack = false;
            return x;
        }

        /// <summary>
        /// Rotate the specified node "right".
        /// </summary>
        /// <param name="node">Specified node.</param>
        /// <returns>New root node.</returns>
        private static Node RotateRight(Node node)
        {
            Node x = node.Left;
            node.Left = x.Right;
            x.Right = node;
            x.IsBlack = node.IsBlack;
            node.IsBlack = false;
            return x;
        }

        /// <summary>
        /// Moves a red node from the right child to the left child.
        /// </summary>
        /// <param name="node">Parent node.</param>
        /// <returns>New root node.</returns>
        private static Node MoveRedLeft(Node node)
        {
            FlipColor(node);
            if (IsRed(node.Right.Left))
            {
                node.Right = RotateRight(node.Right);
                node = RotateLeft(node);
                FlipColor(node);

                // * Avoid creating right-leaning nodes
                if (IsRed(node.Right.Right))
                {
                    node.Right = RotateLeft(node.Right);
                }
            }
            return node;
        }

        /// <summary>
        /// Moves a red node from the left child to the right child.
        /// </summary>
        /// <param name="node">Parent node.</param>
        /// <returns>New root node.</returns>
        private static Node MoveRedRight(Node node)
        {
            FlipColor(node);
            if (IsRed(node.Left.Left))
            {
                node = RotateRight(node);
                FlipColor(node);
            }
            return node;
        }

        /// <summary>
        /// Deletes the minimum node under the specified node.
        /// </summary>
        /// <param name="node">Specified node.</param>
        /// <returns>New root node.</returns>
        private Node DeleteMinimum(Node node)
        {
            if (null == node.Left)
            {
                // Nothing to do
                return null;
            }

            if (!IsRed(node.Left) && !IsRed(node.Left.Left))
            {
                // Move red node left
                node = MoveRedLeft(node);
            }

            // Recursively delete
            node.Left = DeleteMinimum(node.Left);

            // Maintain invariants
            return FixUp(node);
        }

        /// <summary>
        /// Maintains invariants by adjusting the specified nodes children.
        /// </summary>
        /// <param name="node">Specified node.</param>
        /// <returns>New root node.</returns>
        private static Node FixUp(Node node)
        {
            if (IsRed(node.Right))
            {
                // Avoid right-leaning node
                node = RotateLeft(node);
            }

            if (IsRed(node.Left) && IsRed(node.Left.Left))
            {
                // Balance 4-node
                node = RotateRight(node);
            }

            if (IsRed(node.Left) && IsRed(node.Right))
            {
                // Push red up
                FlipColor(node);
            }

            // * Avoid leaving behind right-leaning nodes
            if ((null != node.Left) && IsRed(node.Left.Right) && !IsRed(node.Left.Left))
            {
                node.Left = RotateLeft(node.Left);
                if (IsRed(node.Left))
                {
                    // Balance 4-node
                    node = RotateRight(node);
                }
            }

            return node;
        }

        /// <summary>
        /// Gets the (first) node corresponding to the specified key.
        /// </summary>
        /// <param name="key">Key to search for.</param>
        /// <returns>Corresponding node or null if none found.</returns>
        private Node GetNodeForKey(TKey key)
        {
            // Initialize
            Node node = root;
            while (null != node)
            {
                // Compare keys and go left/right
                int comparisonResult = comparer.Compare(key, node.Key);
                if (comparisonResult < 0)
                {
                    node = node.Left;
                }
                else if (0 < comparisonResult)
                {
                    node = node.Right;
                }
                else
                {
                    // Match; return node
                    return node;
                }
            }

            // No match found
            return null;
        }

        /// <summary>
        /// Gets an extreme (ex: minimum/maximum) value.
        /// </summary>
        /// <typeparam name="T">Type of value.</typeparam>
        /// <param name="node">Node to start from.</param>
        /// <param name="successor">Successor function.</param>
        /// <param name="selector">Selector function.</param>
        /// <returns>Extreme value.</returns>
        private static T GetExtreme<T>(Node node, Func<Node, Node> successor, Func<Node, T> selector)
        {
            // Initialize
            T extreme = default(T);
            Node current = node;
            while (null != current)
            {
                // Go to extreme
                extreme = selector(current);
                current = successor(current);
            }
            return extreme;
        }

        /// <summary>
        /// Traverses a subset of the sequence of nodes in order and selects the specified nodes.
        /// </summary>
        /// <typeparam name="T">Type of elements.</typeparam>
        /// <param name="node">Starting node.</param>
        /// <param name="condition">Condition method.</param>
        /// <param name="selector">Selector method.</param>
        /// <returns>Sequence of selected nodes.</returns>
        private IEnumerable<T> Traverse<T>(Node node, Func<Node, bool> condition, Func<Node, T> selector)
        {
            // Create a stack to avoid recursion
            Stack<Node> stack = new Stack<Node>();
            Node current = node;
            while (null != current)
            {
                if (null != current.Left)
                {
                    // Save current state and go left
                    stack.Push(current);
                    current = current.Left;
                }
                else
                {
                    do
                    {
                        if (condition(current))
                        {
                            yield return selector(current);
                        }
                        current = current.Right;
                    }
                    while ((current == null) &&
                           (stack.Count > 0 ) &&
                           ((current = stack.Pop()) != null));
                }
            }
        }
    }
}

