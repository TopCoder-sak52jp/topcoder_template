using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace topcoder_template_test
{
    [TestClass]
    public class Kruskal_Tests
    {
        [TestMethod]
        public void Ordinal()
        {
            var ks = new Kruskal(3);

            ks.AddPath(0, 1, 300);
            ks.AddPath(1, 2, 50);
            ks.AddPath(0, 2, 351);
            ks.AddPath(1, 3, 90);
            ks.AddPath(2, 3, 25);

            Assert.AreEqual(375, ks.GetCost());
        }
    }
}
