using System;
using System.Collections.Generic;
using System.Linq;

namespace NestedSets
{
    public static class NestedSetsCreator
    {
        public static List<Node> ToNestedSet(List<ClassicNode> classicTree)
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

                if (parent.Node == null)
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
        
        private static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var elem in list)
                action(elem);
        }

        private struct NodeWithParent
        {
            public Node Node;
            public decimal? Parent;
        }
    }
}