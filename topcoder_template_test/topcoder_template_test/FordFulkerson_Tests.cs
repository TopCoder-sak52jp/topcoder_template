using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace topcoder_template_test
{
    [TestClass]
    public class FordFulkerson_Tests
    {
        [TestMethod]
        public void Ordinal()
        {
            var ff = new FordFulkerson();
            ff.AddEdge(0, 1, 10);
            ff.AddEdge(0, 2, 2);
            ff.AddEdge(1, 2, 6);
            ff.AddEdge(1, 3, 6);
            ff.AddEdge(2, 4, 5);
            ff.AddEdge(3, 2, 3);
            ff.AddEdge(3, 4, 8);
            Assert.AreEqual(11, ff.GetMaxFlow(0, 4));


            ff = new FordFulkerson();
            ff.AddEdge(0, 1, 10);
            ff.AddEdge(0, 2, 2);
            ff.AddEdge(1, 2, 6);
            ff.AddEdge(1, 3, 6);
            ff.AddEdge(2, 4, 5);
            ff.AddEdge(3, 2, 3);
            ff.AddEdge(3, 4, 8);
            Assert.AreEqual(11, ff.GetMaxFlow(1, 4));


            ff = new FordFulkerson();
            ff.AddEdge(0, 1, 10);
            ff.AddEdge(0, 2, 2);
            ff.AddEdge(1, 2, 6);
            ff.AddEdge(1, 3, 6);
            ff.AddEdge(2, 4, 5);
            ff.AddEdge(3, 2, 3);
            ff.AddEdge(3, 4, 8);
            Assert.AreEqual(9, ff.GetMaxFlow(1, 2));

        }
    }
}
