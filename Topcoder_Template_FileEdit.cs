$BEGINCUT$
$PROBLEMDESC$
$ENDCUT$
using System;
using System.Text;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Linq;

    #region My Libraries

    public class PriorityQueue<T> where T : IComparable
    {
        #region member
        SortedList<T, int> sortedList;
        int _count;
        int order_type = 0;
        #endregion

        public int Count
        {
            get { return _count; }
        }
        public PriorityQueue()
        {
            _count = 0;

            //force ascii sorting
            //if (typeof(T) == typeof(string))    //cannot use it for TopCoder
            if (typeof(T).ToString()=="System.String")
            {
                //Console.WriteLine(typeof(T).ToString());

                SortedList<string, int> wk = new SortedList<string, int>(StringComparer.Ordinal);
                sortedList = wk as SortedList<T, int>;
            }
            else
            {
                sortedList = new SortedList<T, int>();
            }

        }
        //type 0: dec, 1: asc
        public PriorityQueue(int type)
            : this()
        {
            if (type == 1) order_type = 1;
        }
        public PriorityQueue(IEnumerable<T> enumerable)
            : this()
        {
            foreach (var r in enumerable)
            {
                this.Enqueue(r);
            }
        }
        public PriorityQueue(int type, IEnumerable<T> enumerable)
            : this(enumerable)
        {
            if (type == 1) order_type = 1;
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

            if (order_type == 0)
            {
                return sortedList.Keys[0];
            }
            else
            {
                return sortedList.Keys[sortedList.Count - 1];
            }
        }
        public IEnumerator<T> GetEnumerator()
        {
            IEnumerable<KeyValuePair<T, int>> workSortedList = sortedList;
            if (order_type != 0) workSortedList = workSortedList.Reverse();

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

    // find the shortest path from start to all destination
    // works for directed/nondirected graph
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

    // Get min cost between two points (Should not contain negative cost)
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

    //has bug, dont use it
    public class UnionFind<T>
    {
        Dictionary<T, int> dic = new Dictionary<T, int>();
        int _key = 0;

        public bool Add(T parameter)
        {
            if (dic.ContainsKey(parameter))
            {
                //do nothing (error)
                return false;
            }
            else
            {
                dic.Add(parameter, _key++);
                return true;
            }
        }

        public bool Unite(T parameter1, T parameter2)
        {
            if (!dic.ContainsKey(parameter1) || !dic.ContainsKey(parameter2))
            {
                return false;
            }
            else
            {
                dic[parameter2] = dic[parameter1] = Math.Min(dic[parameter2], dic[parameter1]);
                return true;
            }
        }

        public bool IsSameGroup(T parameter1, T parameter2)
        {
            if (!dic.ContainsKey(parameter1) || !dic.ContainsKey(parameter2))
            {
                return false;
            }
            else
            {
                return dic[parameter2] == dic[parameter1];
            }
        }
    }

    public class Kruskal
    {
        List<ComparablePair<int, Tuple<int, int>>> list = new List<ComparablePair<int, Tuple<int, int>>>();

        public void AddPath(int s, int t, int cost)
        {
            var tuple = new Tuple<int, int>(s, t);
            list.Add(new ComparablePair<int, Tuple<int, int>>(cost, tuple));
        }

        public int GetCost()
        {
            int ret = 0;
            var unionFind = new UnionFind<int>();
            list.Sort();

            foreach (var path in list)
            {
                int cost = path.Item1;
                int vertex1 = path.Item2.Item1;
                int vertex2 = path.Item2.Item2;

                if (!unionFind.IsSameGroup(vertex1, vertex2))
                {
                    unionFind.Add(vertex1);
                    unionFind.Add(vertex2);
                    unionFind.Unite(vertex1, vertex2);
                    ret += cost;
                }
            }

            return ret;
        }
    }

    //get max flow from s to t
    public class FordFulkerson
    {
        Dictionary<Tuple<int, int>, int> dic = new Dictionary<Tuple<int, int>, int>();  //edge s to t, and its cost
        int[,] edgeArray;
        bool[] usedFlg;
        int maxIdx = -1;

        public void AddEdge(int s, int t, int flow)
        {
            var tuple = new Tuple<int, int>(s, t);

            if (dic.ContainsKey(tuple))
            {
                dic[tuple] = dic[tuple] + flow;
            }
            else
            {
                dic.Add(tuple, flow);
            }

            maxIdx = Math.Max(Math.Max(maxIdx, s), t);
        }

        private void getEdgeArray()
        {
            edgeArray = new int[maxIdx + 1, maxIdx + 1];
            foreach (var edge in dic)
            {
                edgeArray[edge.Key.Item1, edge.Key.Item2] = edge.Value;
            }
        }

        private int getMaxFlow(int s, int t, int f)
        {
            if (s == t) return f;
            usedFlg[s] = true;

            for (int nextVertex = 0; nextVertex < maxIdx + 1; nextVertex++)
            {
                int capacity = edgeArray[s, nextVertex];
                if (usedFlg[nextVertex] == false && capacity > 0)
                {
                    int usedFlowCapacity = getMaxFlow(nextVertex, t, Math.Min(capacity, f));
                    if (usedFlowCapacity > 0)
                    {
                        edgeArray[s, nextVertex] -= usedFlowCapacity;
                        edgeArray[nextVertex, s] += usedFlowCapacity;
                        return usedFlowCapacity;
                    }
                }
            }
            return 0;
        }

        public int GetMaxFlow(int s, int t)
        {
            if (maxIdx == -1) return 0; //no path information

            int flow = 0;
            getEdgeArray();

            while (true)
            {
                usedFlg = new bool[maxIdx + 1];

                int f = getMaxFlow(s, t, Int32.MaxValue);
                if (f == 0) return flow;
                flow += f;
            }
        }

    }

    public class BipartiteMatching
    {
        FordFulkerson fordFulkerson = new FordFulkerson();
        HashSet<int> startVertice = new HashSet<int>();
        HashSet<int> targetVertice = new HashSet<int>();
        int maxIndex = -1;

        public void AddPair(int leftSide, int rightSide)
        {
            fordFulkerson.AddEdge(leftSide + 1, rightSide, 1);

            startVertice.Add(leftSide + 1);
            targetVertice.Add(rightSide);

            maxIndex = Math.Max(leftSide + 1, Math.Max(rightSide, maxIndex));
        }

        public int GetMatchingNum()
        {
            int start = 0;
            int target = maxIndex + 1;

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

public class $CLASSNAME$ {
    public $RC$ $METHODNAME$($METHODPARMS$) {
        $RC$ res;
        return res;
    }
$WRITERCODE$
$BEGINCUT$
    public static void Main(String[] args) {
        try  {
$MAINBODY$
               Console.ReadLine();
        } 
        catch( Exception exx)  {
            System.Console.WriteLine(exx);
            System.Console.WriteLine( exx.StackTrace);
        }
    }
    private static void eq( int n, object have, object need) {
        if( eq( have, need ) ) {
            Console.WriteLine( "Case "+n+" passed." );
        } else {
            Console.Write( "Case "+n+" failed: expected " );
            print( need );
            Console.Write( ", received " );
            print( have );
            Console.WriteLine();
        }
    }
    private static void eq( int n, Array have, Array need) {
        if( have == null || have.Length != need.Length ) {
            Console.WriteLine("Case "+n+" failed: returned "+have.Length+" elements; expected "+need.Length+" elements.");
            print( have );
            print( need );
            return;
        }
        for( int i= 0; i < have.Length; i++ ) {
            if( ! eq( have.GetValue(i), need.GetValue(i) ) ) {
                Console.WriteLine( "Case "+n+" failed. Expected and returned array differ in position "+i );
                print( have );
                print( need );
                return;
            }
        }
        Console.WriteLine("Case "+n+" passed.");
    }
    private static bool eq( object a, object b ) {
        if ( a is double && b is double ) {
            return Math.Abs((double)a-(double)b) < 1E-9;
        } else {
            return a!=null && b!=null && a.Equals(b);
        }
    }
    private static void print( object a ) {
        if ( a is string ) {
            Console.Write("\"{0}\"", a);
        } else if ( a is long ) {
            Console.Write("{0}L", a);
        } else {
            Console.Write(a);
        }
    }
    private static void print( Array a ) {
        if ( a == null) {
            Console.WriteLine("<NULL>");
        }
        Console.Write('{');
        for ( int i= 0; i < a.Length; i++ ) {
            print( a.GetValue(i) );
            if( i != a.Length-1 ) {
                Console.Write(", ");
            }
        }
        Console.WriteLine( '}' );
    }
$ENDCUT$
}