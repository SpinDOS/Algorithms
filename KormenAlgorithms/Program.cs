using System;

namespace KormenAlgorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            while(true)
            {
                try
                {
                    RunTests();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    throw;
                }
            }
        }

        private static void RunTests()
        {
            RedBlackTree.Test.Run();
            BinaryHeap.Test.TestPiramidalSort();
            BinaryHeap.Test.TestPriorityQueue();
        }
    }
}