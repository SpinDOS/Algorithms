using System;

namespace KormenAlgorithms.RedBlackTree
{
    public partial class RedBlackTree<TKey, TValue> where TKey: IComparable<TKey>
    {
        internal sealed class Node
        {
            public Node(TKey key, TValue value, Node parent) { Key = key; Value = value; Parent = parent; }

            public TKey Key;
            public TValue Value;
            public Node Left, Right, Parent;

            public bool IsBlack { get; set; }

            public bool IsRed
            {
                get => !IsBlack;
                set => IsBlack = !value;
            }

            public override string ToString() => $"{(IsBlack? "B" : "R")}: {Key.ToString()}:{Value.ToString()}";
        }
    }
}