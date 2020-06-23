// Created on     2018/03/10
// Last update on 2018/03/10 by Mohammad Mir mostafa 

using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mohammad.Helpers;

namespace CoreLib45.Test.HelpersTests
{
    [TestClass]
    public class StringHelperTest
    {
        [TestMethod]
        public void IsPersianTest()
        {
            Assert.AreEqual("سلام".IsPersian(), true);
            Assert.AreEqual("Hi".IsPersian(), false);
        }

        [TestMethod]
        public void HasPersianTest()
        {
            Assert.AreEqual("Hi, سلام".HasPersian(), true);
            Assert.AreEqual("Hi".IsPersian(), false);
        }

        [TestMethod]
        public void IsNullOrEmptyTest()
        {
            Assert.AreEqual("".IsNullOrEmpty(), true);
            string s = null;
            Assert.AreEqual(s.IsNullOrEmpty(), true);
            Assert.AreEqual("`".IsNullOrEmpty(), false);
        }

        [TestMethod]
        public void GetPhraseTest()
        {
            if ("I:'Hi'He:'How is going?'I:'Just fine.'".GetPhrases('\'').Count() != 3)
            {
                Assert.Fail("GetPhrases result corrupted.");
            }
        }

        [TestMethod]
        public void GetKeyValuesTest()
        {
            const string str = "Key1=Val1;Key2=Val2;Key3=Val3;Key4=Val4;Key5=Val5";
            if (str.GetKeyValues().Count() != 5)
            {
                Assert.Fail("GetKeyValues result corrupted.");
            }
        }

        [TestMethod]
        public void IfTest()
        {
            Assert.AreEqual("".If(string.IsNullOrEmpty, true, false), true);
            Assert.AreEqual("".If(string.IsNullOrEmpty, _ => true, _ => false), true);
            "".If(string.IsNullOrEmpty,
                _ =>
                {
                },
                _ =>
                {
                });
            "Test".If(string.IsNullOrEmpty,
                _ =>
                {
                },
                _ =>
                {
                });
        }
    }
}