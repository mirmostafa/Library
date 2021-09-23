using Library.Globalization;

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
}
