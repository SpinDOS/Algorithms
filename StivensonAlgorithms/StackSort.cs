using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    static class StackSort
    {
        public static void SortInsertion(Stack<int> s)
        {
            Stack<int> temp = new Stack<int>();
            int l = s.Count;
            for (int i = 0; i < l; i++)
            {
                int next = s.Pop();
                int j = 0;
                for (; j < l - i - 1; j++)
                    temp.Push(s.Pop());
                for (int k = 0; k < i; k++)
                {
                    int cur = s.Pop();
                    if (cur >= next)
                    {
                        s.Push(cur);
                        break;
                    }
                    temp.Push(cur);
                    j++;
                }
                s.Push(next);
                while (j-- > 0)
                {
                    s.Push(temp.Pop());
                }
            }
        }


        public static void SortSelection(Stack<int> s)
        {
            Stack<int> temp = new Stack<int>();
            int l = s.Count;
            for (int i = 0; i < l; i++)
            {
                int largest = s.Pop();
                temp.Push(largest);
                for (int j = 1; j < l - i; j++)
                {
                    int cur = s.Pop();
                    if (cur > largest)
                        largest = cur;
                    temp.Push(cur);
                }
                s.Push(largest);
                bool first = true;
                for (int j = 0; j < l - i; j++)
                {
                    int cur = temp.Pop();
                    if (first && cur == largest)
                    {
                        first = false;
                        continue;
                    }
                    s.Push(cur);
                }
            }
        }
    }
}
