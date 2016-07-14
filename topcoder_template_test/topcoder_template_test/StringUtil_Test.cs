using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace topcoder_template_test
{
    [TestClass]
    public class StringUtil_Test
    {
        [TestMethod]
        public void Z_Algorithm_Test()
        {
            var ret = StringUtil.Z_Algorithm("aaabaaaab");
            var exp = new int[] { 9, 2, 1, 0, 3, 4, 2, 1, 0 };
            for (int i = 0; i < exp.Length; i++)
            {
                Assert.AreEqual(exp[i], ret[i]);
            }
        }
    }
}
