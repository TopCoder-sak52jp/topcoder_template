using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace topcoder_template_test
{
    public static class LinearRegression
    {
        const double EPS = 10E-17;

        /// <summary>
        /// get linear regression result(Least Square Method)
        /// (theta0 + theta1 * x1 + theta2 * x2 ... = y)
        /// </summary>
        /// <param name="x">training sets(data, two-dimentional)</param>
        /// <param name="y">training sets(results, vector)</param>
        /// <param name="normalEquation">true: use normal equation, false: use gradient descent</param>
        /// <param name="alpha">alpha for the gradient descent</param>
        public static Matrix GetResult(Matrix x, Matrix y, bool normalEquation=true, double alpha=0.1)
        {
            if (normalEquation)
                return GetResult_NormalEquation(x, y);
            else
                return GetResult_GradientDescent(x, y, alpha);
        }

        static Matrix GetResult_NormalEquation(Matrix x, Matrix y)
        {
            var xT = x.Transpose();
            return (xT * x).Inverse() * xT * y;
        }

        static Matrix GetResult_GradientDescent(Matrix x, Matrix y, double alpha)
        {
            var theta = new Matrix(x.ColNum, 1);
            var prevCost = -0.1;
            while(true)
            {
                var x_mux_theta_minus_y = (x * theta) - y;

                var cost = (x_mux_theta_minus_y.Transpose() * x_mux_theta_minus_y)[0, 0];
                if (Math.Abs(prevCost - cost) < EPS) break;
                prevCost = cost;

                theta = theta - ((alpha / x.RowNum) * x.Transpose()) * x_mux_theta_minus_y;
            }

            return theta;
        }

        /// <summary>
        /// get linear regression result
        /// (theta0 + theta1 * x1 + theta2 * x2 ... = y)
        /// </summary>
        /// <param name="trainingSets">training set: { ({ x1, x2, ... xn }, y ) } </param>
        /// <param name="alpha">alpha value</param>
        /// <returns>{ theta0, theta1, ... }</returns>
        public static double[] GetResult_GradientDescent_old(Tuple<Matrix, double>[] trainingSets, double alpha)
        {
            var theta = new Matrix(trainingSets[0].Item1.ColNum, 1);

            var prevCost = -0.1;
            while (true)
            {
                var cost = 0.0;
                var h_minus_y = new double[trainingSets.Length];
                for (int dataIdx = 0; dataIdx < trainingSets.Length; dataIdx++)
                {
                    h_minus_y[dataIdx] = (trainingSets[dataIdx].Item1 * theta)[0, 0] - trainingSets[dataIdx].Item2;
                    cost += (h_minus_y[dataIdx] * h_minus_y[dataIdx]);
                }
                cost /= (2 * trainingSets.Length);

                if (Math.Abs(cost - prevCost) < EPS) break;
                prevCost = cost;

                for (int thetaIdx = 0; thetaIdx < theta.RowNum; thetaIdx++)
                {
                    var sum = 0.0;
                    for (int dataIdx = 0; dataIdx < trainingSets.Length; dataIdx++)
                    {
                        sum += h_minus_y[dataIdx] * trainingSets[dataIdx].Item1[0, thetaIdx];
                    }
                    sum /= trainingSets.Length;
                    theta[thetaIdx, 0] -= alpha * sum;
                }
            }

            var ret = new double[theta.RowNum];
            for (int i = 0; i < theta.RowNum; i++) ret[i] = theta[i, 0];

            return ret;
        }
    }
}