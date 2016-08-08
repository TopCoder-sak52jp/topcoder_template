using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace topcoder_template_test
{
    static public class StringUtil
    {
        //return true if t contains s
        public static bool RollingHash(string s, string t)
        {
            const UInt64 B = 1000000007;
            if (s.Length > t.Length) return false;

            UInt64 Bpowl = 1;
            for (int i = 0; i < s.Length; i++) Bpowl *= B;

            UInt64 hs = 0;
            for (int i = 0; i < s.Length; i++) hs = hs * B + (s[i]);

            UInt64 ht = 0;
            for (int i = 0; i < s.Length; i++) ht = ht * B + (t[i]);

            if (hs == ht) return true;

            for (int i = s.Length; i < t.Length; i++)
            {
                ht = ht * B + t[i] - t[i - s.Length] * Bpowl;
                if (hs == ht) return true;
            }

            return false;
        }

        public static int[] Z_Algorithm(string str)
        {
            var ret = new int[str.Length];
            ret[0] = str.Length;

            var j = 0;
            for (int i = 1; i < str.Length; )
            {
                while (i + j < str.Length && str[j] == str[i + j]) j++;
                ret[i] = j;

                if (j == 0)
                {
                    i++;
                    continue;
                }

                var k = 1;
                while (i + k < str.Length && k + ret[k] < j)
                {
                    ret[i + k] = ret[k];
                    k++;
                }

                i += k;
                j -= k; //jを0まで戻す必要はない。[0,j-k)はすでに比較し、一致することが分かっている！
            }

            return ret;
        }
    }

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
        public PriorityQueue(IComparer<T> comparer)
        {
            _heap = new T[128];
            _comparer = comparer;
        }

        /// <summary>
        /// Priority queue
        /// </summary>
        /// <param name="type">0: asc, 1:desc</param>
        public PriorityQueue(int type = 0)
        {
            _heap = new T[128];
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
            if (_count > _heap.Length)
            {
                var newheap = new T[_heap.Length * 2];
                for (int n = 0; n < _heap.Length; n++) newheap[n] = _heap[n];
                _heap = newheap;
            }

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

    static public class MyLib
    {
        public static List<T> TopologicalSort<T>(List<T> list, List<List<int>> graph)
        {
            var ret = new List<int>();

            var inEdgeNum = new int[list.Count()];
            for (int i = 0; i < graph.Count(); i++)
                foreach (var e in graph[i]) inEdgeNum[e]++;

            var que = new PriorityQueue<int>();
            for (int i = 0; i < list.Count(); i++)
                if (inEdgeNum[i] == 0) que.Push(i);

            while (que.Count() > 0)
            {
                var val = que.Pop();
                ret.Add(val);

                foreach (var e in graph[val])
                {
                    inEdgeNum[e]--;
                    if (inEdgeNum[e] == 0) que.Push(e);
                }
            }

            if (ret.Count() != list.Count()) return null;

            var retVal = new List<T>();
            foreach (var r in ret)
            {
                retVal.Add(list[r]);
            }
            return retVal;
        }

        public static int LowerBound(int[] ar, int val)
        {
            var lb = -1;
            var ub = ar.Length;

            while (ub - lb > 1)
            {
                var mid = (lb + ub) / 2;
                if (ar[mid] >= val)
                {
                    ub = mid;
                }
                else
                {
                    lb = mid;
                }
            }

            return ub;
        }

        public static int UpperBound(int[] ar, int val)
        {
            var lb = -1;
            var ub = ar.Length;

            while (ub - lb > 1)
            {
                var mid = (lb + ub) / 2;
                if (ar[mid] <= val)
                {
                    lb = mid;
                }
                else
                {
                    ub = mid;
                }
            }

            return lb + 1;
        }

        static Int64 ModInv(Int64 a, Int64 m)
        {
            Int64 x = 0, y = 0;
            ExtGcd(a, m, ref x, ref y);
            if (x < 0) x += m; //modInv will never be negative
            return x;
        }

        static Int64[] fact = new Int64[500005];
        static Int64[] inv = new Int64[500005];

        static void Precal_FactAndInv(Int64 mod)
        {
            fact[0] = 1;
            inv[0] = ModInv(1, mod);

            for (Int64 i = 1; i < 500005; i++)
            {
                fact[i] = (fact[i - 1] * i) % mod;
                inv[i] = ModInv(fact[i], mod);
            }
        }

        static Int64 Nck(int n, int k, Int64 mod)
        {
            return fact[n] * inv[n - k] % mod * inv[k] % mod;
        }

        static public Int64 ModPow(Int64 x, Int64 n, Int64 mod)
        {
            Int64 ret = 1;
            while (n > 0)
            {
                if ((n & 1) == 1) ret = ret * x % mod;
                x = x * x % mod;
                n >>= 1;
            }
            return ret;
        }

        //等差数列の和を逆元なしで求める。O(Logn)。
        static public Int64 ModPowSum(Int64 r, Int64 n, Int64 mod)
        {
            if (n == 0) return 0;

            //nが奇数：1 + r + ... + r^(n-1) = 1 + r(1 + r + ... + r^(n-2))
            if (n % 2 == 1) return (ModPowSum(r, n - 1, mod) * r + 1) % mod;

            //nが偶数：1 + r + ... + r^(n-1) = ( 1 + r + ... + r^(n/2-1)) +  r^(n/2) x ( 1 + r + ... + r^(n/2-1))
            Int64 result = ModPowSum(r, n / 2, mod);
            return (result * ModPow(r, n / 2, mod) + result) % mod;
        }

        static public void Sieve(int[] prime, bool[] isPrime)
        {
            for (int i = 0; i < prime.Length; i++) prime[i] = -1;
            for (int i = 0; i < isPrime.Length; i++) isPrime[i] = true;
            isPrime[0] = isPrime[1] = false;

            var idx = 0;
            for (int i = 2; i < isPrime.Length; i++)
            {
                if (isPrime[i])
                {
                    prime[++idx] = i;
                    for (int j = 2 * i; j < isPrime.Length; j += i) isPrime[j] = false;
                }
            }
        }

        static public Int64 ExtGcd(Int64 a, Int64 b, ref Int64 x, ref Int64 y)
        {
            Int64 d = a;
            if (b != 0)
            {
                d = ExtGcd(b, a % b, ref y, ref x);
                y -= (a / b) * x;
            }
            else
            {
                x = 1;
                y = 0;
            }
            return d;
        }

        static Int64 Gcd(Int64 a, Int64 b)
        {
            if (a < b)
            {
                var tmp = a;
                a = b;
                b = tmp;
            }
            if (b == 0) return a;
            var p = a > b ? a : b;
            return Gcd(b, p % b);
        }

        //素因数分解
        public static Dictionary<UInt64, UInt64> GetFactors(UInt64 n)
        {
            var ret = new Dictionary<UInt64, UInt64>();

            while (n % 2 == 0)
            {
                if (!ret.ContainsKey(2)) ret.Add(2, 0);
                ret[2]++;
                n = n / 2;
            }
            for (UInt64 i = 3; i <= Math.Sqrt(n); i = i + 2)
            {
                while (n % i == 0)
                {
                    if (!ret.ContainsKey(i)) ret.Add(i, 0);
                    ret[i]++;
                    n = n / i;
                }
            }
            if (n > 2)
            {
                if (!ret.ContainsKey(n)) ret.Add(n, 0);
                ret[n]++;
            }

            return ret;
        }

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

        static public void ShuffleList<T>(List<T> list, Random rnd)
        {
            if (list.Count() < 2) return;

            var n = list.Count();
            while (n > 0)
            {
                n--;
                int k = (int)(rnd.Next() % (n + 1));

                var value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
    }

    //強連結成分(Strongly Connected Component)分解
    public class Scc
    {
        public List<List<int>> _graph;
        public List<List<int>> _r_Graph;

        bool[] _used;
        List<int> _vs = new List<int>();

        //属する強連結成分のトポロジカル順序
        public int[] Cmp;

        //各強連結成分に属するノード
        public List<List<int>> Components;

        int _vn;

        public void Init(int vn)
        {
            _vn = vn;

            _graph = new List<List<int>>();
            for (int i = 0; i < vn; i++) _graph.Add(new List<int>());

            _r_Graph = new List<List<int>>();
            for (int i = 0; i < vn; i++) _r_Graph.Add(new List<int>());

            Cmp = new int[vn];
            _used = new bool[vn];
        }

        public void AddEdge(int from, int to)
        {
            _graph[from].Add(to);
            _r_Graph[to].Add(from);
        }

        void Dfs(int v)
        {
            _used[v] = true;
            for (int i = 0; i < _graph[v].Count(); i++)
            {
                if (!_used[_graph[v][i]]) Dfs(_graph[v][i]);
            }
            _vs.Add(v);
        }

        void RDfs(int v, int k, List<int> c)
        {
            c.Add(v);
            _used[v] = true;
            Cmp[v] = k;
            for (int i = 0; i < _r_Graph[v].Count(); i++)
            {
                if (!_used[_r_Graph[v][i]]) RDfs(_graph[v][i], k, c);
            }
        }

        //Cmp[]とComponentsに値を設定し、強連結成分数を返す
        public int GetScc()
        {
            for (int i = 0; i < _used.Length; i++) _used[i] = false;
            _vs.Clear();
            for (int v = 0; v < _vn; v++)
            {
                if (!_used[v]) Dfs(v);
            }

            Components = new List<List<int>>();
            for (int i = 0; i < _used.Length; i++) _used[i] = false;
            int k = 0;
            for (int i = _vs.Count() - 1; i >= 0; i--)
            {
                var c = new List<int>();
                if (!_used[_vs[i]]) RDfs(_vs[i], k++, c);
                Components.Add(c);
            }

            return k;
        }
    }

    /// <summary>
    /// find the shortest path from start to all destination
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

        public BellmanFord(int v)
        {
            V = v;
        }

        /// <summary>
        ///  return shortestPath[V] represents distance from startIndex
        ///  null if there are negative loop from the startIndex
        /// </summary>
        public int[] GetShortestPath(int startIndex)
        {
            int[] shortestPath = new int[V];
            var INF = Int32.MaxValue;

            for (int i = 0; i < V; i++) shortestPath[i] = INF;
            shortestPath[startIndex] = 0;

            var cnt = 0;
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

                cnt++;
                if (cnt == V - 1) return null;
            }

            return shortestPath;
        }

        /// <summary>
        ///  return true if it has any negative close loop
        /// </summary>
        public bool HasAnyNegativeLoop()
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
    /// find the shortest path from start to all destination
    /// </summary>
    public class Dijkstra
    {
        private class Edge
        {
            public int T { get; set; }
            public int C { get; set; }
        }

        int maxNodeIdx = 0;
        private const int INF = Int32.MaxValue;

        private Dictionary<int, List<Edge>> _edgeArray = new Dictionary<int, List<Edge>>();

        /// <summary>
        /// Dijkstra, get min cost between two points
        /// should not contain negatie cost path
        /// </summary>
        public Dijkstra()
        {
        }

        // Add a path with its cost
        public void AddPath(int s, int t, int cost)
        {
            if (!_edgeArray.ContainsKey(s)) _edgeArray.Add(s, new List<Edge>());
            _edgeArray[s].Add(new Edge() { T = t, C = cost });

            maxNodeIdx = Math.Max(Math.Max(maxNodeIdx, s), t);
        }

        //Get the min cost from s
        //return Int32.MaxValue if no path
        public int[] GetMinCost(int s)
        {
            int[] cost = new int[maxNodeIdx + 1];
            for (int i = 0; i < cost.Length; i++) cost[i] = INF;
            cost[s] = 0;

            var priorityQueue = new PriorityQueue<ComparablePair<int, int>>(_edgeArray.Sum(e => e.Value.Count()) + 1);
            priorityQueue.Push(new ComparablePair<int, int>(0, s));

            while (priorityQueue.Count() > 0)
            {
                var costDestinationPair = priorityQueue.Pop();
                if (cost[costDestinationPair.Item2] < costDestinationPair.Item1) continue;
                if (!_edgeArray.ContainsKey(costDestinationPair.Item2)) continue;

                foreach (var edge in _edgeArray[costDestinationPair.Item2])
                {
                    var newCostToi = costDestinationPair.Item1 + edge.C;
                    if (newCostToi < cost[edge.T])
                    {
                        cost[edge.T] = newCostToi;
                        priorityQueue.Push(new ComparablePair<int, int>(newCostToi, edge.T));
                    }
                }
            }

            return cost;
        }
    }

    public class WarshallFloyd
    {
        int[,] dist;
        public int V;

        public WarshallFloyd(int v)
        {
            V = v;
            dist = new int[v, v];

            for (int i = 0; i < V; i++)
            {
                for (int j = 0; j < V; j++)
                {
                    dist[i, j] = Int32.MaxValue;
                    if (i == j) dist[i, j] = 0;
                }
            }
        }

        public void AddEdge(int s, int t, int c)
        {
            dist[s, t] = c;
        }

        public int[,] GetDistances()
        {
            for (int k = 0; k < V; k++)
            {
                for (int i = 0; i < V; i++)
                {
                    for (int j = 0; j < V; j++)
                    {
                        dist[i, j] = Math.Min(dist[i, j], dist[i, k] == Int32.MaxValue || dist[k, j] == Int32.MaxValue ? Int32.MaxValue : dist[i, k] + dist[k, j]);
                    }
                }
            }
            return dist;
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

        private Dictionary<object, Node> _dict = new Dictionary<object, Node>();

        private Node Root(object data)
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

    public class UnionFind_WithSize
    {
        List<int> par;

        public UnionFind_WithSize(int N)
        {
            par = new List<int>(N);
            for (int i = 0; i < N; i++) par.Add(-1);
        }

        public void Init()
        {
            for (int i = 0; i < par.Count; i++) par[i] = -1;
        }

        public int root(int x)
        {
            return par[x] >= 0 ? par[x] = root(par[x]) : x;
        }

        public bool Same(int x, int y)
        {
            return root(x) == root(y);
        }

        public bool Unite(int x, int y)
        {
            x = root(x);
            y = root(y);
            if (x == y) return false;
            if (par[x] > par[y])
            {
                var tmp = y;
                y = x;
                x = tmp;
            }
            par[x] += par[y];
            par[y] = x;
            return true;
        }

        public int Size(int x)
        {
            return -par[root(x)];
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

        public Kruskal()
        {
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

        public FordFulkerson()
        {
            G = new List<Edge>[128];
            for (int i = 0; i < 128; i++)
            {
                G[i] = new List<Edge>();
            }
            used = new bool[128];
        }

        public void AddEdge(int f, int t, int cap)
        {
            while (t + 2 >= G.Length || f + 2 >= G.Length) Expand();

            G[f].Add(new Edge() { to = t, cap = cap, rev = G[t].Count() });
            G[t].Add(new Edge() { to = f, cap = 0, rev = G[f].Count() - 1 });
        }

        void Expand()
        {
            var newG = new List<Edge>[G.Length * 2];
            var newUsed = new bool[used.Length * 2];
            for (int i = 0; i < G.Length; i++)
            {
                newG[i] = G[i];
                newUsed[i] = used[i];
            }
            for (int i = G.Length; i < newG.Length; i++)
            {
                newG[i] = new List<Edge>();
            }
            G = newG;
            used = newUsed;
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
            while (s + 2 >= G.Length || t + 2 >= G.Length) Expand();

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

        public int MaxIdx { get { return G.Length - 2; } }
    }

    /// <summary>
    /// get max matching between left side nodes and right side nodes
    /// </summary>
    public class BipartiteMatching
    {
        FordFulkerson fordFulkerson;
        HashSet<int> startVertice = new HashSet<int>();
        HashSet<int> targetVertice = new HashSet<int>();

        public BipartiteMatching()
        {
            fordFulkerson = new FordFulkerson();
        }

        public void AddPair(int leftSide, int rightSide)
        {
            fordFulkerson.AddEdge(leftSide, rightSide, 1);

            startVertice.Add(leftSide);
            targetVertice.Add(rightSide);
        }

        public int GetMaxMatchingNum()
        {
            int start = fordFulkerson.MaxIdx;
            int target = start + 1;

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
