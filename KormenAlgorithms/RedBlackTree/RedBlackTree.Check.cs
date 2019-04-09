using System;

namespace KormenAlgorithms.RedBlackTree
{
    public partial class RedBlackTree<TKey, TValue> where TKey: IComparable<TKey>
    {
        internal void Check()
        {
            var root = Root.Left;

            Assert(root == null || root.IsBlack);
            CheckRedChildren(root);
            CheckBlackHeight(root);

            if (root != null)
            {
                CheckBinaryTree(root);
            }
        }

        private void CheckRedChildren(Node node)
        {
            if (node == null)
            {
                return;
            }

            var left = node.Left;
            var right = node.Right;
            
            if (node.IsRed)
            {
                Assert(left == null || left.IsBlack);
                Assert(right == null || right.IsBlack);
            }
            
            CheckRedChildren(left);
            CheckRedChildren(right);
        }

        private int CheckBlackHeight(Node node)
        {
            if (node == null)
            {
                return 1;
            }

            var leftHeight = CheckBlackHeight(node.Left);
            var rightHeight = CheckBlackHeight(node.Right);
            Assert(leftHeight == rightHeight);

            return leftHeight + (node.IsBlack? 1 : 0);
        }

        private void CheckBinaryTree(Node node)
        {
            var left = node.Left;
            if (left != null)
            {
                Assert(left.Key.CompareTo(node.Key) < 0);
                CheckBinaryTree(left);
            }
            
            var right = node.Right;
            if (right != null)
            {
                Assert(right.Key.CompareTo(node.Key) > 0);
                CheckBinaryTree(right);
            }
        }
        
        private void Assert(bool b)
        {
            if (!b)
            {
                throw new Exception($"Check failed for tree: {Environment.NewLine}{this.ToString()}");
            }
        }
    }
}