using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace topcoder_template_test
{
    [TestClass]
    public class ML_Tests
    {
        const double EPS = 0.01;

        [TestMethod]
        public void LinearRegressionTest()
        {
            //y = 0.0 + 2.0 x1
            var x = new Matrix(new double[,] { { 1, 1 }, { 1, 2 }, { 1, 4 } });
            var y = new Matrix(new double[,] { { 2 }, { 4 }, { 8 } });
            var ret = LinearRegression.GetResult_GradientDescent(x, y, 0.01, 0, 4000, false).Item1;
            Assert.IsTrue(Math.Abs(ret[0, 0] - 0.0) < EPS);
            Assert.IsTrue(Math.Abs(ret[1, 0] - 2.0) < EPS);
            ret = LinearRegression.GetResult_NormalEquation(x, y, 0).Item1;
            Assert.IsTrue(Math.Abs(ret[0, 0] - 0.0) < EPS);
            Assert.IsTrue(Math.Abs(ret[1, 0] - 2.0) < EPS);

            //y = 1.5 + 3.5 x1
            x = new Matrix(new double[,] { { 1, 1 }, { 1, 2 }, { 1, 4 } });
            y = new Matrix(new double[,] { { 5 }, { 8.5 }, { 15.5 } });
            ret = LinearRegression.GetResult_GradientDescent(x, y, 0.01, 0, 4000, false).Item1;
            Assert.IsTrue(Math.Abs(ret[0, 0] - 1.5) < EPS);
            Assert.IsTrue(Math.Abs(ret[1, 0] - 3.5) < EPS);
            ret = LinearRegression.GetResult_NormalEquation(x, y, 0).Item1;
            Assert.IsTrue(Math.Abs(ret[0, 0] - 1.5) < EPS);
            Assert.IsTrue(Math.Abs(ret[1, 0] - 3.5) < EPS);

            //y = -3.3 + 2.1 x1 - 0.03 x2
            x = new Matrix(new double[,] { { 1, 1, 2 }, { 1, 2, 2 }, { 1, 4, 5 } });
            y = new Matrix(new double[,] { { -1.26 }, { 0.84 }, { 4.95 } });
            ret = LinearRegression.GetResult_GradientDescent(x, y, 0.01, 0, 4000, false).Item1;
            Assert.IsTrue(Math.Abs(ret[0, 0] + 3.3) < EPS);
            Assert.IsTrue(Math.Abs(ret[1, 0] - 2.1) < EPS);
            Assert.IsTrue(Math.Abs(ret[2, 0] + 0.03) < EPS);
            ret = LinearRegression.GetResult_NormalEquation(x, y, 0).Item1;
            Assert.IsTrue(Math.Abs(ret[0, 0] + 3.3) < EPS);
            Assert.IsTrue(Math.Abs(ret[1, 0] - 2.1) < EPS);
            Assert.IsTrue(Math.Abs(ret[2, 0] + 0.03) < EPS);
        }

        [TestMethod]
        public void NeuralNetworkTest_ForwardProp()
        {
            //XNOR

            var nn = new NeuralNetwork(3, new int[] { 2, 2, 1 });
            nn.Thetas[0][0, 0] = -30;
            nn.Thetas[0][0, 1] = 20;
            nn.Thetas[0][0, 2] = 20;
            nn.Thetas[0][1, 0] = 10;
            nn.Thetas[0][1, 1] = -20;
            nn.Thetas[0][1, 2] = -20;
            nn.Thetas[1][0, 0] = -10;
            nn.Thetas[1][0, 1] = 20;
            nn.Thetas[1][0, 2] = 20;

            var input = new Matrix(2, 1);
            input[0, 0] = 0;
            input[1, 0] = 1;
            var result = nn.ForwardProp(input);
            Assert.IsTrue(result.Last()[0, 0] < 0.01);

            input[0, 0] = 1;
            input[1, 0] = 0;
            result = nn.ForwardProp(input);
            Assert.IsTrue(result.Last()[0, 0] < 0.01);

            input[0, 0] = 1;
            input[1, 0] = 1;
            result = nn.ForwardProp(input);
            Assert.IsTrue(result.Last()[0, 0] >= 0.99);

            input[0, 0] = 0;
            input[1, 0] = 0;
            result = nn.ForwardProp(input);
            Assert.IsTrue(result.Last()[0, 0] >= 0.99);
        }

        Matrix[] GetNumericGradients(NeuralNetwork nn, Matrix[] thetas, Matrix[] input, Matrix[] output)
        {
            var ret = new Matrix[thetas.Length];

            for (int idx = 0; idx < thetas.Length; idx++)
            {
                ret[idx] = GetNumericGradient(nn, thetas, idx, input, output);
            }

            return ret;
        }

        Matrix GetNumericGradient(NeuralNetwork nn, Matrix[] thetas, int idx, Matrix[] input, Matrix[] output)
        {
            var t = thetas.ToArray();

            const double e = 1e-4;
            var ret = new Matrix(t[idx].RowNum, t[idx].ColNum);
            var diffMat = new Matrix(t[idx].RowNum, t[idx].ColNum);

            for (int row = 0; row < diffMat.RowNum; row++)
            {
                for (int col = 0; col < diffMat.ColNum; col++)
                {
                    var orgValue = t[idx][row, col];

                    t[idx][row, col] = orgValue - e;
                    var loss1 = nn.J(thetas, input, output);

                    thetas[idx][row, col] = orgValue + e;
                    var loss2 = nn.J(thetas, input, output);

                    ret[row, col] = (loss2 - loss1) / (2.0 * e);

                    thetas[idx][row, col] = orgValue;
                }
            }

            return ret;
        }

        [TestMethod]
        public void NeuralNetworkTest_Gradients()
        {
            double lambda = 0;

            var nn = new NeuralNetwork(3, new int[] { 2, 1, 2 });
            var theta1 = new Matrix(1, 3);
            theta1[0, 0] = 0.01;
            theta1[0, 1] = 0.02;
            theta1[0, 2] = 0.03;

            var theta2 = new Matrix(2, 2);
            theta2[0, 0] = 0.02;
            theta2[0, 1] = 0.03;
            theta2[1, 0] = 0.05;
            theta2[1, 1] = 0.04;

            nn.Thetas = new Matrix[] { theta1, theta2 };

            var x1 = new Matrix(2, 1);
            x1[0, 0] = 0.08;
            x1[1, 0] = 0.07;

            var x2 = new Matrix(2, 1);
            x2[0, 0] = 0.06;
            x2[1, 0] = 0.05;

            var x = new Matrix[] { x1, x2 };

            var y1 = new Matrix(2, 1);
            y1[0, 0] = 0;
            y1[1, 0] = 1;

            var y2 = new Matrix(2, 1);
            y2[0, 0] = 1;
            y2[1, 0] = 0;

            var y = new Matrix[] { y1, y2 };

            var numgrads = GetNumericGradients(nn, nn.Thetas, x, y);
            var grads = nn.GetGrad(new Matrix[] { theta1, theta2 }, x, y, x.Length, lambda);

            var ret = numgrads.Sum(n => n.Sum()) - grads.Sum(n => n.Sum());

            Assert.IsTrue(ret < 0.0000001);
        }

        [TestMethod]
        public void NeuralNetworkTest_Gradients_random()
        {
            Random rnd = new Random();
            double lambda = 0;

            var nn = new NeuralNetwork(4, new int[] { 2, 2, 2, 1 });
            var theta1 = new Matrix(2, 3);
            theta1[0, 0] = rnd.NextDouble();
            theta1[0, 1] = rnd.NextDouble();
            theta1[0, 2] = rnd.NextDouble();
            theta1[1, 0] = rnd.NextDouble();
            theta1[1, 1] = rnd.NextDouble();
            theta1[1, 2] = rnd.NextDouble();

            var theta2 = new Matrix(2, 3);
            theta2[0, 0] = rnd.NextDouble();
            theta2[0, 1] = rnd.NextDouble();
            theta2[0, 2] = rnd.NextDouble();
            theta2[1, 0] = rnd.NextDouble();
            theta2[1, 1] = rnd.NextDouble();
            theta2[1, 2] = rnd.NextDouble();

            var theta3 = new Matrix(1, 3);
            theta3[0, 0] = rnd.NextDouble();
            theta3[0, 1] = rnd.NextDouble();
            theta3[0, 2] = rnd.NextDouble();

            nn.Thetas = new Matrix[] { theta1, theta2, theta3 };

            var x1 = new Matrix(2, 1);
            x1[0, 0] = rnd.NextDouble();
            x1[1, 0] = rnd.NextDouble();

            var x2 = new Matrix(2, 1);
            x2[0, 0] = rnd.NextDouble();
            x2[1, 0] = rnd.NextDouble();

            var x = new Matrix[] { x1, x2 };

            var y1 = new Matrix(1, 1);
            y1[0, 0] = 0;

            var y2 = new Matrix(1, 1);
            y2[0, 0] = 1;

            var y = new Matrix[] { y1, y2 };

            var numgrads = GetNumericGradients(nn, nn.Thetas, x, y);
            var grads = nn.GetGrad(nn.Thetas.ToArray(), x, y, x.Length, lambda);

            var ret = numgrads.Sum(n => n.Sum()) - grads.Sum(n => n.Sum());

            Assert.IsTrue(ret < 0.0000001);
        }

        [TestMethod]
        public void NeuralNetworkTest_Learn()
        {
            //XNOR

            var nn = new NeuralNetwork(3, new int[] { 2, 2, 1 });
            nn.RandomizeThetas();

            var inputs = new List<Matrix>();
            var outputs = new List<Matrix>();

            var input = new Matrix(2, 1);
            input[0, 0] = 0;
            input[1, 0] = 1;
            inputs.Add(input);
            var output = new Matrix(1, 1);
            output[0, 0] = 0;
            outputs.Add(output);

            input = new Matrix(2, 1);
            input[0, 0] = 1;
            input[1, 0] = 0;
            inputs.Add(input);
            output = new Matrix(1, 1);
            output[0, 0] = 0;
            outputs.Add(output);

            input = new Matrix(2, 1);
            input[0, 0] = 1;
            input[1, 0] = 1;
            inputs.Add(input);
            output = new Matrix(1, 1);
            output[0, 0] = 1;
            outputs.Add(output);

            input = new Matrix(2, 1);
            input[0, 0] = 0;
            input[1, 0] = 0;
            inputs.Add(input);
            output = new Matrix(1, 1);
            output[0, 0] = 1;
            outputs.Add(output);

            nn.Learn(inputs.ToArray(), outputs.ToArray(), 3, 0.01, 60000);

            //check
            var result = nn.ForwardProp(inputs[0]).Last();
            Assert.IsTrue(Math.Abs(result[0, 0] - outputs[0][0, 0]) < EPS);

            result = nn.ForwardProp(inputs[1]).Last();
            Assert.IsTrue(Math.Abs(result[0, 0] - outputs[1][0, 0]) < EPS);

            result = nn.ForwardProp(inputs[2]).Last();
            Assert.IsTrue(Math.Abs(result[0, 0] - outputs[2][0, 0]) < EPS);

            result = nn.ForwardProp(inputs[3]).Last();
            Assert.IsTrue(Math.Abs(result[0, 0] - outputs[3][0, 0]) < EPS);
        }
    }
}
