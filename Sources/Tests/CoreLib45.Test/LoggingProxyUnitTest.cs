#region Code Identifications

// Created on     2018/03/10
// Last update on 2018/03/10 by Mohammad Mir mostafa 

#endregion

using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mohammad.Helpers;
using Mohammad.Runtime.Proxies;

namespace CoreLib45.Test
{
    /// <summary>
    ///     Summary description for LoggingProxyUnitTest
    /// </summary>
    [TestClass]
    public class LoggingProxyUnitTest
    {
        /// <summary>
        ///     Gets or sets the test context which provides
        ///     information about and functionality for the current test run.
        /// </summary>
        public TestContext TestContext { get; set; }

        [TestMethod]
        public void TestMethod1()
        {
            var person = LoggingProxy<Person>.Create(new Person("Ali"),
                e => e.Arguments.ForEach((n, _) => Trace.WriteLine($"{n.Name} : {n.Value}\n\r", "CoreLib Unit Testing")));
            person.Age = 30;
            person.Address = "Here!";
        }

        #region Additional test attributes

        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassInitialize to run code before running the first test in the class
        // [ClassInitialize()]
        // public static void MyClassInitialize(TestContext testContext) { }
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //

        #endregion
    }
}