using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace topcoder_template_test
{
    [TestClass]
    public class BipartiteMatching_Tests
    {
        [TestMethod]
        public void Ordinal()
        {
            var bm = new BipartiteMatching();
            bm.AddPair(0, 3);
            bm.AddPair(0, 4);
            bm.AddPair(1, 3);
            bm.AddPair(1, 5);
            bm.AddPair(2, 4);
            Assert.AreEqual(3, bm.GetMaxMatchingNum());

            bm = new BipartiteMatching();
            bm.AddPair(0, 4);
            bm.AddPair(1, 3);
            bm.AddPair(1, 5);
            bm.AddPair(2, 4);
            Assert.AreEqual(2, bm.GetMaxMatchingNum());

            bm = new BipartiteMatching();
            bm.AddPair(0, 4);
            bm.AddPair(0, 5);
            bm.AddPair(1, 4);
            bm.AddPair(1, 6);
            bm.AddPair(2, 5);
            bm.AddPair(2, 6);
            Assert.AreEqual(3, bm.GetMaxMatchingNum());
        }
    }
}
