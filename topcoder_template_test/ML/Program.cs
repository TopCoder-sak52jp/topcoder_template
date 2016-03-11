using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using topcoder_template_test;

namespace ML
{
    class Program
    {
        static void LinearTest_Example()
        {
            #region data
            var data = new List<int[]>(){
                new int[]{2104,3,399900},
                new int[] {1600,3,329900},
                new int[] {2400,3,369000},
                new int[] {1416,2,232000},
                new int[] {3000,4,539900},
                new int[] {1985,4,299900},
                new int[] {1534,3,314900},
                new int[] {1427,3,198999},
                new int[] {1380,3,212000},
                new int[] {1494,3,242500},
                new int[] {1940,4,239999},
                new int[] {2000,3,347000},
                new int[] {1890,3,329999},
                new int[] {4478,5,699900},
                new int[] {1268,3,259900},
                new int[] {2300,4,449900},
                new int[] {1320,2,299900},
                new int[] {1236,3,199900},
                new int[] {2609,4,499998},
                new int[] {3031,4,599000},
                new int[] {1767,3,252900},
                new int[] {1888,2,255000},
                new int[] {1604,3,242900},
                new int[] {1962,4,259900},
                new int[] {3890,3,573900},
                new int[] {1100,3,249900},
                new int[] {1458,3,464500},
                new int[] {2526,3,469000},
                new int[] {2200,3,475000},
                new int[] {2637,3,299900},
                new int[] {1839,2,349900},
                new int[] {1000,1,169900},
                new int[] {2040,4,314900},
                new int[] {3137,3,579900},
                new int[] {1811,4,285900},
                new int[] {1437,3,249900},
                new int[] {1239,3,229900},
                new int[] {2132,4,345000},
                new int[] {4215,4,549000},
                new int[] {2162,4,287000},
                new int[] {1664,2,368500},
                new int[] {2238,3,329900},
                new int[] {2567,4,314000},
                new int[] {1200,3,299000},
                new int[] {852,2,179900},
                new int[] {1852,4,299900},
                new int[] {1203,3,239500}};
            #endregion

            Console.WriteLine("**Linear Regression from course data**");

            Console.WriteLine("y = 89597 + 139x1 - 8378x2");
            var xar = new double[47, 3];
            var yar = new double[47, 1];
            for (int i = 0; i < data.Count(); i++)
            {
                xar[i, 0] = 1;
                xar[i, 1] = data[i][0];
                xar[i, 2] = data[i][1];
                yar[i, 0] = data[i][2];
            }
            var x = new Matrix(xar);

            var xnrm = x.NormalizeFeature().Item1;

            var y = new Matrix(yar);
            var ret = LinearRegression.GetResult_GradientDescent(xnrm, y, 0.01, 1, 400);
            Console.WriteLine("  gradient descent:\t" + Enumerable.Range(0, ret.Item1.RowNum).Select(i => string.Format("{0}", ret.Item1[i, 0])).Aggregate((a, b) => a + " " + b));
            Console.WriteLine("     - should be close to 334302, 100087, 3673");
            ret = LinearRegression.GetResult_NormalEquation(x, y, 0);
            Console.WriteLine("  normal equation:\t" + Enumerable.Range(0, ret.Item1.RowNum).Select(i => string.Format("{0}", ret.Item1[i, 0])).Aggregate((a, b) => a + " " + b));
            Console.WriteLine("     - should be close to 89597, 139, 8378");
        }

