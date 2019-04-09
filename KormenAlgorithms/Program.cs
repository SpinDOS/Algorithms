using System;

namespace KormenAlgorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 1000; i++)
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
            
            Console.WriteLine("OK");
            Console.ReadKey(true);
        }

        private static void RunTests()
        {
            RedBlackTree.Test.Run();
            BinaryHeap.Test.TestPiramidalSort();
            BinaryHeap.Test.TestPriorityQueue();
        }
    }
}