using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace topcoder_template_test
{
    public static class LogisticRegression
    {
        public static Tuple<Matrix, double[]> GetResult(Matrix x, Matrix y, double alpha, double lambda, int maxItr, bool logCosts = false)
        {
            var theta = new Matrix(x.ColNum, 1);
            var costs = new List<double>();
            while (maxItr > 0)
            {
                var h = Matrix.GetSigmoid(x * theta);
                var diff = h - y;

                if (logCosts)
                {
                    var wktheta = theta.Clone();
                    wktheta[0, 0] = 0;
                    var j = ( 1.0 / (double)x.RowNum ) *
                            ( ( -1.0 * (y.Transpose() * h.Log())) - (1.0 - y).Transpose() * (1.0 - h).Log())
                            + ( lambda / ( 2.0 * (double)x.RowNum) ) * (wktheta^2).Sum();
                    costs.Add(j[0, 0]);
                }

                var grad = ((alpha / (double)x.RowNum) * (diff.Transpose() * x).Transpose()) + ((alpha * lambda) / (double)x.RowNum) * theta;
                grad[0, 0] -= ((alpha * lambda) / (double)x.RowNum) * theta[0, 0];

                theta -= grad;

                maxItr--;
            }

            return new Tuple<Matrix, double[]>(theta, costs.ToArray());
        }
    }

    public static class LinearRegression
    {
        public static Tuple<Matrix, double[]> GetResult_NormalEquation(Matrix x, Matrix y, double lambda, bool logCosts = false)
        {
            var xT = x.Transpose();
            var l = Matrix.Eye(x.ColNum);
            l[0, 0] = 0;

            var ret = (xT * x + lambda * l).Inverse() * xT * y;

            var costs = new List<double>();
            if (logCosts)
            {
                var h = x * ret;
                var j = ((1.0 / (2.0 * (double)x.RowNum)) * ((h - y) ^ 2)).Sum();
                costs.Add(j);
            }

            return new Tuple<Matrix,double[]>(ret, costs.ToArray());
        }

        public static Tuple<Matrix, double[]> GetResult_GradientDescent(Matrix x, Matrix y, double alpha, double lambda, int maxItr, bool logCosts=true)
        {
            var theta = new Matrix(x.ColNum, 1);
            var costs = new List<double>();
            while (maxItr > 0)
            {
                var h = x * theta;

                if (logCosts)
                {
                    var j = ((1.0 / (2.0 * (double)x.RowNum)) * ((h - y) ^ 2)).Sum();
                    costs.Add(j);
                }
                var diff = (alpha / (double)x.RowNum) * (x.Transpose() * (h - y)) - ((alpha * lambda) / (double)x.RowNum) * theta;
                diff[0, 0] += ((alpha * lambda) / (double)x.RowNum) * theta[0, 0];
                theta = theta - diff;

                maxItr--;
            }

            return new Tuple<Matrix,double[]>(theta, costs.ToArray());
        }
    }

    public class NeuralNetwork
    {
        public Matrix[] Thetas { get; set; }
        public int NumLayers { get; set; }
        public int[] NumNeurons { get; set; }
        
        public NeuralNetwork(int numLayers, int[] numNeurons)
        {
            NumLayers = numLayers;
            NumNeurons = numNeurons;

            Thetas = new Matrix[numLayers - 1];

            for(int idx=0; idx < numLayers - 1; idx++)
            {
                var matrix = new Matrix(numNeurons[idx+1], numNeurons[idx] + 1);
                Randomize(matrix);
                Thetas[idx] = matrix;
            }
        }

        void Randomize(Matrix matrix)
        {
            var rnd = new Random();
            for(int row=0; row<matrix.RowNum; row++)
            {
                for(int col=0; col<matrix.ColNum; col++)
                {
                    matrix[row, col] = rnd.NextDouble() * ( rnd.Next() % 2 == 1 ? -1.0 : 1.0 );
                }
            }
        }

        public void RandomizeThetas()
        {
            foreach(var matrix in Thetas) Randomize(matrix);
        }

        public void Learn(Matrix[] input, Matrix[] output, double alpha, double lambda, int maxItr)
        {
            while (maxItr > 0)
            {
                var L_deltas = InitializeLDeltas();

                for (int idx = 0; idx < input.GetLength(0); idx++)
                {
                    var values = ForwardProp(input[idx]);
                    BackProp(L_deltas, output[idx], values);
                }

                UpdateThetas(input.GetLength(0), L_deltas, alpha, lambda);
                maxItr--;
            }
        }

        Matrix[] InitializeLDeltas()
        {
            var ret = new Matrix[Thetas.Length];
            for (int i = 0; i < ret.Length; i++)
            {
                ret[i] = new Matrix(Thetas[i].RowNum, Thetas[i].ColNum);
            }
            return ret;
        }

        void UpdateThetas(int m, Matrix[] L_deltas, double alpha, double lambda)
        {
            if (m == 0) return;
            for (int l = 0; l < Thetas.Length; l++)
            {
                for (int i = 0; i < Thetas[l].RowNum; i++)
                {
                    for (int j = 0; j < Thetas[l].ColNum; j++)
                    {
                        var d = j == 0 ? L_deltas[l][i, j] / (double)m :
                                         (L_deltas[l][i, j] + lambda * (Thetas[l][i, j])) / (double)m;
                        Thetas[l][i, j] -= alpha * d;
                    }
                }
            }
        }

        /// <summary>
        /// execute forward prop.
        /// </summary>
        /// <returns>values of each node</returns>
        public Matrix[] ForwardProp(Matrix input)
        {
            var ret = new List<Matrix>();
            ret.Add(input);

            var current = input;
            for (int thetaIdx = 0; thetaIdx < Thetas.Length; thetaIdx++)
            {
                var theta = Thetas[thetaIdx];
                current = Matrix.GetSigmoid(Matrix.MuxWithBias(theta, current));

                ret.Add(current);
            }

            return ret.ToArray();
        }

        void BackProp(Matrix[] L_deltas, Matrix output, Matrix[] a)
        {
            var l = Thetas.Length;
            var S_deltas = new Matrix[Thetas.Length + 1];
            S_deltas[l] = a[l] - output;
            l--;

            while (l >= 1)
            {
                var aa = Thetas[l].Transpose() * S_deltas[l + 1];
                var bb = a[l];
                var cc = 1 - a[l];

                var dd = Matrix.MuxWithBias_ElementWise(aa, bb);
                var ee = Matrix.MuxWithBias_ElementWise(dd, cc);

                S_deltas[l] = ee;
                l--;
            }

            for (l = 0; l < L_deltas.Length; l++)
            {
                for (int i = 0; i < L_deltas[l].RowNum; i++)
                {
                    for (int j = 0; j < L_deltas[l].ColNum; j++)
                    {
                        L_deltas[l][i, j] += (j == 0 ? 1.0 : a[l][j - 1, 0]) * S_deltas[l + 1][i, 0];
                    }
                }
            }
        }
    }
}