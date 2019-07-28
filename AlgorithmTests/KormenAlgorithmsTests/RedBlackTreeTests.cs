using System.Collections.Generic;
using KormenAlgorithms.RedBlackTree;
using NUnit.Framework;

namespace AlgorithmTests.KormenAlgorithmsTests
{
    public class RedBlackTreeTests
    {
        [Test]
        [Repeat(1000)]
        public void RedBlackTreeTest()
        {
            var rnd = TestContext.CurrentContext.Random;
            var tree = new RedBlackTree<int, int>();
            var goodImpl = new HashSet<int>();

            var c = rnd.Next(2000);
            for (int i = 0; i < c; i++)
            {
                var x = rnd.Next(-1000, 1000);
                tree[x] = x;
                goodImpl.Add(x);
            }
            
            Check(tree, goodImpl);
            
            c = rnd.Next(2000);
            for (int i = 0; i < c; i++)
            {
                var x = rnd.Next(-1000, 1000);
                tree.Remove(x);
                goodImpl.Remove(x);
            }
            
            Check(tree, goodImpl);
        }
        
        private void Check(RedBlackTree<int, int> tree, HashSet<int> goodImpl)
        {
            if (tree._root != null)
                ValidateTree(tree._root);
            else
                Assert.AreEqual(0, tree.Count, "Root is null, Count is not zero");
            
            Assert.AreEqual(goodImpl.Count, tree.Count, "Count of elements differs");
            Assert.That(goodImpl, 
                Is.All.Matches<int>(it => tree.TryGet(it, out _)), 
                "Red black tree differs from HashSet");
        }

        private void ValidateTree(RedBlackTree<int, int>.Node root)
        {
            Assert.True(root.IsBlack, "Root is red");
            CheckRedChildren(root);
            CheckBlackHeight(root);
            CheckBinaryTree(root);
        }

        private void CheckRedChildren(RedBlackTree<int, int>.Node node)
        {
            if (node == null)
                return;

            var left = node.Left;
            var right = node.Right;
            
            if (node.IsRed)
            {
                Assert.That(node.Left, Is.Null.Or.Property(nameof(node.IsBlack)).True, "Red's child is red");
                Assert.That(node.Right, Is.Null.Or.Property(nameof(node.IsBlack)).True, "Red's child is red");
            }
            
            CheckRedChildren(left);
            CheckRedChildren(right);
        }

        private int CheckBlackHeight(RedBlackTree<int, int>.Node node)
        {
            if (node == null)
                return 1;

            var leftHeight = CheckBlackHeight(node.Left);
            var rightHeight = CheckBlackHeight(node.Right);
            Assert.AreEqual(leftHeight, rightHeight, "Black height differs");
            return leftHeight + (node.IsBlack? 1 : 0);
        }

        private void CheckBinaryTree(RedBlackTree<int, int>.Node node)
        {
            var left = node.Left;
            if (left != null)
            {
                Assert.Less(left.Key, node.Key, "Left node's key is not less that parent nodes key");
                CheckBinaryTree(left);
            }
            
            var right = node.Right;
            if (right != null)
            {
                Assert.Greater(right.Key, node.Key, "Right node's key is not greater that parent nodes key");
                CheckBinaryTree(right);
            }
        }
    }
}