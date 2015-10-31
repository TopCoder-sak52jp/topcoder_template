using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace topcoder_template_test
{
    [TestClass]
    public class PriorityQueue_Tests
    {
        [TestMethod]
        public void Ordinal_Int_Asc()
        {
            var pq = new PriorityQueue<int>();

            pq.Enqueue(0);
            pq.Enqueue(5);
            pq.Enqueue(3);
            pq.Enqueue(8);
            pq.Enqueue(-1);
            pq.Enqueue(3);

            Assert.AreEqual(6, pq.Count);
            Assert.IsTrue(pq.Contains(0));
            Assert.IsTrue(pq.Contains(3));
            Assert.IsTrue(pq.Contains(-1));
            Assert.IsFalse(pq.Contains(6));

            Assert.AreEqual(-1, pq.Dequeue());
            Assert.AreEqual(0, pq.Dequeue());
            Assert.IsTrue(pq.Contains(3));
            Assert.AreEqual(3, pq.Dequeue());
            Assert.IsTrue(pq.Contains(3));
            Assert.AreEqual(3, pq.Dequeue());
            Assert.IsFalse(pq.Contains(3));
            Assert.AreEqual(5, pq.Dequeue());
            Assert.AreEqual(8, pq.Dequeue());
            Assert.AreEqual(0, pq.Count);

            pq.Enqueue(0);
            pq.Enqueue(5);
            pq.Enqueue(5);
            pq.Clear();
            Assert.AreEqual(0, pq.Count);

            pq.Enqueue(0);
            pq.Enqueue(5);
            pq.Enqueue(5);
            Assert.AreEqual(0, pq.Peek());
            Assert.AreEqual(0, pq.Dequeue());
            Assert.AreEqual(5, pq.Peek());
            Assert.AreEqual(5, pq.Dequeue());
            Assert.AreEqual(5, pq.Peek());
            Assert.AreEqual(5, pq.Dequeue());
            Assert.AreEqual(0, pq.Count);

            pq.Enqueue(0);
            pq.Enqueue(5);
            pq.Enqueue(3);
            pq.Enqueue(8);
            pq.Enqueue(-1);
            pq.Enqueue(3);
            var ar = pq.ToArray();

            Assert.AreEqual(-1, ar[0]);
            Assert.AreEqual(0, ar[1]);
            Assert.AreEqual(3, ar[2]);
            Assert.AreEqual(3, ar[3]);
            Assert.AreEqual(5, ar[4]);
            Assert.AreEqual(8, ar[5]);

        }

        [TestMethod]
        public void Ordinal_Int_Desc()
        {
            var pq = new PriorityQueue<int>(1);

            pq.Enqueue(0);
            pq.Enqueue(5);
            pq.Enqueue(3);
            pq.Enqueue(8);
            pq.Enqueue(-1);
            pq.Enqueue(3);

            Assert.AreEqual(6, pq.Count);
            Assert.IsTrue(pq.Contains(0));
            Assert.IsTrue(pq.Contains(3));
            Assert.IsTrue(pq.Contains(-1));
            Assert.IsFalse(pq.Contains(6));

            Assert.AreEqual(8, pq.Dequeue());
            Assert.AreEqual(5, pq.Dequeue());
            Assert.IsTrue(pq.Contains(3));
            Assert.AreEqual(3, pq.Dequeue());
            Assert.IsTrue(pq.Contains(3));
            Assert.AreEqual(3, pq.Dequeue());
            Assert.IsFalse(pq.Contains(3));
            Assert.AreEqual(0, pq.Dequeue());
            Assert.AreEqual(-1, pq.Dequeue());
            Assert.AreEqual(0, pq.Count);

            pq.Enqueue(0);
            pq.Enqueue(5);
            pq.Enqueue(5);
            pq.Clear();
            Assert.AreEqual(0, pq.Count);

            pq.Enqueue(0);
            pq.Enqueue(5);
            pq.Enqueue(5);
            Assert.AreEqual(5, pq.Peek());
            Assert.AreEqual(5, pq.Dequeue());
            Assert.AreEqual(5, pq.Peek());
            Assert.AreEqual(5, pq.Dequeue());
            Assert.AreEqual(0, pq.Peek());
            Assert.AreEqual(0, pq.Dequeue());
            Assert.AreEqual(0, pq.Count);

            pq.Enqueue(0);
            pq.Enqueue(5);
            pq.Enqueue(3);
            pq.Enqueue(8);
            pq.Enqueue(-1);
            pq.Enqueue(3);
            var ar = pq.ToArray();

            Assert.AreEqual(8, ar[0]);
            Assert.AreEqual(5, ar[1]);
            Assert.AreEqual(3, ar[2]);
            Assert.AreEqual(3, ar[3]);
            Assert.AreEqual(0, ar[4]);
            Assert.AreEqual(-1, ar[5]);

        }

        [TestMethod]
        public void Ordinal_String_Asc()
        {
            var pq = new PriorityQueue<string>();

            pq.Enqueue("ABC");
            pq.Enqueue("HJ");
            pq.Enqueue("DE");
            pq.Enqueue("XY");
            pq.Enqueue("AA");
            pq.Enqueue("DE");

            Assert.AreEqual(6, pq.Count);
            Assert.IsTrue(pq.Contains("ABC"));
            Assert.IsTrue(pq.Contains("DE"));
            Assert.IsTrue(pq.Contains("AA"));
            Assert.IsFalse(pq.Contains("ZZ"));

            Assert.AreEqual("AA", pq.Dequeue());
            Assert.AreEqual("ABC", pq.Dequeue());
            Assert.IsTrue(pq.Contains("DE"));
            Assert.AreEqual("DE", pq.Dequeue());
            Assert.IsTrue(pq.Contains("DE"));
            Assert.AreEqual("DE", pq.Dequeue());
            Assert.IsFalse(pq.Contains("DE"));
            Assert.AreEqual("HJ", pq.Dequeue());
            Assert.AreEqual("XY", pq.Dequeue());
            Assert.AreEqual(0, pq.Count);
        }

        [TestMethod]
        public void Ordinal_String_Desc()
        {
            var pq = new PriorityQueue<string>(1);

            pq.Enqueue("ABC");
            pq.Enqueue("HJ");
            pq.Enqueue("DE");
            pq.Enqueue("XY");
            pq.Enqueue("AA");
            pq.Enqueue("DE");

            Assert.AreEqual(6, pq.Count);
            Assert.IsTrue(pq.Contains("ABC"));
            Assert.IsTrue(pq.Contains("DE"));
            Assert.IsTrue(pq.Contains("AA"));
            Assert.IsFalse(pq.Contains("ZZ"));

            Assert.AreEqual("XY", pq.Dequeue());
            Assert.AreEqual("HJ", pq.Dequeue());
            Assert.IsTrue(pq.Contains("DE"));
            Assert.AreEqual("DE", pq.Dequeue());
            Assert.IsTrue(pq.Contains("DE"));
            Assert.AreEqual("DE", pq.Dequeue());
            Assert.IsFalse(pq.Contains("DE"));
            Assert.AreEqual("ABC", pq.Dequeue());
            Assert.AreEqual("AA", pq.Dequeue());
            Assert.AreEqual(0, pq.Count);
        }
    }
}
