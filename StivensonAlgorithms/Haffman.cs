using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;

namespace Algorithms
{
    class Haffman
    {
        private BinaryTreeNode root_tree;

        private class QueueWithPriority
        {
            private class Cell
            {
                public Cell(BinaryTreeNode t, int p)
                {
                    Value = t;
                    Priority = p;
                }
                public BinaryTreeNode Value { get; }
                public Cell Next { get; set; }
                public int Priority { get; set; }
            }

            private readonly Cell root = new Cell(null, 0);

            public int Count { get; private set; }
            public void Enqueue(BinaryTreeNode t, int priority)
            {
                Cell cur = root;
                while (cur.Next != null && cur.Next.Priority <= priority)
                    cur = cur.Next;
                Cell newCell = new Cell(t, priority);
                newCell.Next = cur.Next;
                cur.Next = newCell;
                Count++;
            }

            public BinaryTreeNode Dequeue(out int priority)
            {
                priority = 0;
                Cell c = root.Next;
                if (c == null)
                    return null;
                root.Next = c.Next;
                Count--;
                priority = c.Priority;
                return c.Value;
            }

            public void AddChar(char c)
            {
                Cell cur = root;
                while (cur.Next != null && cur.Next.Value.Value.Value != c)
                    cur = cur.Next;
                if (cur.Next == null)
                {
                    cur = root;
                    while (cur.Next != null && cur.Next.Priority == 1)
                        cur = cur.Next;
                    Cell newCell = new Cell(new BinaryTreeNode(c), 1);
                    newCell.Next = cur.Next;
                    cur.Next = newCell;
                    Count++;
                }
                else
                {
                    cur.Next.Priority++;
                    if (cur.Next.Next != null &&
                        cur.Next.Priority > cur.Next.Next.Priority)
                    {
                        Cell temp = cur.Next;
                        cur.Next = cur.Next.Next;
                        while (cur.Next != null && cur.Next.Priority < temp.Priority)
                            cur = cur.Next;
                        temp.Next = cur.Next;
                        cur.Next = temp;
                    }
                }
            }
        }

        private class BinaryTreeNode
        {
            public BinaryTreeNode(char c)
            {
                Value = c;
            }

            public BinaryTreeNode(BinaryTreeNode l, BinaryTreeNode r)
            {
                Value = null;
                Left = l;
                Right = r;
            }
            public char? Value { get; }
            public BinaryTreeNode Left { get; set; }
            public BinaryTreeNode Right { get; set; }

            public static void AddKeyValue(BinaryTreeNode node, 
                Dictionary<char, Queue<bool>> dictionary, Queue<bool> current)
            {
                if (node.Value.HasValue)
                    dictionary.Add(node.Value.Value, current);
                else
                {
                    Queue<bool> leftq = new Queue<bool>();
                    foreach (bool b in current)
                        leftq.Enqueue(b);
                    leftq.Enqueue(false);
                    AddKeyValue(node.Left, dictionary, leftq);
                    current.Enqueue(true);
                    AddKeyValue(node.Right, dictionary, current);
                }
                
            }
        }

        

        private BinaryTreeNode CreateTable(string s)
        {
            QueueWithPriority q = new QueueWithPriority();

            foreach (char c in s)
                q.AddChar(c);
            while (q.Count > 1)
            {
                int first, second;
                BinaryTreeNode newNode = new BinaryTreeNode
                    (q.Dequeue(out first), q.Dequeue(out second));
                q.Enqueue(newNode, first + second);
            }
            int notused;
            root_tree = q.Dequeue(out notused);
            return root_tree;
        }

        private void WriteChar(char c)
        {
            byte b = BitConverter.GetBytes(c)[0];
            for (byte byter = 128; byter >= 1; byter >>= 1)
                Console.Write( (byter & b) != 0? 1: 0);
            Console.Write(" ");
        }

        public bool[] Compress(string s)
        {
            Console.WriteLine("Исходная строка в бинарном виде: ");
            foreach (char c in s)
                WriteChar(c);
            BinaryTreeNode root = CreateTable(s);
            Dictionary<char, Queue<bool>> dictionary = new Dictionary<char, Queue<bool>>();
            BinaryTreeNode.AddKeyValue(root, dictionary, new Queue<bool>());
            Console.WriteLine();
            Console.WriteLine("Словарь: ");
            foreach (var pair in dictionary)
            {
                Console.Write(pair.Key + ": ");
                foreach (bool b in pair.Value)
                    Console.Write(b? "1" : "0");
                Console.WriteLine();
            }
            Queue<bool> q = new Queue<bool>();
            foreach (char c in s)
            {
                foreach (bool b in dictionary[c])
                {
                    q.Enqueue(b);
                    Console.Write(b? "1" : "0");
                }
                Console.Write(" ");
            }
            return q.ToArray();
        }

        public void Decompress(bool[] a)
        {
            BinaryTreeNode cur = root_tree;
            foreach (bool b in a)
            {
                if (b)
                {
                    cur = cur.Right;
                }
                else
                {
                    cur = cur.Left;
                }
                if (cur.Value.HasValue)
                {
                    Console.Write(cur.Value.Value);
                    cur = root_tree;
                }
            }
        }
    }
}
