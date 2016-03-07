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

        void Randomize()
        {
            foreach(var matrix in Thetas) Randomize(matrix);
        }

        public void Learn(Matrix[] input, Matrix[] output)
        {
            Randomize();

            for(int idx=0; idx<input.GetLength(0); idx++)
            {
                Learn(input[idx], output[idx]);
            }
        }

        void Learn(Matrix input, Matrix output)
        {
            var values = ForwardProp(input);
            //BackProp(output, values);
        }

        /// <summary>
        /// execute forward prop.
        /// </summary>
        /// <returns>last layer values</returns>
        public Matrix ForwardProp(Matrix input)
        {
            Matrix ret = input;

            for (int thetaIdx = 0; thetaIdx < Thetas.Length; thetaIdx++)
            {
                var theta = Thetas[thetaIdx];
                ret = Matrix.GetSigmoid(Matrix.MuxWithBias(theta, ret));
            }

            return ret;
        }
    }
}