using System;
using System.Collections.Generic;
using System.Text;

namespace KormenAlgorithms.RedBlackTree
{
    public partial class RedBlackTree<TKey, TValue> where TKey: IComparable<TKey>
    {
        // https://ru.wikipedia.org/wiki/Красно-чёрное_дерево
        
        private readonly Node Root = new Node(); // fake node

        public void Add(TKey key, TValue value)
        {
            var parent = FindParent(key, out var isLeft);
            var node = isLeft? parent.Left : parent.Right;
            if (node != null)
            {
                throw new ArgumentException($"An item with the same key has already been added. Key: {key.ToString()}", nameof(key));
            }
            
            AddInternal(key, value, parent, isLeft);
        }

        public bool TryGet(TKey key, out TValue value)
        {
            var parent = FindParent(key, out var isLeft);
            var node = isLeft? parent.Left : parent.Right;
            if (node == null)
            {
                value = default(TValue); 
                return false;
            }
            
            value = node.Value;
            return true;
        }
        
        public bool Remove(TKey key) => throw new NotImplementedException();

        public TValue this[TKey key]
        {
            get => TryGet(key, out var result)? result : throw new KeyNotFoundException(key.ToString());
            set
            {
                var parent = FindParent(key, out var isLeft);
                var node = isLeft? parent.Left : parent.Right;
                if (node == null)
                {
                    AddInternal(key, value, parent, isLeft);
                }
                else
                {
                    node.Value = value;
                }
            }
        }

        public override string ToString()
        {
            var stringBuilder = new StringBuilder();
            ToStringInternal(stringBuilder, Root.Left, 0);
            return stringBuilder.ToString();
        }

        private Node FindParent(TKey key, out bool isLeft)
        {
            var cur = Root;
            isLeft = true;
            while (true)
            {
                var next = isLeft? cur.Left : cur.Right;
                if (next == null)
                {
                    return cur;
                }

                var compare = key.CompareTo(next.Key);
                if (compare == 0)
                {
                    return cur;
                }

                isLeft = compare < 0;
                cur = next;
            }
        }

        private void AddInternal(TKey key, TValue value, Node parent, bool isLeft)
        {
            var newNode = new Node(key, value, parent);
            if (isLeft)
            {
                parent.Left = newNode;
            }
            else
            {
                parent.Right = newNode;
            }

            OnInsert(newNode);
        }

        private void ToStringInternal(StringBuilder stringBuilder, Node node, int level)
        {
            var prefix = new string(' ', level * 2);
            var content = node == null? "B: Empty" : node.ToString();
            stringBuilder.AppendLine(prefix + content);
            if (node == null)
            {
                return;
            }
            
            ToStringInternal(stringBuilder, node.Left, level + 1);
            ToStringInternal(stringBuilder, node.Right, level + 1);
        }
        
    }
}