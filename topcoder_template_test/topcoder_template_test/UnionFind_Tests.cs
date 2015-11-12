using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace topcoder_template_test
{
    [TestClass]
    public class UnionFind_Tests
    {
        [TestMethod]
        public void Ordinal()
        {
            var uf = new UnionFind();
            Assert.IsFalse(uf.IsSameGroup(1, 2));
            Assert.IsFalse(uf.IsSameGroup(2, 3));
            Assert.IsFalse(uf.IsSameGroup(1, 3));

            uf.Unite(1, 3);
            Assert.IsFalse(uf.IsSameGroup(1, 2));
            Assert.IsFalse(uf.IsSameGroup(2, 3));
            Assert.IsTrue(uf.IsSameGroup(1, 3));

            uf.Unite(2, 3);
            Assert.IsTrue(uf.IsSameGroup(1, 2));
            Assert.IsTrue(uf.IsSameGroup(2, 3));
            Assert.IsTrue(uf.IsSameGroup(1, 3));

            uf.Unite(50, 51);
            uf.Unite(52, 53);
            Assert.IsTrue(uf.IsSameGroup(50, 51));
            Assert.IsTrue(uf.IsSameGroup(52, 53));
            Assert.IsFalse(uf.IsSameGroup(50, 53));
            uf.Unite(53, 54);
            Assert.IsTrue(uf.IsSameGroup(52, 54));
            uf.Unite(50, 54);
            Assert.IsTrue(uf.IsSameGroup(50, 53));
            Assert.IsTrue(uf.IsSameGroup(50, 54));
            Assert.IsTrue(uf.IsSameGroup(50, 51));

            uf.Unite(70, 71);
            uf.Unite(70, 72);
            uf.Unite(73, 72);
            Assert.IsTrue(uf.IsSameGroup(70, 73));
            Assert.IsTrue(uf.IsSameGroup(73, 70));
            Assert.IsTrue(uf.IsSameGroup(73, 72));

            uf.Unite(99, 98);
            uf.Unite(0, 98);
            Assert.IsTrue(uf.IsSameGroup(98, 99));
            Assert.IsTrue(uf.IsSameGroup(99, 0));
            Assert.IsTrue(uf.IsSameGroup(98, 0));

        }

        [TestMethod]
        public void StringTest()
        {
            var uf = new UnionFind();

            Assert.IsFalse(uf.IsSameGroup("1", "2"));
            Assert.IsFalse(uf.IsSameGroup("2", "3"));
            Assert.IsFalse(uf.IsSameGroup("1", "3"));

            uf.Unite("1", "3");
            Assert.IsFalse(uf.IsSameGroup("1", "2"));
            Assert.IsFalse(uf.IsSameGroup("2", "3"));
            Assert.IsTrue(uf.IsSameGroup("1", "3"));
        }
    }
}
