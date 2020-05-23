#region Code Identifications

// Created on     2017/08/07
// Last update on 2017/11/04 by Mohammad Mir mostafa 

#endregion

using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mohammad.DesignPatterns.Creational;
using Mohammad.DesignPatterns.Creational.Exceptions;
using Mohammad.Helpers;
using Mohammad.Validation;
using Mohammad.Validation.Exceptions;

namespace CoreLib45.Test
{
    [TestClass]
    public class SingletonTests
    {
        [TestMethod]
        public void InstanceNotNullTest()
        {
            Assert.AreNotEqual(SingletonTest1.Instance, null);
        }

        [TestMethod]
        [ExpectedException(typeof(NotNullOrZeroValidationException))]
        public void TestValiatorSingletonWithException()
        {
            Validator.Instance.AssertNotDefault(0, "Number");
        }

        [TestMethod]
        public void TestValiatorSingletonWithNoException()
        {
            Validator.Instance.AssertNotDefault(1, "Number");
        }

        [TestMethod]
        public void TestInterfaceSingleton2()
        {
            Assert.AreNotEqual(SingletonTest2.Instance, null);
        }

        [TestMethod]
        public void TestInterfaceSingleton3()
        {
            Assert.AreNotEqual(SingletonTest3.Instance, null);
        }

        [TestMethod]
        [ExpectedException(typeof(SingletonException))]
        public void TestInterfaceSingleton4()
        {
            Assert.AreNotEqual(SingletonTest4.Instance, null);
        }

        [TestMethod]
        public void TestInterfaceSingleton5()
        {
            Assert.AreNotEqual(SingletonTest5.Instance, null);
        }

        [TestMethod]
        public void TestInterfaceSingleton6()
        {
            Assert.AreNotEqual(SingletonTest6.Instance.Fake(), null);
        }
    }

    internal class SingletonTest1 : Singleton<SingletonTest1>
    {
        protected SingletonTest1() { }
    }

    internal class SingletonTest2 : Singleton<SingletonTest2>
    {
        private static SingletonTest2 CreateInstance() => new SingletonTest2();
    }

    internal class SingletonTest3 : ISingleton<SingletonTest3>
    {
        private static readonly Lazy<SingletonTest3> _Instance = ObjectHelper.GenerateLazySingletonInstance<SingletonTest3>();

        public static SingletonTest3 Instance => _Instance.Value;
        private SingletonTest3() { }
    }

    internal class SingletonTest4 : ISingleton<SingletonTest4>
    {
        private static readonly Lazy<SingletonTest4> _Instance =
            new Lazy<SingletonTest4>(() => ObjectHelper.GenerateSingletonInstance<SingletonTest4>());

        public static SingletonTest4 Instance => _Instance.Value;
    }

    internal class SingletonTest5 : ISingleton<SingletonTest5>
    {
        private static readonly Lazy<SingletonTest5> _Instance =
            new Lazy<SingletonTest5>(() => ObjectHelper.GenerateSingletonInstance(CreateInstance));

        public static SingletonTest5 Instance => _Instance.Value;
        public static SingletonTest5 CreateInstance() => new SingletonTest5();
    }

    internal class SingletonTest6Base<T> : Singleton<T>
        where T : class, ISingleton<T>
    {
        protected SingletonTest6Base() { }
    }

    internal class SingletonTest6 : SingletonTest6Base<SingletonTest6>
    {
        private SingletonTest6()
        {
            
        }
        public string Fake() => "Ali";
    }
}