using Library.Coding;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest;

[TestClass]
public class IfStatementTest
{
    [TestMethod]
    public void TestMethod1()
    {
        true.If().Then(Methods.Empty).Else(Methods.Empty).Build();
        Assert.That.ItsOk();
    }

    [TestMethod]
    public void TestMethod2()
    {
        var actual = true.If<int>().Then<int>(() => 1).Else<int>(() => 0).Build();
        Assert.AreEqual(1, actual);
    }

    [TestMethod]
    public void TestMethod3()
    {
        var actual = false.If<int>().Then<int>(() => 1).Else<int>(() => 0).Build();
        Assert.AreEqual(0, actual);
    }
}
