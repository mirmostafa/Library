using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace UnitTest;
[TestClass]
public class CultureInfoHelperTestClass
{
    [TestMethod]
    public void GetCountryEnglishNameTest()
    {
        var expected = "United States";
        var actual = CultureInfo.GetCultureInfo(1033).GetCountryEnglishName();
        Assert.AreEqual(expected, actual);
    }

    [TestMethod]
    public void GetCountryEnglishNameTest01()
    {
        var expected = "Iran";
        var actual = CultureInfo.GetCultureInfo("fa-Ir").GetCountryEnglishName();
        Assert.AreEqual(expected, actual);
    }
}