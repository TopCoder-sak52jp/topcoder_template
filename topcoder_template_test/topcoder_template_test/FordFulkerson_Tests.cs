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

            ff.AddEdge(0, 1, 50);
            ff.AddEdge(0, 2, 60);
            ff.AddEdge(1, 2, 40);
            ff.AddEdge(1, 3, 40);
            ff.AddEdge(2, 3, 30);

            Assert.AreEqual(40, ff.GetMaxFlow(1, 3));
            Assert.AreEqual(30, ff.GetMaxFlow(0, 3));

        }
    }
}
