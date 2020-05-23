#region Code Identifications

// Created on     2018/07/25
// Last update on 2018/07/25 by Mohammad Mir mostafa 

#endregion

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mohammad.Helpers;

namespace CoreLib45.Test.HelpersTests
{
    [TestClass]
    public class ImmutableTypeInitializerTest
    {
        [TestMethod]
        public void TestDynamic()
        {
            dynamic a1 = new ImmutableTypeInitializer<Person>();
            a1.Name = "Ali";
            a1.Age = 5;
            a1.Age = 15;
            Person p1 = a1;
            Assert.AreEqual(p1.Name, "Ali");
        }

        [TestMethod]
        public void TestObjectInitializer()
        {
            var a2 = new ImmutableTypeInitializer<Person>
            {
                ["Name"] = "Ali",
                ["Age"] = 15
            };
            Person p2 = a2;
            Assert.AreEqual(p2.Name, "Ali");
        }

        [TestMethod]
        public void TestObjectOriented()
        {
            var p2 = new ImmutableTypeInitializer<Person>
            {
                ["Name"] = "Ali",
                ["Age"] = 15
            }.Build();
            Assert.AreEqual(p2.Name, "Ali");
        }

        [TestMethod]
        public void TestMethodChain()
        {
            var p2 = new ImmutableTypeInitializer<Person>().CtorParam("Name", "Ali").CtorParam("Age", 5).Build();
            Assert.AreEqual(p2.Name, "Ali");
        }

        [TestMethod]
        public void TestFunctional()
        {
            Person p2 = ImmutableTypeInitializer<Person>.NewDynamic().SetName("Ali").SetAge(5).Build();
            Assert.AreEqual(p2.Name, "Ali");
        }
    }
}