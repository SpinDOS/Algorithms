namespace KormenAlgorithms.RedBlackTree
{
    public static class Test
    {
        public static void Run()
        {
            var rnd = CommonHelper.GlobalRandom;
            var tree = new RedBlackTree<int, int>();

            var c = rnd.Next(20000);
            for (int i = 0; i < c; i++)
            {
                var x = rnd.Next(-1000, 1000);
                tree[x] = x;
            }

            tree.Check();
        }
    }
}