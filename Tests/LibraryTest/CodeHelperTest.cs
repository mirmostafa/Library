using Library.Coding;
using Library.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LibraryTest;

[TestClass]
public class CodeHelperTest
{
    [TestMethod]
    [ExpectedException(typeof(BreakException))]
    public void BreakTest()
    {
        int zero = 0;
        if (zero == 0)
            CodeHelper.Break();
        var invalid = 5 / zero;
    }
}
