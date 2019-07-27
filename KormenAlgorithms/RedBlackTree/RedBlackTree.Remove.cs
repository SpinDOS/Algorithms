using System;

namespace KormenAlgorithms.RedBlackTree
{
    public partial class RedBlackTree<TKey, TValue> where TKey: IComparable<TKey>
    {
        private void RemoveInternal(Node node)
        {
            if (node.Left != null && node.Right != null)
                node = SwapWithClosestDescendant(node);
            
            var child = ReplaceWithChild(node);
            if (node.IsRed)
                return;
            
            // child is null or red
            if (child == null) 
                DecreasedBlackHeight(null, node.Parent);
            else
                child.IsBlack = true;
        }

        private void DecreasedBlackHeight(Node node, Node parent)
        {
            while (parent != null)
            {
                var sibling = Sibling(node, parent);
                if (sibling.IsRed)
                    sibling = MakeSiblingBlack(sibling, parent);

                if (IsRed(sibling.Left) || IsRed(sibling.Right))
                {
                    sibling = MakeSiblingOuterRed(sibling, out var siblingOuterChild);
                    RotateByParentAndRecolorOnRemove(sibling, parent, siblingOuterChild);
                    return;
                }

                sibling.IsRed = true;
                if (parent.IsRed)
                {
                    parent.IsBlack = true;
                    return;
                }

                node = parent;
                parent = node.Parent;
            }
        }

        private Node MakeSiblingBlack(Node sibling, Node parent)
        {
            // sibling is red => parent is black
            sibling.IsBlack = true;
            parent.IsRed = true;
            if (IsLeft(sibling, parent))
            {
                RotateRight(parent);
                return parent.Left;
            }
            else
            {
                RotateLeft(parent);
                return parent.Right;
            }
        }

        private Node MakeSiblingOuterRed(Node sibling, out Node outer)
        {
            var siblingIsLeft = IsLeft(sibling);
            
            outer = siblingIsLeft? sibling.Left : sibling.Right;
            if (IsRed(outer))
                return sibling;

            var inner = siblingIsLeft? sibling.Right : sibling.Left;

            sibling.IsRed = true;
            inner.IsBlack = true;
            
            if (siblingIsLeft)
                RotateLeft(sibling);
            else
                RotateRight(sibling);

            outer = sibling;
            return inner;
        }

        private void RotateByParentAndRecolorOnRemove(
            Node sibling,
            Node parent,
            Node siblingOuterChild)
        {
            if (IsLeft(sibling, parent))
                RotateRight(parent);
            else
                RotateLeft(parent);
            
            sibling.IsBlack = parent.IsBlack;
            parent.IsBlack = siblingOuterChild.IsBlack = true;
        }

        private Node ReplaceWithChild(Node node)
        {
            var child = node.Left ?? node.Right;
            FindRefToNode(node) = child;
            if (child != null)
                child.Parent = node.Parent;
            return child;
        }

        private static Node SwapWithClosestDescendant(Node node)
        {
            var descendant = node.Left;
            while (descendant.Right != null)
                descendant = descendant.Right;

            node.Key = descendant.Key;
            node.Value = descendant.Value;
            return descendant;
        }
    }
}