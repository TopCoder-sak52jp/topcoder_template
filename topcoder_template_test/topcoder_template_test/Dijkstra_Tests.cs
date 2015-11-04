using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace topcoder_template_test
{
    [TestClass]
    public class Dijkstra_Tests
    {
        [TestMethod]
        public void Ordinal()
        {
            var dk = new Dijkstra(4);

            dk.AddPath(0, 1, 3);
            dk.AddPath(0, 2, 1);
            dk.AddPath(1, 2, 2);
            dk.AddPath(1, 3, 1);
            dk.AddPath(1, 4, 2);
            dk.AddPath(2, 1, 1);
            dk.AddPath(2, 4, 4);
            dk.AddPath(3, 2, 2);
            dk.AddPath(3, 4, 4);

            Assert.AreEqual(4, dk.GetMinCost(0, 4));
        }

        [TestMethod]
        public void EdgeCase()
        {
            var dk = new Dijkstra(4);
            dk.AddPath(0, 1, 1);
            dk.AddPath(1, 2, 1);
            dk.AddPath(2, 3, 1);
            dk.AddPath(3, 4, 1);
            Assert.AreEqual(4, dk.GetMinCost(0, 4));

            dk = new Dijkstra(1);
            dk.AddPath(0, 1, 1);
            Assert.AreEqual(1, dk.GetMinCost(0, 1));
        }

        [TestMethod]
        public void NoPath()
        {
            var dk = new Dijkstra(5);
            dk.AddPath(0, 1, 1);
            dk.AddPath(2, 3, 1);
            dk.AddPath(3, 4, 1);
            dk.AddPath(4, 5, 1);
            Assert.AreEqual(Int32.MaxValue, dk.GetMinCost(0, 5));
        }
    }
}
