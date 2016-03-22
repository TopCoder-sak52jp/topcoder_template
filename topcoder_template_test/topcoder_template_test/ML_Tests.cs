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

            var numgrads = nn.GetNumericGradients(x, y);
            var grads = nn.GetGrad(new Matrix[] { theta1, theta2 }, x, y, x.Length, lambda);

            var ret = numgrads.Sum(n => n.Sum()) - grads.Sum(n => n.Sum());


            Assert.IsTrue(ret < 0.0000001);
        }
    }
}
