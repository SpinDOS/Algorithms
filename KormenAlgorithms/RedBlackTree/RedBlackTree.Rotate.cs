using System;

namespace KormenAlgorithms.RedBlackTree
{
    public partial class RedBlackTree<TKey, TValue> where TKey : IComparable<TKey>
    {
        private void RotateLeft(Node center)
        {
            var right = center.Right;
            var rightLeft = right.Left;

            FindRefToNode(center) = right;
            right.Parent = center.Parent;

            right.Left = center;
            center.Parent = right;

            center.Right = rightLeft;
            if (rightLeft != null)
                rightLeft.Parent = center;
        }

        private void RotateRight(Node center)
        {
            var left = center.Left;
            var leftRight = left.Right;

            FindRefToNode(center) = left;
            left.Parent = center.Parent;

            left.Right = center;
            center.Parent = left;

            center.Left = leftRight;
            if (leftRight != null)
                leftRight.Parent = center;
        }

        private ref Node FindRefToNode(Node node)
        {
            var parent = node.Parent;
            if (parent == null)
                return ref _root;
            if (parent.Left == node)
                return ref parent.Left;
            return ref parent.Right;
        }

        private static bool IsRed(Node node) => node != null && node.IsRed;
        private static bool IsBlack(Node node) => node == null || node.IsBlack;

        private static bool IsLeft(Node node) => IsLeft(node, node.Parent);
        private static bool IsLeft(Node node, Node parent) => parent.Left == node;
        
        private static Node Sibling(Node node, Node parent) => 
            IsLeft(node, parent)? parent.Right : parent.Left;
    }
}