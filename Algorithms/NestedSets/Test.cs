using System;
using System.Collections.Generic;
using System.Linq;

namespace NestedSets
{
    public static class Test
    {
        public static void Run()
        {
            var classicTree = RandomizeClassicTree();
            var tree = ToNestedSet(classicTree);
            
            AssertTree(tree);

            Print(classicTree);
            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine();
            Print(tree);
        }

        private static List<Node> ToNestedSet(List<ClassicNode> classicTree)
        {
            var tree = classicTree
                .Select(n => new NodeWithParent() 
                { 
                    Node = new Node() { Value = n.Id, Left = -1, Right = -1, Level = -1 }, 
                    Parent = n.ParentId,
                })
                .ToList();

            var root = tree.First(t => t.Parent == null).Node;
            root.Level = 1;
            root.Left = 1;
            root.Right = 2;

            while (true)
            {
                var parent = (from node in tree
                    join parentNode in tree on node.Parent equals parentNode.Node.Value
                    orderby node.Parent ?? -1
                    where node.Node.Left == -1 && parentNode.Node.Left >= 0
                    select parentNode).FirstOrDefault();

                if (parent == null)
                    break;

                var parentId = parent.Node.Value;
                var parentRight = parent.Node.Right;
                var parentLevel = parent.Node.Level;

                var diff = 2 * tree.Count(n => n.Parent == parentId);
                
                tree.Where(n => n.Node.Left  >= parentRight).ForEach(n => n.Node.Left += diff);
                tree.Where(n => n.Node.Right >= parentRight).ForEach(n => n.Node.Right += diff);

                tree.Where(n => n.Parent == parentId)
                    .OrderBy(n => n.Node.Value)
                    .ForEach(n =>
                    {
                        n.Node.Left = parentRight++;
                        n.Node.Right = parentRight++;
                        n.Node.Level = parentLevel + 1;
                    });
            }

            return tree.Select(n => n.Node).ToList();
        }

        private static List<ClassicNode> RandomizeClassicTree()
        {
            var rnd = new Random();
            var list = new List<ClassicNode>();
            for (int i = 0; i < 20; i++)
            {
                decimal val;
                do
                    val = rnd.Next(100);
                while (list.Any(n => n.Id == val));
                list.Add(new ClassicNode() { Id = val, });
            }

            for (int i = 1; i < 20; i++)
                list[i].ParentId = list[rnd.Next(i)].Id;

            return list;
        }

        static void Print(List<Node> tree)
        {
            tree.OrderBy(n => n.Left)
                .ForEach(n => Console.WriteLine(new string(' ', n.Level * 2 - 2) + n.Value));
        }
        
        static void Print(List<ClassicNode> tree)
        {
            Print(tree.First(n => n.ParentId == null), 1);
            void Print(ClassicNode node, int level)
            {
                Console.WriteLine(new string(' ', level * 2 - 2) + node.Id);
                tree.Where(n => n.ParentId == node.Id)
                    .OrderBy(n => n.Id)
                    .ForEach(n => Print(n, level + 1));
            }
        }

        static void AssertTree(List<Node> tree)
        {
            if (tree.Count == 0)
                return;
            
            // 1. Левый ключ ВСЕГДА меньше правого;
            var leftNotLessRight = tree.Where(node => node.Left >= node.Right).ToArray();
            Assert.IsEmpty(leftNotLessRight, "Left is not less than right: " + 
                                             leftNotLessRight.ToStringWithPrepend(Environment.NewLine));

            // 2. Наименьший левый ключ ВСЕГДА равен 1;
            var minLeft = tree.OrderBy(node => node.Left).First();
            Assert.AreEqual(1, minLeft.Left, "Minimal left is not 1: " + minLeft);

            // 3. Наибольший правый ключ ВСЕГДА равен двойному числу узлов;
            var maxRight = tree.OrderByDescending(node => node.Right).First();
            Assert.AreEqual(tree.Count * 2, maxRight.Right, "Max right is not equal to double node count: " + maxRight);

            // 4. Разница между правым и левым ключом ВСЕГДА нечетное число;
            var withEvenDiff = tree.Where(node => (node.Right - node.Left) % 2 == 0).ToArray();
            Assert.IsEmpty(withEvenDiff, "There are nodes with even difference of right and left: " + 
                                         withEvenDiff.ToStringWithPrepend(Environment.NewLine));

            // 5. Если уровень узла нечетное число то тогда левый ключ ВСЕГДА нечетное число, то же самое и для четных чисел;
            var invalidLevelLeftOddness = tree.Where(node => (node.Left + node.Level) % 2 == 1).ToArray();
            Assert.IsEmpty(invalidLevelLeftOddness, "There are nodes with invalid oddness of left and level: " +
                                                    invalidLevelLeftOddness.ToStringWithPrepend(Environment.NewLine));
            
            // 6. Ключи ВСЕГДА уникальны, вне зависимости от того правый он или левый;
            var duplicateKeys = tree.Select(node => new {node, Key = node.Left})
                .Concat(tree.Select(node => new {node, Key = node.Right}))
                .GroupBy(x => x.Key)
                .Select(g => new {Key = g.Key, Nodes = g.Select(x => x.node).ToArray()})
                .Where(x => x.Nodes.Length > 1)
                .ToArray();
            
            Assert.IsEmpty(duplicateKeys, "Found keys with duplicates: " +
                                          duplicateKeys.Select(x => x.Key + x.Nodes.ToStringWithPrepend(Environment.NewLine + "  "))
                                          .ToStringWithPrepend(Environment.NewLine));
        }

        private class NodeWithParent
        {
            public Node Node;
            public decimal? Parent;
        }
    }
}