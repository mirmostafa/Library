﻿using Library.Globalization;

namespace LibraryTest;

[TestClass]
public class PersianDateTimeTest
{
    [TestMethod]
    public void DateTimeToPersianTest1()
    {
        var date = new DateTime(2021, 9, 3);
        var pers = PersianDateTime.ParseDateTime(date).ToDateString();
        Assert.AreEqual("1400/06/12", pers);
    }

    [TestMethod]
    public void DateTimeToPersianTest2()
    {
        var date = new DateTime(1977, 9, 3);
        var pers = PersianDateTime.ParseDateTime(date).ToDateString();
        Assert.AreEqual("1356/06/12", pers);
    }

    [TestMethod]
    public void Ctor1Test()
    {
        var pers = new PersianDateTime(1356, 06, 12, 13, 0, 0, 0);
        Assert.AreEqual("1356/06/12 13:00:00", pers.ToString());
    }

    [TestMethod]
    public void LocalizationTest() => Assert.AreEqual("Doshanbeh", PersianDateTime.DaysOfWeek.ElementAt(1));

    [TestMethod]
    public void NowTest()
        => Assert.AreEqual(PersianDateTime.Now.ToDateTime().ToString(), DateTime.Now.ToString());

    [TestMethod]
    public void PersianWeekDaysTest()
        => Assert.AreEqual(DayOfWeek.Friday, PersianDateTime.PersianWeekDays.ElementAt(6).Day);

    [TestMethod]
    public void PersianWeekDaysAbbrTest()
        => Assert.AreEqual(DayOfWeek.Friday, PersianDateTime.PersianWeekDaysAbbrs.ElementAt(6).Day);

    [TestMethod]
    public void DayOfWeekTest()
        => Assert.AreEqual(DateTime.Now.DayOfWeek.ToInt(), PersianDateTime.Now.DayOfWeek.ToInt());

    [TestMethod]
    public void ParsePersianTest()
    {
        var date = PersianDateTime.TryParsePersian("1400/7/10").Result;
        Assert.AreEqual("1400/07/10", date.ToDateString());
    }

    [TestMethod]
    public void ParseEnglishTest()
    {
        var date = PersianDateTime.ParseEnglish("2021/10/2");
        Assert.AreEqual("1400/07/10", date.ToDateString());
    }

    [TestMethod]
    public void DayOfWeekTitleTest()
    {
        var date = PersianDateTime.ParsePersian("1400/7/10 17:18:19");
        var actual = date.DayOfWeekTitle;
        Assert.AreEqual("Shanbe", actual);
    }

    [TestMethod]
    public void PersianCultureInfoTest()
    {
        var culture = new PersianCultureInfo();
        Assert.IsNotNull(culture);
        Assert.IsNotNull(culture.Calendar);
    }

    [TestMethod]
    public void DateTimeToPersianTest()
    {
        var actual = PersianDateTime.ToPersian(DateTime.Parse("2021/10/2"));

        Assert.AreEqual("1400/07/10 00:00:00 AM", actual);
    }

    [TestMethod]
    public void DateTimeToPersianTest3()
    {
        var actual = PersianDateTime.ToPersian(DateTime.Parse("2021/10/2 5:4:3"));

        Assert.AreEqual("1400/07/10 05:04:03 AM", actual);
    }

    [TestMethod]
    public void DateTimeToPersianTest4()
    {
        var actual = PersianDateTime.ToPersian(DateTime.Parse("2021/10/2 15:4:3"));

        Assert.AreEqual("1400/07/10 03:04:03 PM", actual);
    }
}
