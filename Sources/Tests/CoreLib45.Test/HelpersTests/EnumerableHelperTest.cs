#region Code Identifications

// Created on     2018/04/29
// Last update on 2018/04/30 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mohammad.Helpers;

namespace CoreLib45.Test.HelpersTests
{
    [TestClass]
    public class EnumerableHelperTest
    {
        [TestMethod]
        public void TestDistinct()
        {
            var data = new List<Person>
                       {
                           new Person("Ali")
                           {
                               Age = 5
                           },
                           new Person("Ali")
                           {
                               Age = 5
                           },
                           new Person("Mohammad")
                           {
                               Age = 5
                           }
                       };
            var distinct = data.Distinct((x, y) => x.Name == y.Name).ToList();
            Assert.AreEqual(distinct.Count, 2);
        }

        [TestMethod]
        public void CastTest()
        {
            Assert.IsNotNull(EnumerableHelper.AsEnuemrable(1, 2, 3, 4, 5).Cast(i => i.ToString()));
        }

        [TestMethod]
        public void CountTest()
        {
            Assert.AreEqual(EnumerableHelper.Count(1, 2, 3, 4, 5), 5);
        }

        [TestMethod]
        public void ElementAtTest()
        {
            Assert.AreEqual(EnumerableHelper.ElementAt(EnumerableHelper.ToArray(1, 2, 3, 4, 5), 2), 3);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ElementAtExceptionTest()
        {
            EnumerableHelper.ElementAt(EnumerableHelper.ToArray(1), 2);
        }

        [TestMethod]
        public void TakeGroupsTest()
        {
            var result = EnumerableHelper.ToArray(1, 2, 3, 4, 5).TakeGroups(2);
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public void ToArrayTest()
        {
            var array = EnumerableHelper.ToArray("Dog", "Cat", 1, 2, 3);
            // ReSharper disable once IsExpressionAlwaysTrue
            Assert.IsTrue(array is object[]);
        }

        [TestMethod]
        public void Enumerable1Test()
        {
            var enum1 = "Cat".AsEnumerable();
        }

        [TestMethod]
        public void Enumerable2Test()
        {
            var enum2 = EnumerableHelper.AsEnuemrable(new object(), new object());
            var count = enum2.Count();
        }

        [TestMethod]
        public void Enumerable3Test()
        {
            var source = (object) new[] {new object(), new object()};
            var enumerable = (IEnumerable) source;
            var arr1 = EnumerableHelper.ToArray(enumerable);
            var arr2 = EnumerableHelper.ToArray(source);
            var enum1 = EnumerableHelper.AsEnuemrable(source);
            var count = enum1.Count();
            var enum2 = EnumerableHelper.AsEnuemrable<object>(source).ToArray();
            var enum3 = enum2.ToEnumerable();
            var collection = arr1.ToCollection();
            collection.AddMany(enum2);
            collection.AddMany(source);
            var list = arr1.ToList();
            list.AddMany(source);
            var arr3 = list.AddFirst(new object());
            var dict = arr1.ToDictionary();
        }

        [TestMethod]
        public void Enumerable4Test()
        {
            var source = Enumerable.Range(0, 16).ToArray();
            Comparison<int> comparison = (i, j) =>
            {
                if (i == j)
                    return 0;
                if (i > j)
                    return 1;
                return -1;
            };
            Assert.IsTrue(source.Contains(5, comparison));
            Assert.IsFalse(source.Contains(-5, comparison));
            Assert.IsFalse(source.Contains(20, comparison));
            Assert.IsTrue(source.Contains(5, (i, j) => i == j));
            Assert.IsFalse(source.Contains(20, (i, j) => i == j));
            var i1 = source.IndexOf(5);
            Assert.AreEqual(i1, 5);

            i1 = source.IndexOf(50);
            Assert.AreEqual(i1, null);

            i1 = source.IndexOf(i => i == 5);
            Assert.AreEqual(i1, 5);

            var source1 = new[] {"Dog", "Cat", "Mouse", null,string.Empty};
            Assert.IsTrue(source1.Contains("Cat"));
            source1 = source1.Except("Cat").ToArray();
            Assert.IsFalse(source1.Contains("Cat"));
            source1 = source1.Except(animal => animal.EqualsTo("dog")).ToArray();
            Assert.IsFalse(source1.Contains("Dog"));
            Assert.AreEqual(source1.CountOf("Mouse"), 1);
        }

        [TestMethod]
        public void ForEachTest()
        {
            var source = Enumerable.Range(0, 16).ToArray();
            var odds = source.ForEachFunc(i => i % 2 == 0).ToArray();
        }
    }
}