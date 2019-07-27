using System;
using System.Collections.Generic;
using System.Text;

namespace KormenAlgorithms.RedBlackTree
{
    public partial class RedBlackTree<TKey, TValue> where TKey: IComparable<TKey>
    {
        // https://en.wikipedia.org/wiki/Red–black_tree

        private Node _root;
        
        public int Count { get; private set; }

        public void Add(TKey key, TValue value)
        {
            var node = FindNode(key, out var parent);
            if (node != null)
                throw new ArgumentException($"An item with the same key has already been added. Key: {key.ToString()}", nameof(key));
            
            AddInternal(key, value, parent);
        }

        public bool TryGet(TKey key, out TValue value)
        {
            var node = FindNode(key, out _);
            value = node != null? node.Value : default(TValue);
            return node != null;
        }

        public bool Remove(TKey key)
        {
            var node = FindNode(key, out _);
            if (node == null)
                return false;

            RemoveInternal(node);

            Count--;
            return true;
        }

        public TValue this[TKey key]
        {
            get
            {
                var node = FindNode(key, out _);
                return node != null? node.Value: throw new KeyNotFoundException(key.ToString());
            }
            
            set
            {
                var node = FindNode(key, out var parent);
                if (node == null)
                    AddInternal(key, value, parent);
                else
                    node.Value = value;
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            ToStringInternal(stringBuilder, _root, 0);
            return stringBuilder.ToString();
        }

        private Node FindNode(TKey key, out Node parent)
        {
            parent = null;
            var current = _root;
            
            while (true)
            {
                if (current == null)
                    return null;

                var compare = key.CompareTo(current.Key);
                if (compare == 0)
                    return current;

                parent = current;
                current = compare < 0 ? current.Left : current.Right;
            }
        }

        private static void ToStringInternal(StringBuilder stringBuilder, Node node, int level)
        {
            stringBuilder.Append(new string(' ', level * 2));
            stringBuilder.AppendLine(node?.ToString() ?? "B: Empty");
            if (node == null)
                return;
            
            ToStringInternal(stringBuilder, node.Left, level + 1);
            ToStringInternal(stringBuilder, node.Right, level + 1);
        }
        
    }
}