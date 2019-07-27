using System;

namespace KormenAlgorithms.RedBlackTree
{
    public partial class RedBlackTree<TKey, TValue> where TKey: IComparable<TKey>
    {
        private void AddInternal(TKey key, TValue value, Node parent)
        {
            var newNode = new Node(key, value, parent);
            
            if (parent == null)
                _root = newNode;
            else if (key.CompareTo(parent.Key) < 0)
                parent.Left = newNode;
            else
                parent.Right = newNode;

            OnInsert(newNode);
            _root.IsBlack = true;
            Count++;
        }
        
        private void OnInsert(Node newRedNode)
        {
            while (true)
            {
                var parent = newRedNode.Parent;
                if (parent == null || parent.IsBlack)
                    return;

                // parent is red => parent is not root
                var grandParent = parent.Parent;
                var uncle = Sibling(parent, grandParent);
            
                if (IsBlack(uncle))
                {
                    parent = EnsureNodeIsOnOuterPlace(newRedNode, parent);
                    RotateGrandParentAndRecolorOnInsert(parent, grandParent);
                    return;
                }

                parent.IsBlack = uncle.IsBlack = true;
                grandParent.IsRed = true;
                newRedNode = grandParent;
            }
        }

        private Node EnsureNodeIsOnOuterPlace(Node node, Node parent)
        {
            var nodeIsLeft = IsLeft(node, parent);
            if (nodeIsLeft == IsLeft(parent))
                return parent;
            
            if (nodeIsLeft)
                RotateRight(parent);
            else
                RotateLeft(parent);

            return node;
        }

        private void RotateGrandParentAndRecolorOnInsert(Node parent, Node grandParent)
        {
            if (IsLeft(parent, grandParent))
                RotateRight(grandParent);
            else
                RotateLeft(grandParent);

            grandParent.IsRed = true;
            parent.IsBlack = true;
        }
    }
}