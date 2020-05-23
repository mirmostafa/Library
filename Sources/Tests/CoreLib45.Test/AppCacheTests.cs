#region Code Identifications

// Created on     2018/04/21
// Last update on 2018/04/21 by Mohammad Mir mostafa 

#endregion

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mohammad.Primitives;

namespace CoreLib45.Test
{
    [TestClass]
    public class AppCacheTests
    {
        private const string UNITTEST_OBJECT = "UnitTest Object";

        [TestMethod]
        public void Test1()
        {
            AppCache.Set(UNITTEST_OBJECT, 5);
        }

        [TestMethod]
        public void Test2()
        {
            Assert.AreEqual(AppCache.Get(UNITTEST_OBJECT), 5);
        }

        [TestMethod]
        public void Test3()
        {
            Assert.AreEqual(AppCache.Get<int>(UNITTEST_OBJECT), 5);
        }

        [TestMethod]
        public void Test4()
        {
            Assert.AreEqual(AppCache.Get(UNITTEST_OBJECT, () => 5), 5);
        }

        [TestMethod]
        public void Test5()
        {
            Assert.AreEqual(AppCache.Get("Fake", () => 5), 5);
        }

        [TestMethod]
        public void Test6()
        {
            const string tmpKey = "Test 6";
            Assert.AreEqual(new AppCache
                {
                    [tmpKey] = 5
                }[tmpKey],
                5);
        }
    }
}