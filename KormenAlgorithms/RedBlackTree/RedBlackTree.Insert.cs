using System;

namespace KormenAlgorithms.RedBlackTree
{
    public partial class RedBlackTree<TKey, TValue> where TKey: IComparable<TKey>
    {
        private void OnInsert(Node node)
        {
            var parent = node.Parent;
            if (parent == Root)
            {
                node.IsBlack = true;
                return;
            }

            if (parent.IsBlack)
            {
                return;
            }
            
            var grandParent = node.GrandParent;
            var uncle = node.Uncle;
            if (uncle != null && uncle.IsRed)
            {
                uncle.IsBlack = parent.IsBlack = true;
                grandParent.IsRed = true;
                
                OnInsert(grandParent);
                return;
            }

            bool nodeIsLeft = node.IsLeft;
            if (nodeIsLeft != parent.IsLeft)
            {
                if (nodeIsLeft)
                {
                    RotateRight(parent);
                }
                else
                {
                    RotateLeft(parent);
                }

                nodeIsLeft = !nodeIsLeft;
                Swap(ref node, ref parent);
            }

            if (nodeIsLeft)
            {
                RotateRight(grandParent);
            }
            else
            {
                RotateLeft(grandParent);
            }

            grandParent.IsRed = true;
            parent.IsBlack = true;
        }

        private static void RotateLeft(Node center)
        {
            var parent = center.Parent;
            var right = center.Right;
            var rightLeft = right.Left;

            if (center.IsLeft)
            {
                parent.Left = right;
            }
            else
            {
                parent.Right = right;
            }
            
            right.Parent = parent;

            right.Left = center;
            center.Parent = right;

            center.Right = rightLeft;
            if (rightLeft != null)

            rightLeft.Parent = center;
        }

        private static void RotateRight(Node center)
        {
            var parent = center.Parent;
            var left = center.Left;
            var leftRight = left.Right;

            if (center.IsLeft)
            {
                parent.Left = left;
            }
            else
            {
                parent.Right = left;
            }
            left.Parent = parent;

            left.Right = center;
            center.Parent = left;

            center.Left = leftRight;
            if (leftRight != null)
            leftRight.Parent = center;
        }

        private static void Swap<T>(ref T x, ref T y)
        {
            var temp = x;
            x = y;
            y = temp;
        }
    }
}