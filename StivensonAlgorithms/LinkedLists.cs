using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    public class Cell
    {
        public Cell(int v, Cell c)
        {
            Value = v;
            Next = c;
        }
        public Cell (int v) : this(v, null) { }

        public int Value { get; set; }
        public Cell Next { get; set; }
    }

    static class LinkedLists
    {
        public static Cell CreateListWithCircle()
        {
            int i = 1;
            Cell first = new Cell(i++);
            Cell cur = first;
            Cell super = null;
            while (i < 9)
            {
                cur = cur.Next = new Cell(i++);
                if (i == 5)
                    super = cur;
            }
            cur.Next = super;
            return first;
        }

        public static Cell CreateListWithOutCircle()
        {
            int i = 1;
            Cell first = new Cell(i++);
            Cell cur = first;
            while (i < 9)
            {
                cur = cur.Next = new Cell(i++);
            }
            return first;
        }

        public static bool CheckCirclesWithTracer(Cell first)
        {
            Cell current = first.Next;
            while (current != null)
            {
                Cell tracer = first;
                while (tracer != current)
                {
                    if (tracer.Next == current.Next)
                        return true;
                    tracer = tracer.Next;
                }
                current = current.Next;
            }
            return false;
        }

        public static bool CheckCirclesWithFastSlow(Cell first)
        {
            Cell fast = first, slow = first;
            do
            {
                if (fast?.Next == null)
                    return false;
                fast = fast.Next.Next;
                slow = slow.Next;
            } while (fast != slow);
            return true;
        }

        private static Cell ReverseList(Cell first)
        {
            Cell prev = null;
            Cell cur = first;
            while (cur != null)
            {
                Cell next = cur.Next;
                cur.Next = prev;
                prev = cur;
                cur = next;
            }
            return prev;
        }

        public static bool CheckCirclesWithReverse(Cell first)
        {
            bool result = ReverseList(first) == first;
            ReverseList(first);
            return result;
        }
}
}
