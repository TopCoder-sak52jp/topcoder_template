using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace topcoder_template_test
{
    public class DoubleUtil
    {
        private static double EPS = 1e-10;

        public static double Add(double a, double b)
        {
            if (Math.Abs(a + b) < EPS * (Math.Abs(a) + Math.Abs(b))) return 0;
            return a + b;
        }

        public static bool Eq(double a, double b)
        {
            return Math.Abs(a - b) < 1e-9;
        }
    }

    public class PriorityQueue<T> where T: IComparable
    {
        private IComparer<T> _comparer = null;
        private int _type = 0;

        private T[] _heap;
        private int _sz = 0;

        private int _count = 0;

        /// <summary>
        /// Priority Queue with custom comparer
        /// </summary>
        public PriorityQueue(int maxSize, IComparer<T> comparer)
        {
            _heap = new T[maxSize];
            _comparer = comparer;
        }

        /// <summary>
        /// Priority queue
        /// </summary>
        /// <param name="maxSize">max size</param>
        /// <param name="type">0: asc, 1:desc</param>
        public PriorityQueue(int maxSize, int type = 0)
        {
            _heap = new T[maxSize];
            _type = type;
        }

        private int Compare(T x, T y)
        {
            if (_comparer != null) return _comparer.Compare(x, y);
            return _type == 0 ? x.CompareTo(y) : y.CompareTo(x);
        }

        public void Push(T x)
        {
            _count++;

            //node number
            var i = _sz++;

            while (i > 0)
            {
                //parent node number
                var p = (i - 1) / 2;

                if (Compare(_heap[p], x) <= 0) break;

                _heap[i] = _heap[p];
                i = p;
            }

            _heap[i] = x;
        }

        public T Pop()
        {
            _count--;

            T ret = _heap[0];
            T x = _heap[--_sz];

            int i = 0;
            while (i * 2 + 1 < _sz)
            {
                //children
                int a = i * 2 + 1;
                int b = i * 2 + 2;

                if (b < _sz && Compare(_heap[b], _heap[a]) < 0) a = b;

                if (Compare(_heap[a], x) >= 0) break;

                _heap[i] = _heap[a];
                i = a;
            }

            _heap[i] = x;

            return ret;
        }

        public int Count()
        {
            return _count;
        }

        public T Peek()
        {
            return _heap[0];
        }

        public bool Contains(T x)
        {
            for (int i = 0; i < _sz; i++) if (x.Equals(_heap[i])) return true;
            return false;
        }

        public void Clear()
        {
            while (this.Count() > 0) this.Pop();
        }

        public IEnumerator<T> GetEnumerator()
        {
            var ret = new List<T>();

            while (this.Count() > 0)
            {
                ret.Add(this.Pop());
            }

            foreach (var r in ret)
            {
                this.Push(r);
                yield return r;
            }
        }

        public T[] ToArray()
        {
            T[] array = new T[_sz];
            int i = 0;

            foreach (var r in this)
            {
                array[i++] = r;
            }

            return array;
        }
    }

    /// <summary>
    /// NOT COMPLETED
    /// </summary>
    public class Bounds<T> where T: IComparable
    {
        public int LowerBound(IList<T> enumParameter, T target)
        {
            int l = 0;
            int r = enumParameter.Count - 1;

            while (r > l)
            {
                int m = (l + r) / 2;
                if (enumParameter[m].CompareTo(target) < 0)
                {
                    l = m + 1;
                }
                else
                {
                    r = m - 1;
                }
            }

            return l;
        }
    }

    static public class MyLib
    {
        static public void DisplayIEnumContents<T>(IEnumerable<T> EnumParam)
        {
            Console.WriteLine(string.Join(",", EnumParam.ToArray()));
        }
    
        //any split char (should be one char between numbers)
        static public int[] StringToIntArray(string st)
        {
            //convert all no numeral val to space
            char[] wchar = st.ToCharArray();
            for (int i = 0; i < wchar.Length; i++)
            {
                if ((int)wchar[i] <= (int)'9' && (int)wchar[i] >= (int)'0')
                {
                }
                else
                {
                    wchar[i] = ' ';
                }
            }

            string wstring = new string(wchar);

            int[] ret = wstring.Split().Select(int.Parse).ToArray();
            return ret;
        }

        static public void GetPermutation<T>(ref List<T[]> ret, T[] arry, int i, int n)
        {
            int j;
            if (i == n)
                ret.Add(new List<T>(arry).ToArray());
            else
            {
                for (j = i; j <= n; j++)
                {
                    Swap(ref arry[i], ref arry[j]);
                    GetPermutation(ref ret, arry, i + 1, n);
                    Swap(ref arry[i], ref arry[j]); //backtrack
                }
            }
        }

        static public void Swap<T>(ref T a, ref T b)
        {
            T tmp;
            tmp = a;
            a = b;
            b = tmp;
        }
    }

    /// <summary>
    /// find the shortest path from start to all destination
    /// works for directed/nondirected graph
    /// </summary>
    public class BellmanFord
    {
        public List<edge> Edge = new List<edge>();
        public int V;   // number of vertex

        public class edge
        {
            public int from, to, cost;
            public edge(int _from, int _to, int _cost)
            {
                from = _from;
                to = _to;
                cost = _cost;
            }
        }

        private int GetTotalPositiveCost()
        {
            int sum = 0;
            foreach (var e in Edge)
            {
                if (e.cost > 0) sum += e.cost;
            }
            return sum;
        }

        private void generateV()
        {
            foreach ( var e in Edge ){
                V = Math.Max(V, e.from);
                V = Math.Max(V, e.to);
            }
            V++;
        }

        /// <summary>
        ///  return shortestPath[V] represents distance from startIndex
        /// </summary>
        public int[] GetShortestPath(int startIndex)
        {
            if (V == 0 && Edge.Count > 0) generateV();

            int[] shortestPath = new int[V];
            int INF = this.GetTotalPositiveCost() + 1;

            for (int i = 0; i < V; i++) shortestPath[i] = INF;

            shortestPath[startIndex] = 0;
            while (true)
            {
                bool update = false;
                foreach (edge e in Edge)
                {
                    if (shortestPath[e.from] != INF && shortestPath[e.to] > shortestPath[e.from] + e.cost)
                    {
                        shortestPath[e.to] = shortestPath[e.from] + e.cost;
                        update = true;
                    }
                }
                if (!update) break;
            }

            return shortestPath;
        }

        /// <summary>
        ///  return true if it has negative close loop
        /// </summary>
        public bool HasNegativeLoop()
        {
            int[] d = new int[V];
            for (int i = 0; i < V; i++)
            {
                foreach (edge e in Edge)
                {
                    if (d[e.to] > d[e.from] + e.cost)
                    {
                        d[e.to] = d[e.from] + e.cost;
                        if (i == V - 1) return true;
                    }
                }
            }
            return false;
        }
    }

    /// <summary>
    /// Get min cost between two points
    /// </summary>
    public class Dijkstra
    {
        private int maxIndex = -1;
        private const int INF = Int32.MaxValue;

        private int[,] _edgeArray;

        /// <summary>
        /// Dijkstra, get min cost between two points
        /// should not contain negatie cost path
        /// </summary>
        /// <param name="size">max index of vertice</param>
        public Dijkstra(int size)
        {
            maxIndex = size + 1;
            _edgeArray = new int[maxIndex, maxIndex];

            for (int i = 0; i < _edgeArray.GetLength(0); i++)
            {
                for (int j = 0; j < _edgeArray.GetLength(1); j++)
                {
                    _edgeArray[i, j] = INF;
                    if (i == j) _edgeArray[i, j] = 0;
                }
            }
        }

        // Add a path(no direction) with its cost
        public void AddPath(int s, int t, int cost)
        {
            _edgeArray[s, t] = Math.Min(_edgeArray[s, t], cost);
            _edgeArray[t, s] = _edgeArray[s, t];
        }

        //Get the min cost between s and t
        //return Int32.MaxValue if no path
        public int GetMinCost(int s, int t)
        {
            int[] cost = new int[maxIndex];
            for (int i = 0; i < cost.Length; i++) cost[i] = INF;
            cost[s] = 0;

            var priorityQueue = new PriorityQueue<ComparablePair<int, int>>(maxIndex);
            priorityQueue.Push( new ComparablePair<int, int>(0, s) );

            while (priorityQueue.Count() > 0)
            {
                var costDestinationPair = priorityQueue.Pop();
                if (cost[costDestinationPair.Item2] < costDestinationPair.Item1) continue;

                for (int i = 0; i < maxIndex; i++)
                {
                    int newCostToi = _edgeArray[costDestinationPair.Item2, i] == INF ? INF :
                        costDestinationPair.Item1 + _edgeArray[costDestinationPair.Item2, i];
                    if (newCostToi < cost[i])
                    {
                        cost[i] = newCostToi;
                        priorityQueue.Push(new ComparablePair<int, int>(newCostToi, i));
                    }
                }
            }

            return cost[t];
        }
    }

    public class ComparablePair<T, U> : IComparable where T : IComparable<T>
    {
        public readonly T Item1;
        public readonly U Item2;

        public int CompareTo(object obj)
        {
            ComparablePair<T, U> castedObj = (ComparablePair<T, U>)obj;
            return this.Item1.CompareTo(castedObj.Item1);
        }

        public ComparablePair(T t, U u)
        {
            Item1 = t;
            Item2 = u;
        }
    }

    public class UnionFind
    {
        private class Node
        {
            public Node Parent { get; set; }
            public int Rank { get; set; }
        }

        private Dictionary<IComparable, Node> _dict = new Dictionary<IComparable, Node>();

        private Node Root(IComparable data)
        {
            if (!_dict.ContainsKey(data))
            {
                var node = new Node();
                _dict.Add(data, node);
                return node;
            }
            else
            {
                var node = _dict[data];
                return Find(node);
            }
        }

        private Node Find(Node node)
        {
            if (node.Parent == null) return node;
            return node.Parent = Find(node.Parent);
        }

        public void Unite(IComparable x, IComparable y)
        {
            var xRoot = Root(x);
            var yRoot = Root(y);
            if (xRoot == yRoot) return;

            if (xRoot.Rank < yRoot.Rank)
            {
                xRoot.Parent = yRoot;
            }
            else
            {
                yRoot.Parent = xRoot;
                if (xRoot.Rank == yRoot.Rank) xRoot.Rank++;
            }
        }

        public bool IsSameGroup(IComparable x, IComparable y)
        {
            return Root(x) == Root(y);
        }
    }

    ///// <summary>
    ///// union find, positive int only
    ///// </summary>
    //public class UnionFind
    //{
    //    private int[] data;

    //    public UnionFind(int size)
    //    {
    //        data = new int[size];
    //        for (int i = 0; i < size; i++) data[i] = -1;
    //    }

    //    public bool Unite(int x, int y)
    //    {
    //        x = Root(x);
    //        y = Root(y);

    //        if (x != y)
    //        {
    //            if (data[y] < data[x])
    //            {
    //                var tmp = y;
    //                y = x;
    //                x = tmp;
    //            }
    //            data[x] += data[y];
    //            data[y] = x;
    //        }
    //        return x != y;
    //    }

    //    public bool IsSameGroup(int x, int y)
    //    {
    //        return Root(x) == Root(y);
    //    }

    //    private int Root(int x)
    //    {
    //        return data[x] < 0 ? x : data[x] = Root(data[x]);
    //    }
    //}

    /// <summary>
    /// Vertices index should be positive number
    /// </summary>
    public class Kruskal
    {
        SortedSet<ComparablePair<int, Tuple<int, int>>> pathSet = new SortedSet<ComparablePair<int, Tuple<int, int>>>();

        private int _maxIndex = 1;
        public Kruskal(int maxIndex)
        {
            _maxIndex = maxIndex;
        }

        public void AddPath(int s, int t, int cost)
        {
            var tuple = new Tuple<int, int>(s, t);
            pathSet.Add(new ComparablePair<int, Tuple<int, int>>(cost, tuple));
        }

        public int GetCost()
        {
            int ret = 0;
            var unionFind = new UnionFind();

            foreach (var path in pathSet)
            {
                int cost = path.Item1;
                int vertex1 = path.Item2.Item1;
                int vertex2 = path.Item2.Item2;

                if (!unionFind.IsSameGroup(vertex1, vertex2))
                {
                    unionFind.Unite(vertex1, vertex2);
                    ret += cost;
                }
            }

            return ret;
        }
    }

    /// <summary>
    /// get max flow from s to t
    /// </summary>
    public class FordFulkerson
    {
        class Edge { public int to; public int cap; public int rev;};

        List<Edge>[] G;
        bool[] used;

        public FordFulkerson(int size)
        {
            G = new List<Edge>[size];
            for (int i = 0; i < size; i++)
            {
                G[i] = new List<Edge>();
            }
            used = new bool[size];
        }

        public void AddEdge(int f, int t, int cap)
        {
            G[f].Add(new Edge() { to = t, cap = cap, rev = G[t].Count() });
            G[t].Add(new Edge() { to = f, cap = 0, rev = G[f].Count() - 1 });
        }

        private int dfs(int v, int t, int f)
        {
            if (v == t) return f;
            used[v] = true;
            for (int i = 0; i < G[v].Count(); i++)
            {
                var e = G[v][i];
                if (!used[e.to] && e.cap > 0)
                {
                    var d = dfs(e.to, t, Math.Min(f, e.cap));
                    if (d > 0)
                    {
                        e.cap -= d;
                        var tgt = G[e.to].ElementAt(e.rev);
                        tgt.cap += d;
                        return d;
                    }
                }
            }
            return 0;
        }

        public int GetMaxFlow(int s, int t)
        {
            int flow = 0;
            while (true)
            {
                for (int i = 0; i < used.Length; i++) used[i] = false;
                int f = dfs(s, t, Int32.MaxValue);
                if (f == 0)
                {
                    return flow;
                }
                flow += f;
            }
        }
    }

    /// <summary>
    /// get max matching between left side nodes and right side nodes
    /// </summary>
    public class BipartiteMatching
    {
        FordFulkerson fordFulkerson;
        HashSet<int> startVertice = new HashSet<int>();
        HashSet<int> targetVertice = new HashSet<int>();
        int maxIndex = -1;

        public BipartiteMatching(int size)
        {
            fordFulkerson = new FordFulkerson(size + 2);
            maxIndex = size + 1;
        }

        public void AddPair(int leftSide, int rightSide)
        {
            fordFulkerson.AddEdge(leftSide + 1, rightSide + 1, 1);

            startVertice.Add(leftSide + 1);
            targetVertice.Add(rightSide + 1);
        }

        public int GetMaxMatchingNum()
        {
            int start = 0;
            int target = maxIndex;

            foreach (var s in startVertice)
            {
                fordFulkerson.AddEdge(start, s, 1);
            }
            foreach (var t in targetVertice)
            {
                fordFulkerson.AddEdge(t, target, 1);
            }

            return fordFulkerson.GetMaxFlow(start, target);
        }
    }
}
