using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    class Program
    {
        static void Main(string[] args)
        {
            Haffman h = new Haffman();
            bool[] b = h.Compress(Console.ReadLine());
            Console.WriteLine();
            h.Decompress(b);
            Console.WriteLine();
            Console.ReadKey();
        }
    }
}
