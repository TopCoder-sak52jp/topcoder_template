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
            const double epsilon_init = 0.12;
            var rnd = new Random();
            for(int row=0; row<matrix.RowNum; row++)
            {
                for(int col=0; col<matrix.ColNum; col++)
                {
                    matrix[row, col] = rnd.NextDouble() * 2 * epsilon_init - epsilon_init;
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
                var grad = GetGrad(Thetas, input, output, input.Length, lambda);
                UpdateThetas(input.GetLength(0), grad, alpha, lambda, input, output);
                maxItr--;
            }
        }

        void UpdateThetas(int m, Matrix[] grads, double alpha, double lambda, Matrix[] input, Matrix[] output)
        {
            if (m == 0) return;
            for (int l = 0; l < Thetas.Length; l++)
            {
                Thetas[l] = Thetas[l] - alpha * grads[l];
            }
        }

        public Matrix[] GetGrad(Matrix[] thetas, Matrix[] input, Matrix[] output, int m, double lambda)
        {
            var L_deltas = InitializeLDeltas();

            for (int idx = 0; idx < input.GetLength(0); idx++)
            {
                Matrix[] z = null;
                var values = ForwardProp(input[idx], thetas, ref z);
                BackProp(L_deltas, output[idx], values, z);
            }

            var grad = GetGrad(thetas, L_deltas, input.Length, lambda);
            return grad;
        }

        Matrix[] GetGrad(Matrix[] thetas, Matrix[] L_deltas, int m, double lambda)
        {
            var ret = new List<Matrix>();

            for (int idx = 0; idx < thetas.Length; idx++)
            {
                var wk_theta = thetas[idx].Clone();
                for (int col = 0; col < wk_theta.ColNum; col++)
                {
                    wk_theta[0, col] = 0.0;
                }

                var grad = (L_deltas[idx] / m) + ( ( lambda / (double)m ) * wk_theta);
                ret.Add(grad);
            }

            return ret.ToArray();
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

        public double J(Matrix[] thetas, Matrix[] input, Matrix[] output)
        {
            var ret = 0.0;

            for (int inputIdx = 0; inputIdx < input.Length; inputIdx++)
            {
                Matrix[] z = null;
                var ak = ForwardProp(input[inputIdx], thetas, ref z).Last();
                var y = output[inputIdx];

                var sum = -1.0 * Matrix.ElementWiseMux(y, ak.Log()) - Matrix.ElementWiseMux((1.0 - y), (1 - ak).Log());
                ret += sum.Sum();
            }

            ret /= input.Length;

            return ret;
        }

        public Matrix[] ForwardProp(Matrix input)
        {
            Matrix[] z = null;
            return ForwardProp(input, this.Thetas, ref z);
        }

        /// <summary>
        /// execute forward prop.
        /// </summary>
        /// <returns>values of each node</returns>
        Matrix[] ForwardProp(Matrix input, Matrix[] thetas, ref Matrix[] z)
        {
            var zret = new List<Matrix>();
            zret.Add(null);
            var ret = new List<Matrix>();

            var inputWithBias = Matrix.AddOneToTopRow(input);
            ret.Add(inputWithBias);

            var current = inputWithBias;
            for (int thetaIdx = 0; thetaIdx < thetas.Length; thetaIdx++)
            {
                var theta = thetas[thetaIdx];
                var zValue = theta * current;

                current = Matrix.GetSigmoid(zValue);

                zret.Add(thetaIdx == thetas.Length - 1 ? zValue : Matrix.AddOneToTopRow(zValue));
                ret.Add(thetaIdx == thetas.Length - 1 ? current : Matrix.AddOneToTopRow(current));

                current = Matrix.AddOneToTopRow(current);
            }

            z = zret.ToArray();
            return ret.ToArray();
        }

        void BackProp(Matrix[] L_deltas, Matrix output, Matrix[] a, Matrix[] z)
        {
            var l = Thetas.Length;
            var S_deltas = new Matrix[Thetas.Length + 1];
            S_deltas[l] = a[l] - output;
            L_deltas[l - 1] += S_deltas[l] * a[l - 1].Transpose();
            l--;

            while (l >= 1)
            {
                var sigmoidGradient = Matrix.ElementWiseMux(Matrix.GetSigmoid(z[l]), 1 - Matrix.GetSigmoid(z[l]));
                S_deltas[l] = Matrix.ElementWiseMux(Thetas[l].Transpose() * S_deltas[l + 1], sigmoidGradient);
                S_deltas[l] = Matrix.RemoveTopRow(S_deltas[l]);
                L_deltas[l - 1] += S_deltas[l] * a[l - 1].Transpose();
                l--;
            }
        }
    }
}