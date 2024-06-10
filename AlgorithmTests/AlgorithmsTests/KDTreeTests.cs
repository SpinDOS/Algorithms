using System;
using System.Linq;
using K_D_Tree;
using NUnit.Framework;

namespace AlgorithmTests.AlgorithmsTests
{
    public class KDTreeTests
    {
        [Test]
        public void KnnInitialPointsTest()
        {
            var kdTree = KDTree.Build(TestPoints);
            foreach (var point in TestPoints)
            {
                var knn = kdTree.FindKNearestNeighbours(point, 1);
                Assert.That(knn.Count, Is.EqualTo(1));
                AssertNeighboursEqual(knn[0], point, 0);
            }
        }

        [Test]
        public void KnnNearInitialPointsTest()
        {
            var kdTree = KDTree.Build(TestPoints);
            var distanceSqr = Math.Pow(0.1, 2) + Math.Pow(0.2, 2);
            foreach (var point in TestPoints)
            {
                var pointNear = new PointKD(point.Coordinates[0] + 0.1, point.Coordinates[1] - 0.2);
                var knn = kdTree.FindKNearestNeighbours(pointNear, 1);
                Assert.That(knn.Count, Is.EqualTo(1));
                AssertNeighboursEqual(knn[0], point, distanceSqr);
            }
        }

        [Test]
        public void KnnNearToCenterTest()
        {
            var kdTree = KDTree.Build(TestPoints);
            var knn = kdTree.FindKNearestNeighbours(new PointKD(6, 8), 4);
            Assert.That(knn.Count, Is.EqualTo(4));
            Assert.That(knn[0].Point, Is.EqualTo(new PointKD(5, 5)));
            Assert.That(knn[1].Point, Is.EqualTo(new PointKD(2, 9)));
            Assert.That(knn[2].Point, Is.EqualTo(new PointKD(10, 10)));
            Assert.That(knn[3].Point, Is.EqualTo(new PointKD(0, 10)));
        }

        [Test]
        public void KnnOutOfCornerTest()
        {
            var kdTree = KDTree.Build(TestPoints);
            var knn = kdTree.FindKNearestNeighbours(new PointKD(-1, -1), 2);
            Assert.That(knn.Count, Is.EqualTo(2));
            Assert.That(knn[0].Point, Is.EqualTo(new PointKD(0, 0)));
            Assert.That(knn[1].Point, Is.EqualTo(new PointKD(1, 2)));
        }

        [Test]
        public void KnnLargeGridTest()
        {
            var points = Enumerable.Range(0, 10000).Select(it => new PointKD(it / 100, it % 100)).ToArray();
            var kdTree = KDTree.Build(points);
            var knn = kdTree.FindKNearestNeighbours(new PointKD(68.6, 68.7), 5);
            Assert.That(knn.Count, Is.EqualTo(5));
            Assert.That(knn[0].Point, Is.EqualTo(new PointKD(69, 69)));
            Assert.That(knn[1].Point, Is.EqualTo(new PointKD(68, 69)));
            Assert.That(knn[2].Point, Is.EqualTo(new PointKD(69, 68)));
            Assert.That(knn[3].Point, Is.EqualTo(new PointKD(68, 68)));
            Assert.That(knn[4].Point, Is.EqualTo(new PointKD(69, 70)));
        }

        private static void AssertNeighboursEqual(KDTree.Neighbour actual, PointKD point, double? distanceSqr) {
            Assert.That(actual.Point, Is.EqualTo(point));
            Assert.That(actual.DistanceToTargetSqr, Is.EqualTo(distanceSqr).Within(Epsilon));
        }

        private static PointKD[] TestPoints = new PointKD[] {
            new PointKD(0, 0),
            new PointKD(0, 10),
            new PointKD(10, 0),
            new PointKD(10, 10),
            new PointKD(1, 2),
            new PointKD(2, 9),
            new PointKD(5, 5),
        };

        private const double Epsilon = 1e-10;
    }
}
