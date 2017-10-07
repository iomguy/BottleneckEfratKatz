using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BottleneckEfratKatz
{
    class HopcroftKarp
    {
        //static void Main()
        //{
        //    var lefts = new HashSet<int> { 1, 2, 3, 4, 5 };
        //    var rights = new HashSet<int> { 6, 7, 8, 9, 10 };

        //    var edges = new Dictionary<int, HashSet<int>>
        //    {
        //        [1] = new HashSet<int> { 6, 7 },
        //        [2] = new HashSet<int> { 6, 10 },
        //        [3] = new HashSet<int> { 8, 9 },
        //        [4] = new HashSet<int> { 6, 10 },
        //        [5] = new HashSet<int> { 7, 9 }
        //    };

        //    var matches = HopcroftKarpFunction(lefts, rights, edges);

        //    Console.WriteLine($"# of matches: {matches.Count}\n");

        //    foreach (var match in matches)
        //    {
        //        Console.WriteLine($"Match: {match.Key} -> {match.Value}");
        //    }
        //}

        //public static Func<int, HashSet<int>> EdgeDelegate = Graph.Edge;

        // BFS
        static bool HasAugmentingPath(IEnumerable<int> lefts,
                                             IReadOnlyDictionary<int, HashSet<int>> edges,
                                             IReadOnlyDictionary<int, int> toMatchedRight,
                                             IReadOnlyDictionary<int, int> toMatchedLeft,
                                             IDictionary<int, long> distances,
                                             Queue<int> q)
        {
            foreach (var left in lefts)
            {
                if (toMatchedRight[left] == 0)
                {
                    distances[left] = 0;
                    q.Enqueue(left);
                }
                else
                {
                    distances[left] = long.MaxValue;
                }
            }

            distances[0] = long.MaxValue;

            while (0 < q.Count)
            {
                var left = q.Dequeue();

                if (distances[left] < distances[0])
                {
                    foreach (var right in edges[left])
                    {
                        var nextLeft = toMatchedLeft[right];
                        if (distances[nextLeft] == long.MaxValue)
                        {
                            // The nextLeft has not been visited and is being visited.
                            distances[nextLeft] = distances[left] + 1;
                            q.Enqueue(nextLeft);
                        }
                    }
                }
            }

            return distances[0] != long.MaxValue;
        }

        // DFS
        static bool TryMatching(int left,
                                       IReadOnlyDictionary<int, HashSet<int>> edges,
                                       IDictionary<int, int> toMatchedRight,
                                       IDictionary<int, int> toMatchedLeft,
                                       IDictionary<int, long> distances)
        {
            if (left == 0)
            {
                return true;
            }

            foreach (var right in edges[left])
            {
                var nextLeft = toMatchedLeft[right];
                if (distances[nextLeft] == distances[left] + 1)
                {
                    if (TryMatching(nextLeft, edges, toMatchedRight, toMatchedLeft, distances))
                    {
                        toMatchedLeft[right] = left;
                        toMatchedRight[left] = right;
                        return true;
                    }
                }
            }

            // The left could not match any right.
            distances[left] = long.MaxValue;

            return false;
        }

        public static Dictionary<int, int> HopcroftKarpFunction(HashSet<int> lefts,
                                                              IEnumerable<int> rights,
                                                              IReadOnlyDictionary<int, HashSet<int>> edges)
        {
            // "distance" is from a starting left to another left when zig-zaging left, right, left, right, left in DFS.

            // Take the following for example:
            // left1 -> (unmatched edge) -> right1 -> (matched edge) -> left2 -> (unmatched edge) -> right2 -> (matched edge) -> left3
            // distance can be as follows.
            // distances[left1] = 0 (Starting left is distance 0.)
            // distances[left2] = distances[left1] + 1 = 1
            // distances[left3] = distances[left2] + 1 = 2

            // Note
            // Both a starting left and an ending left are unmatched with right.
            // Moving from left to right uses a unmatched edge.
            // Moving from right to left uses a matched edge.

            var distances = new Dictionary<int, long>();

            var q = new Queue<int>();

            // All lefts start as being unmatched with any right.
            var toMatchedRight = lefts.ToDictionary(s => s, s => 0);

            // All rights start as being unmatched with any left.
            var toMatchedLeft = rights.ToDictionary(s => s, s => 0);

            // Note
            // toMatchedRight and toMatchedLeft are the same thing but inverse to each other.
            // Using either of them is enough but inefficient
            // because a dictionary cannot be straightforwardly looked up bi-directionally.

            while (HasAugmentingPath(lefts, edges, toMatchedRight, toMatchedLeft, distances, q))
            {
                foreach (var unmatchedLeft in lefts.Where(left => toMatchedRight[left] == 0))
                {
                    TryMatching(unmatchedLeft, edges, toMatchedRight, toMatchedLeft, distances);
                }
            }

            // Remove unmatches
            RemoveItems(toMatchedRight, kvp => kvp.Value == 0);

            // Return matches
            return toMatchedRight;
        }

        static void RemoveItems<T1, T2>(IDictionary<T1, T2> d, Func<KeyValuePair<T1, T2>, bool> isRemovable)
        {
            foreach (var kvp in d.Where(isRemovable).ToList())
            {
                d.Remove(kvp.Key);
            }
        }
    }
}
