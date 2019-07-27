using System;
using System.Collections.Generic;
using System.Linq;

namespace KormenAlgorithms.RedBlackTree
{
    public static class Test
    {
        public static void Run()
        {
            var rnd = CommonHelper.GlobalRandom;
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

        private static void Check(RedBlackTree<int, int> tree, HashSet<int> goodImpl)
        {
            tree.Check();
            if (tree.Count != goodImpl.Count || goodImpl.Any(it => !tree.TryGet(it, out _)))
                throw new Exception("Red black tree differs from HashSet");
        }
    }
}