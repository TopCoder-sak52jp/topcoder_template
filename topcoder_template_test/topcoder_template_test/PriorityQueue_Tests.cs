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
            var pq = new PriorityQueue<int>(6);

            pq.Push(0);
            pq.Push(5);
            pq.Push(3);
            pq.Push(8);
            pq.Push(-1);
            pq.Push(3);

            Assert.AreEqual(6, pq.Count());
            Assert.IsTrue(pq.Contains(0));
            Assert.IsTrue(pq.Contains(3));
            Assert.IsTrue(pq.Contains(-1));
            Assert.IsFalse(pq.Contains(6));

            Assert.AreEqual(-1, pq.Pop());
            Assert.AreEqual(0, pq.Pop());
            Assert.IsTrue(pq.Contains(3));
            Assert.AreEqual(3, pq.Pop());
            Assert.IsTrue(pq.Contains(3));
            Assert.AreEqual(3, pq.Pop());
            Assert.IsFalse(pq.Contains(3));
            Assert.AreEqual(5, pq.Pop());
            Assert.AreEqual(8, pq.Pop());
            Assert.AreEqual(0, pq.Count());

            pq.Push(0);
            pq.Push(5);
            pq.Push(5);
            pq.Clear();
            Assert.AreEqual(0, pq.Count());

            pq.Push(0);
            pq.Push(5);
            pq.Push(5);
            Assert.AreEqual(0, pq.Peek());
            Assert.AreEqual(0, pq.Pop());
            Assert.AreEqual(5, pq.Peek());
            Assert.AreEqual(5, pq.Pop());
            Assert.AreEqual(5, pq.Peek());
            Assert.AreEqual(5, pq.Pop());
            Assert.AreEqual(0, pq.Count());

            pq.Push(0);
            pq.Push(5);
            pq.Push(3);
            pq.Push(8);
            pq.Push(-1);
            pq.Push(3);
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
            var pq = new PriorityQueue<int>(6, 1);

            pq.Push(0);
            pq.Push(5);
            pq.Push(3);
            pq.Push(8);
            pq.Push(-1);
            pq.Push(3);

            Assert.AreEqual(6, pq.Count());
            Assert.IsTrue(pq.Contains(0));
            Assert.IsTrue(pq.Contains(3));
            Assert.IsTrue(pq.Contains(-1));
            Assert.IsFalse(pq.Contains(6));

            Assert.AreEqual(8, pq.Pop());
            Assert.AreEqual(5, pq.Pop());
            Assert.IsTrue(pq.Contains(3));
            Assert.AreEqual(3, pq.Pop());
            Assert.IsTrue(pq.Contains(3));
            Assert.AreEqual(3, pq.Pop());
            Assert.IsFalse(pq.Contains(3));
            Assert.AreEqual(0, pq.Pop());
            Assert.AreEqual(-1, pq.Pop());
            Assert.AreEqual(0, pq.Count());

            pq.Push(0);
            pq.Push(5);
            pq.Push(5);
            pq.Clear();
            Assert.AreEqual(0, pq.Count());

            pq.Push(0);
            pq.Push(5);
            pq.Push(5);
            Assert.AreEqual(5, pq.Peek());
            Assert.AreEqual(5, pq.Pop());
            Assert.AreEqual(5, pq.Peek());
            Assert.AreEqual(5, pq.Pop());
            Assert.AreEqual(0, pq.Peek());
            Assert.AreEqual(0, pq.Pop());
            Assert.AreEqual(0, pq.Count());

            pq.Push(0);
            pq.Push(5);
            pq.Push(3);
            pq.Push(8);
            pq.Push(-1);
            pq.Push(3);
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
            var pq = new PriorityQueue<string>(6);

            pq.Push("ABC");
            pq.Push("HJ");
            pq.Push("DE");
            pq.Push("XY");
            pq.Push("AA");
            pq.Push("DE");

            Assert.AreEqual(6, pq.Count());
            Assert.IsTrue(pq.Contains("ABC"));
            Assert.IsTrue(pq.Contains("DE"));
            Assert.IsTrue(pq.Contains("AA"));
            Assert.IsFalse(pq.Contains("ZZ"));

            Assert.AreEqual("AA", pq.Pop());
            Assert.AreEqual("ABC", pq.Pop());
            Assert.IsTrue(pq.Contains("DE"));
            Assert.AreEqual("DE", pq.Pop());
            Assert.IsTrue(pq.Contains("DE"));
            Assert.AreEqual("DE", pq.Pop());
            Assert.IsFalse(pq.Contains("DE"));
            Assert.AreEqual("HJ", pq.Pop());
            Assert.AreEqual("XY", pq.Pop());
            Assert.AreEqual(0, pq.Count());
        }

        [TestMethod]
        public void Ordinal_String_Desc()
        {
            var pq = new PriorityQueue<string>(6, 1);

            pq.Push("ABC");
            pq.Push("HJ");
            pq.Push("DE");
            pq.Push("XY");
            pq.Push("AA");
            pq.Push("DE");

            Assert.AreEqual(6, pq.Count());
            Assert.IsTrue(pq.Contains("ABC"));
            Assert.IsTrue(pq.Contains("DE"));
            Assert.IsTrue(pq.Contains("AA"));
            Assert.IsFalse(pq.Contains("ZZ"));

            Assert.AreEqual("XY", pq.Pop());
            Assert.AreEqual("HJ", pq.Pop());
            Assert.IsTrue(pq.Contains("DE"));
            Assert.AreEqual("DE", pq.Pop());
            Assert.IsTrue(pq.Contains("DE"));
            Assert.AreEqual("DE", pq.Pop());
            Assert.IsFalse(pq.Contains("DE"));
            Assert.AreEqual("ABC", pq.Pop());
            Assert.AreEqual("AA", pq.Pop());
            Assert.AreEqual(0, pq.Count());
        }

        public class Human : IComparable
        {
            public int Id { get; set; }
            public string Name { get; set; }
            public string Address { get; set; }
            public static IComparer<Human> CompareByName = new HumanComparerByName();
            public static IComparer<Human> CompareByAddress = new HumanComparerByAddress();

            public int CompareTo(object obj)
            {
                return this.Id - (obj as Human).Id;
            }
        }

        public class HumanComparerByName : IComparer<Human>
        {
            public int Compare(Human x, Human y)
            {
                return String.Compare(x.Name, y.Name);
            }
        }

        public class HumanComparerByAddress : IComparer<Human>
        {
            public int Compare(Human x, Human y)
            {
                return String.Compare(x.Address, y.Address);
            }
        }

        [TestMethod]
        public void Ordinal_Custom_Comparer()
        {
            var pq = new PriorityQueue<Human>(5, Human.CompareByName);

            pq.Push(new Human() { Id = 10, Name = "GHI", Address = "X" });
            pq.Push(new Human() { Id = 0, Name = "DEF", Address = "Y" });
            pq.Push(new Human() { Id = 5, Name = "ABC", Address = "Z" });

            Assert.AreEqual("ABC", pq.Pop().Name);
            Assert.AreEqual("DEF", pq.Pop().Name);
            Assert.AreEqual("GHI", pq.Pop().Name);

            pq = new PriorityQueue<Human>(5, Human.CompareByAddress);

            pq.Push(new Human() { Id = 10, Name = "GHI", Address = "X" });
            pq.Push(new Human() { Id = 0, Name = "DEF", Address = "Y" });
            pq.Push(new Human() { Id = 5, Name = "ABC", Address = "Z" });

            Assert.AreEqual("GHI", pq.Pop().Name);
            Assert.AreEqual("DEF", pq.Pop().Name);
            Assert.AreEqual("ABC", pq.Pop().Name);
        }
    }
}
