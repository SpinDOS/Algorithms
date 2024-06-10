using System;
using System.Collections.Generic;
using System.Linq;

namespace K_D_Tree {
    public struct PointKD : IEquatable<PointKD> {
        public PointKD(double x) => Coordinates = new double[] {x};
        public PointKD(double x, double y) => Coordinates = new double[] {x, y};
        public PointKD(double x, double y, double z) => Coordinates = new double[] {x, y, z};
        public PointKD(double[] coordinates) => Coordinates = coordinates;
        public double[] Coordinates { get; } = Array.Empty<double>();

        public bool Equals(PointKD other) => Enumerable.SequenceEqual(Coordinates, other.Coordinates);
        public override string ToString() => string.Join(", ", Coordinates);
    }

    public sealed class KDTree {
        private sealed class Node {
            public PointKD Point;
            public Node Left;
            public Node Right;
        }

        public struct Neighbour {
            public Neighbour(PointKD point, double distanceToTargetSqr) {
                Point = point;
                DistanceToTargetSqr = distanceToTargetSqr;
            }
            public PointKD Point { get; }
            public double DistanceToTargetSqr { get; }
        }

        private sealed class KNNResultAccumulator {
            public KNNResultAccumulator(int neighboursCount) {
                NeighboursCount_ = neighboursCount;
                Queue_ = new PriorityQueue<Neighbour, double>(neighboursCount);
            }

            public void TryAdd(Neighbour neighbour) {
                if (Queue_.Count == NeighboursCount_ && Queue_.Peek().DistanceToTargetSqr > neighbour.DistanceToTargetSqr) {
                    Queue_.Dequeue();
                }
                if (Queue_.Count < NeighboursCount_) {
                    Queue_.Enqueue(neighbour, -neighbour.DistanceToTargetSqr);
                    // queue finds min priority, but we need find max distance, so we set negative distance as priority
                }
            }

            public double GetCurrentAcceptedWorstDistanceSqr() =>
                Queue_.Count == NeighboursCount_ ? Queue_.Peek().DistanceToTargetSqr : double.PositiveInfinity;

            public List<Neighbour> GetNeighbours() {
                var result = new List<Neighbour>(Queue_.Count);
                while (Queue_.TryDequeue(out var neighbour, out _)) {
                    result.Add(neighbour);
                }
                result.Reverse();
                return result;
            }

            private readonly int NeighboursCount_;
            private readonly PriorityQueue<Neighbour, double> Queue_;
        }

        public static KDTree Build(Span<PointKD> points) {
            if (points == null) {
                throw new ArgumentNullException(nameof(points));
            }
            foreach (var point in points) {
                if (point.Coordinates == null || point.Coordinates.Length == 0) {
                    throw new ArgumentException("Got invalid point without coordinates");
                }
                if (point.Coordinates.Length != points[0].Coordinates.Length) {
                    throw new ArgumentException($"Got points of different dimensions: {point.Coordinates.Length} and {points[0].Coordinates.Length}");
                }
            }
            return new KDTree(BuildNode(points, 0));
        }

        public List<Neighbour> FindKNearestNeighbours(PointKD target, int neighboursCount) {
            if (Root_ == null || neighboursCount <= 0) {
                return new List<Neighbour>();
            }
            if (target.Coordinates == null) {
                throw new ArgumentNullException(nameof(target));
            }
            if (target.Coordinates.Length != Root_.Point.Coordinates.Length) {
                throw new ArgumentException($"Got points of different dimensions: {target.Coordinates.Length} and {Root_.Point.Coordinates.Length}");
            }

            var kNNResultAccumulator = new KNNResultAccumulator(neighboursCount);
            TraverseKNearestNeighboursImpl(kNNResultAccumulator, target, Root_, 0);
            return kNNResultAccumulator.GetNeighbours();
        }

        private static Node BuildNode(Span<PointKD> points, int depth) {
            if (points.Length == 0) {
                return null;
            }
            var currentDim = depth % points[0].Coordinates.Length;
            // sort here slows down build of the tree, it can be replaced with more optimal algorithm.
            points.Sort((lhs, rhs) => lhs.Coordinates[currentDim].CompareTo(rhs.Coordinates[currentDim]));
            var mid = points.Length / 2;
            return new Node {
                Point = points[mid],
                Left = BuildNode(points.Slice(0, mid), depth + 1),
                Right = BuildNode(points.Slice(mid + 1), depth + 1),
            };
        }

        private void TraverseKNearestNeighboursImpl(KNNResultAccumulator kNNResultAccumulator, PointKD target, Node node, int depth) {
            if (node == null) {
                return;
            }
            var currentDim = depth % node.Point.Coordinates.Length;
            Node mainBranch, otherBranch;
            if (target.Coordinates[currentDim] < node.Point.Coordinates[currentDim]) {
                mainBranch = node.Left;
                otherBranch = node.Right;
            } else {
                mainBranch = node.Right;
                otherBranch = node.Left;
            }

            TraverseKNearestNeighboursImpl(kNNResultAccumulator, target, mainBranch, depth + 1);

            var currentDistanceSqr = CalculateDistanceSqr(node.Point, target);
            kNNResultAccumulator.TryAdd(new Neighbour(node.Point, currentDistanceSqr));

            var distanceSqrToDimSplit = Math.Pow(target.Coordinates[currentDim] - node.Point.Coordinates[currentDim], 2.0);
            if (distanceSqrToDimSplit <= kNNResultAccumulator.GetCurrentAcceptedWorstDistanceSqr()) {
                TraverseKNearestNeighboursImpl(kNNResultAccumulator, target, otherBranch, depth + 1);
            }
        }

        private static double CalculateDistanceSqr(PointKD lhs, PointKD rhs) {
            return lhs.Coordinates.Zip(rhs.Coordinates).Select(pair => Math.Pow(pair.First - pair.Second, 2.0)).Sum();
        }

        private KDTree(Node root) => Root_ = root;
        private readonly Node Root_;
    }
}
