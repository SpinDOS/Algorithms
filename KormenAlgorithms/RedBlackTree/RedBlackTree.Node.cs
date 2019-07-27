using System;

namespace KormenAlgorithms.RedBlackTree
{
    public partial class RedBlackTree<TKey, TValue> where TKey: IComparable<TKey>
    {
        private class Node
        {
            public Node() { }
            public Node(TKey key, TValue value, Node parent) { Key = key; Value = value; Parent = parent; }

            public TKey Key;
            public TValue Value;
            public bool IsBlack;
            public Node Left, Right, Parent;

            public bool IsRed
            {
                get => !IsBlack;
                set => IsBlack = !value;
            }

            public override string ToString() => $"{(IsBlack? "B" : "R")}: {Key.ToString()}:{Value.ToString()}";
        }
    }
}