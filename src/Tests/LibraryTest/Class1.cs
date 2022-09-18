using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest;
[TestClass]
public class MyTestClass1
{
    [TestMethod]
    public void MyTestMethod()
    {
        var o = new Object();
        var actual = o.Fluent().ConvertTo();
        Assert.AreEqual(o, actual);
    }

    [TestMethod]
    public void MyTestMethod1()
    {
        var o1 = 5;
        var o2 = o1.Fluent(x => FluencyHelper.Convert<int>(o1, x)).GetValue();
        Assert.AreEqual(o1, o2);
    }
}