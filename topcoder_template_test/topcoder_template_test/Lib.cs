using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace topcoder_template_test
{
    #region My Libraries

    public class PriorityQueue<T> where T : IComparable
    {
        class DescComparer<T> : IComparer<T>
        {
            public int Compare(T x, T y)
            {
                return Comparer<T>.Default.Compare(y, x);
            }
        }

        class StrReverseComparer : IComparer<string>
        {
            public int Compare(string x, string y)
            {
                return -x.CompareTo(y);
            }
        }

        #region member
        SortedList<T, int> sortedList;
        int _count;
        #endregion

        public int Count
        {
            get { return _count; }
        }

        /// <summary>
        /// sort type=0: asc, 1:desc
        /// </summary>
        /// <param name="sortType"></param>
        public PriorityQueue(int sortType=0)
        {
            _count = 0;

            var comparare = sortType == 0 ? (IComparer<T>)Comparer<T>.Default : new DescComparer<T>();

            //force ascii sorting
            //if (typeof(T) == typeof(string))    //cannot use it for TopCoder
            if (typeof(T).ToString()=="System.String")
            {
                //Console.WriteLine(typeof(T).ToString());

                SortedList<string, int> wk = sortType == 0 ? new SortedList<string, int>(StringComparer.Ordinal) :
                    new SortedList<string, int>(new StrReverseComparer());
                sortedList = wk as SortedList<T, int>;
            }
            else
            {
                sortedList = new SortedList<T, int>(comparare);
            }

        }
        public PriorityQueue(IEnumerable<T> enumerable, int sortType=0)
            : this(sortType)
        {
            foreach (var r in enumerable)
            {
                this.Enqueue(r);
            }
        }
        public void Enqueue(T parameter)
        {
            if (sortedList.ContainsKey(parameter))
            {
                sortedList[parameter]++;
            }
            else
            {
                sortedList.Add(parameter, 1);
            }
            _count++;
        }
        public T Dequeue()
        {
            T returnValue;
            returnValue = this.Peek();

            sortedList[returnValue]--;

            if (sortedList[returnValue] == 0)
            {
                sortedList.Remove(returnValue);
            }

            _count--;

            return returnValue;
        }
        public void Clear()
        {
            sortedList.Clear();
            _count = 0;
        }
        public bool Contains(T parameter)
        {
            return sortedList.ContainsKey(parameter);
        }
        public T Peek()
        {
            if (this.Count == 0)
            {
                //throw new InvalidOperationException();    //cannot use for TopCoder
                
                Console.WriteLine("Invalid Operation Exception");
                return default(T);
            }

            return sortedList.Keys[0];
        }
        public IEnumerator<T> GetEnumerator()
        {
            IEnumerable<KeyValuePair<T, int>> workSortedList = sortedList;
            foreach (var r in workSortedList)
            {
                int num = r.Value;
                for (int i = 0; i < num; i++)
                {
                    yield return r.Key;
                }
            }
        }
        public T[] ToArray()
        {
            T[] array = new T[Count];
            int i = 0;

            foreach (var r in this)
            {
                array[i++] = r;
            }

            return array;
        }
    }

    //not finished
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
    /// Get min cost between two points (Should not contain negative cost)
    /// </summary>
    public class Dijkstra
    {
        Dictionary<Tuple<int, int>, int> dic = new Dictionary<Tuple<int, int>, int>();
        int maxIndex = -1;
        const int INF = Int32.MaxValue;

        // Add a path with its cost
        public void AddPath(int s, int t, int cost)
        {
            var tuple = new Tuple<int, int>(s, t);
            if (dic.ContainsKey(tuple))
            {
                dic[tuple] = Math.Min(dic[tuple], cost); //Choose cheaper path
            }
            else
            {
                dic.Add(tuple, cost);
            }

            maxIndex = Math.Max(maxIndex, s);
            maxIndex = Math.Max(maxIndex, t);
        }

        private int[,] getEdgeArray()
        {
            var edgeArray = new int[maxIndex+1, maxIndex+1];
            for (int i = 0; i < edgeArray.GetLength(0); i++)
            {
                for (int j = 0; j < edgeArray.GetLength(1); j++)
                {
                    edgeArray[i, j] = INF;
                    if (i == j) edgeArray[i, j] = 0;
                }
            }

            foreach (var r in dic)
            {
                edgeArray[r.Key.Item1, r.Key.Item2] = r.Value;
                edgeArray[r.Key.Item2, r.Key.Item1] = r.Value;
            }

            return edgeArray;
        }

        //Get the min cost between s and t
        //return Int32.MaxValue if no path
        public int GetMinCost(int s, int t)
        {
            // convert edge data as array
            int[,] edgeArray = getEdgeArray();
            int[] cost = new int[maxIndex + 1];
            for (int i = 0; i < cost.Length; i++) cost[i] = INF;
            cost[s] = 0;

            var priorityQueue = new PriorityQueue< ComparablePair<int, int> >();
            priorityQueue.Enqueue( new ComparablePair<int, int>(0, s) );

            while (priorityQueue.Count > 0)
            {
                var costDestinationPair = priorityQueue.Dequeue();
                if (cost[costDestinationPair.Item2] < costDestinationPair.Item1) continue;

                for (int i = 0; i < maxIndex + 1; i++)
                {
                    int newCostToi = edgeArray[costDestinationPair.Item2, i] == INF ? INF : 
                        costDestinationPair.Item1 + edgeArray[costDestinationPair.Item2, i];
                    if (newCostToi < cost[i])
                    {
                        cost[i] = newCostToi;
                        priorityQueue.Enqueue(new ComparablePair<int, int>(newCostToi, i));
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

    /// <summary>
    /// union find, positive int only
    /// </summary>
    public class UnionFind
    {
        private int[] data;

        public UnionFind(int size)
        {
            data = new int[size];
            for (int i = 0; i < size; i++) data[i] = -1;
        }

        public bool Unite(int x, int y)
        {
            x = Root(x);
            y = Root(y);

            if (x != y)
            {
                if (data[y] < data[x])
                {
                    var tmp = y;
                    y = x;
                    x = tmp;
                }
                data[x] += data[y];
                data[y] = x;
            }
            return x != y;
        }

        public bool IsSameGroup(int x, int y)
        {
            return Root(x) == Root(y);
        }

        private int Root(int x)
        {
            return data[x] < 0 ? x : data[x] = Root(data[x]);
        }
    }

    /// <summary>
    /// Vertice index should be positive number
    /// </summary>
    public class Kruskal
    {
        SortedSet<ComparablePair<int, Tuple<int, int>>> pathSet = new SortedSet<ComparablePair<int, Tuple<int, int>>>();

        private int maxVert = 1;
        public void AddPath(int s, int t, int cost)
        {
            maxVert = Math.Max(maxVert, s);
            maxVert = Math.Max(maxVert, t);

            var tuple = new Tuple<int, int>(s, t);
            pathSet.Add(new ComparablePair<int, Tuple<int, int>>(cost, tuple));
        }

        public int GetCost()
        {
            int ret = 0;
            var unionFind = new UnionFind(maxVert + 1);

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

    #endregion
}
