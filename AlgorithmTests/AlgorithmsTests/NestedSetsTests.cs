using System.Collections.Generic;
using System.Linq;
using NestedSets;
using NUnit.Framework;

namespace AlgorithmTests.AlgorithmsTests
{
    public class NestedSetsTests
    {
        [Test]
        [Repeat(1000)]
        public void CreateNestedSetsTest()
        {
            var classicTree = RandomizeClassicTree();
            var nestedSets = NestedSetsCreator.ToNestedSet(classicTree);
            
            Assert.That(nestedSets, Is.All.Matches<Node>(it => it.Left < it.Right), 
                "Left is not less than right");

            var minLeft = nestedSets.Min(it => it.Left);
            Assert.AreEqual(1, minLeft, "Minimal left is not 1");

            var maxRight = nestedSets.Max(it => it.Right);
            Assert.AreEqual(nestedSets.Count * 2, maxRight, "Max right is not equal to double node count");

            Assert.That(nestedSets, Is.All.Matches<Node>(it => (it.Right - it.Left) % 2 == 1), 
                "There are nodes with even difference of right and left");
            
            Assert.That(nestedSets, Is.All.Matches<Node>(it => (it.Left % 2) == (it.Level % 2)), 
                "There are nodes with invalid oddness of left and level");

            var allKeys = nestedSets.Select(it => it.Left)
                .Concat(nestedSets.Select(it => it.Right));
            Assert.That(allKeys, Is.Unique, "Found keys duplicates");

            ValidateTreesAreEqual(classicTree, nestedSets);
        }

        private void ValidateTreesAreEqual(List<ClassicNode> classicTree, List<Node> nestedSets)
        {
            Assert.AreEqual(classicTree.Count, nestedSets.Count, "NestedSets Count differs");
            
            foreach (var classicNode in classicTree)
            {
                var nestedSetNode = nestedSets.SingleOrDefault(it => it.Value == classicNode.Id);
                Assert.NotNull(nestedSetNode, "Could not find node with id " + classicNode.Id);
                if (classicNode.ParentId == null)
                {
                    Assert.AreEqual(1, nestedSetNode.Left, "Root's left is not 1");
                    continue;
                }
                
                var parentNode = nestedSets.SingleOrDefault(it => it.Value == classicNode.ParentId);
                Assert.NotNull(nestedSetNode, "Could not find node with id " + classicNode.ParentId);
                
                Assert.AreEqual(parentNode.Level + 1, nestedSetNode.Level, "Child Level is wrong");
                Assert.Greater(nestedSetNode.Left, parentNode.Left, "Child's Left is not greater than parent's Left");
                Assert.Less(nestedSetNode.Right, parentNode.Right, "Child's Right is not less than parent's Right");
            }
        }

        private List<ClassicNode> RandomizeClassicTree()
        {
            var rnd = TestContext.CurrentContext.Random;
            var count = rnd.Next(20, 100);
            
            var list = new List<ClassicNode>();
            for (var i = 0; i < count; i++)
            {
                decimal val;
                do
                    val = rnd.Next(200);
                while (list.Any(n => n.Id == val));
                list.Add(new ClassicNode() { Id = val, });
            }

            for (var i = 1; i < list.Count; i++)
                list[i].ParentId = list[rnd.Next(i)].Id;

            return list;
        }
    }
}