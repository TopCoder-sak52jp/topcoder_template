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
        const double EPS = 0.0001;

        [TestMethod]
        public void LinearRegressionTest()
        {
            //y = 0.0 + 2.0 x1
            var x = new Matrix(new double[,] { { 1, 1 }, { 1, 2 }, { 1, 4 } });
            var y = new Matrix(new double[,] { { 2 }, { 4 }, { 8 } });
            var ret = LinearRegression.GetResult(x, y, false, 0.01);
            Assert.IsTrue(Math.Abs(ret[0, 0] - 0.0) < EPS);
            Assert.IsTrue(Math.Abs(ret[1, 0] - 2.0) < EPS);
            ret = LinearRegression.GetResult(x, y);
            Assert.IsTrue(Math.Abs(ret[0, 0] - 0.0) < EPS);
            Assert.IsTrue(Math.Abs(ret[1, 0] - 2.0) < EPS);

            //y = 1.5 + 3.5 x1
            x = new Matrix(new double[,] { { 1, 1 }, { 1, 2 }, { 1, 4 } });
            y = new Matrix(new double[,] { { 5 }, { 8.5 }, { 15.5 } });
            ret = LinearRegression.GetResult(x, y, false, 0.01);
            Assert.IsTrue(Math.Abs(ret[0, 0] - 1.5) < EPS);
            Assert.IsTrue(Math.Abs(ret[1, 0] - 3.5) < EPS);
            ret = LinearRegression.GetResult(x, y);
            Assert.IsTrue(Math.Abs(ret[0, 0] - 1.5) < EPS);
            Assert.IsTrue(Math.Abs(ret[1, 0] - 3.5) < EPS);

            //y = -3.3 + 2.1 x1 - 0.03 x2
            x = new Matrix(new double[,] { { 1, 1, 2 }, { 1, 2, 2 }, { 1, 4, 5 } });
            y = new Matrix(new double[,] { { -1.26 }, { 0.84 }, { 4.95 } });
            ret = LinearRegression.GetResult(x, y, false, 0.01);
            Assert.IsTrue(Math.Abs(ret[0, 0] + 3.3) < EPS);
            Assert.IsTrue(Math.Abs(ret[1, 0] - 2.1) < EPS);
            Assert.IsTrue(Math.Abs(ret[2, 0] + 0.03) < EPS);
            ret = LinearRegression.GetResult(x, y);
            Assert.IsTrue(Math.Abs(ret[0, 0] + 3.3) < EPS);
            Assert.IsTrue(Math.Abs(ret[1, 0] - 2.1) < EPS);
            Assert.IsTrue(Math.Abs(ret[2, 0] + 0.03) < EPS);
        }
    }
}