        static void LogisticTest_Example()
        {
            #region data
            var data = @"0.051267,0.69956,1
-0.092742,0.68494,1
-0.21371,0.69225,1
-0.375,0.50219,1
-0.51325,0.46564,1
-0.52477,0.2098,1
-0.39804,0.034357,1
-0.30588,-0.19225,1
0.016705,-0.40424,1
0.13191,-0.51389,1
0.38537,-0.56506,1
0.52938,-0.5212,1
0.63882,-0.24342,1
0.73675,-0.18494,1
0.54666,0.48757,1
0.322,0.5826,1
0.16647,0.53874,1
-0.046659,0.81652,1
-0.17339,0.69956,1
-0.47869,0.63377,1
-0.60541,0.59722,1
-0.62846,0.33406,1
-0.59389,0.005117,1
-0.42108,-0.27266,1
-0.11578,-0.39693,1
0.20104,-0.60161,1
0.46601,-0.53582,1
0.67339,-0.53582,1
-0.13882,0.54605,1
-0.29435,0.77997,1
-0.26555,0.96272,1
-0.16187,0.8019,1
-0.17339,0.64839,1
-0.28283,0.47295,1
-0.36348,0.31213,1
-0.30012,0.027047,1
-0.23675,-0.21418,1
-0.06394,-0.18494,1
0.062788,-0.16301,1
0.22984,-0.41155,1
0.2932,-0.2288,1
0.48329,-0.18494,1
0.64459,-0.14108,1
0.46025,0.012427,1
0.6273,0.15863,1
0.57546,0.26827,1
0.72523,0.44371,1
0.22408,0.52412,1
0.44297,0.67032,1
0.322,0.69225,1
0.13767,0.57529,1
-0.0063364,0.39985,1
-0.092742,0.55336,1
-0.20795,0.35599,1
-0.20795,0.17325,1
-0.43836,0.21711,1
-0.21947,-0.016813,1
-0.13882,-0.27266,1
0.18376,0.93348,0
0.22408,0.77997,0
0.29896,0.61915,0
0.50634,0.75804,0
0.61578,0.7288,0
0.60426,0.59722,0
0.76555,0.50219,0
0.92684,0.3633,0
0.82316,0.27558,0
0.96141,0.085526,0
0.93836,0.012427,0
0.86348,-0.082602,0
0.89804,-0.20687,0
0.85196,-0.36769,0
0.82892,-0.5212,0
0.79435,-0.55775,0
0.59274,-0.7405,0
0.51786,-0.5943,0
0.46601,-0.41886,0
0.35081,-0.57968,0
0.28744,-0.76974,0
0.085829,-0.75512,0
0.14919,-0.57968,0
-0.13306,-0.4481,0
-0.40956,-0.41155,0
-0.39228,-0.25804,0
-0.74366,-0.25804,0
-0.69758,0.041667,0
-0.75518,0.2902,0
-0.69758,0.68494,0
-0.4038,0.70687,0
-0.38076,0.91886,0
-0.50749,0.90424,0
-0.54781,0.70687,0
0.10311,0.77997,0
0.057028,0.91886,0
-0.10426,0.99196,0
-0.081221,1.1089,0
0.28744,1.087,0
0.39689,0.82383,0
0.63882,0.88962,0
0.82316,0.66301,0
0.67339,0.64108,0
1.0709,0.10015,0
-0.046659,-0.57968,0
-0.23675,-0.63816,0
-0.15035,-0.36769,0
-0.49021,-0.3019,0
-0.46717,-0.13377,0
-0.28859,-0.060673,0
-0.61118,-0.067982,0
-0.66302,-0.21418,0
-0.59965,-0.41886,0
-0.72638,-0.082602,0
-0.83007,0.31213,0
-0.72062,0.53874,0
-0.59389,0.49488,0
-0.48445,0.99927,0
-0.0063364,0.99927,0
0.63265,-0.030612,0";
            #endregion

            var dataLines = data.Split('\n');

            Console.WriteLine("**Logistic Regression from course data**");

            var xar = new double[dataLines.Length, 2];
            var yar = new double[dataLines.Length, 1];
            for (int i = 0; i < dataLines.Count(); i++)
            {
                var line = dataLines[i].Split(',');
                xar[i, 0] = double.Parse(line[0]);
                xar[i, 1] = double.Parse(line[1]);
                yar[i, 0] = double.Parse(line[2]);
            }
            var x = new Matrix(xar);
            var y = new Matrix(yar);

            x = MapFeatures(x);

            var ret = LogisticRegression.GetResult(x, y, 1, 1, 400);
            Console.WriteLine("  result:\t" + Enumerable.Range(0, ret.Item1.RowNum).Select(i => string.Format("{0}", ret.Item1[i, 0])).Aggregate((a, b) => a + " " + b));
            Console.WriteLine("    - should be close to 1.27, 0.62, 1.1, -2.0, -0.9, ...");
        }

        static Matrix MapFeatures(Matrix x)
        {
            var ret = new Matrix(x.RowNum, 28);

            for (int row = 0; row < x.RowNum; row++) ret[row, 0] = 1.0;

            var eidx = 1;
            var degree = 6;
            for (int i = 1; i <= degree; i++)
            {
                for (int j = 0; j <= i; j++)
                {
                    for (int row = 0; row < x.RowNum; row++)
                    {
                        var x1 = x[row, 0];
                        var x2 = x[row, 1];

                        ret[row, eidx] = Math.Pow(x1, (double)(i - j)) * Math.Pow(x2, j);
                    }

                    eidx++;
                }
            }

            return ret;
        }

        static void NeuralNetworkTest()
        {
            Console.WriteLine("**Neural Network**");
            var nn = new NeuralNetwork(3, new int[] { 2, 2, 1 });

            var input = new Matrix(2, 1);
            input[0, 0] = 0;
            input[1, 0] = 1;

            var output = new Matrix(1, 1);
            output[0, 0] = 0;

            nn.Learn(new Matrix[]{ input }, new Matrix[]{ output }, 0.1, 0.1, 1);


        }

        static void Main(string[] args)
        {
            //LinearTest_Example();

            //Console.WriteLine();
            //Console.WriteLine();

            //LogisticTest_Example();

            //Console.WriteLine();
            //Console.WriteLine();

            NeuralNetworkTest();

            Console.ReadLine();
        }
    }
}
