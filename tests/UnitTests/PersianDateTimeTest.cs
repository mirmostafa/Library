using Library.Globalization;

namespace UnitTests;


public class PersianDateTimeTest
{
    [Fact]
    public void DateTimeToPersianTest1()
    {
        var date = new DateTime(2021, 9, 3);
        var pers = PersianDateTime.ParseDateTime(date).ToDateString();
        Assert.Equal("1400/06/12", pers);
    }

    [Fact]
    public void DateTimeToPersianTest2()
    {
        var date = new DateTime(1977, 9, 3);
        var pers = PersianDateTime.ParseDateTime(date).ToDateString();
        Assert.Equal("1356/06/12", pers);
    }

    [Fact]
    public void Ctor1Test()
    {
        var pers = new PersianDateTime(1356, 06, 12, 13, 0, 0, 0);
        Assert.Equal("1356/06/12 01:00:00 PM", pers.ToString());
    }

    [Fact]
    public void LocalizationTest() => Assert.Equal("Doshanbeh", PersianDateTime.DaysOfWeek.ElementAt(1));

    [Fact]
    public void NowTest()
        => Assert.Equal(PersianDateTime.Now.ToDateTime().ToString(), DateTime.Now.ToString());

    [Fact]
    public void PersianWeekDaysTest()
        => Assert.Equal(DayOfWeek.Friday, PersianDateTime.PersianWeekDays.ElementAt(6).Day);

    [Fact]
    public void PersianWeekDaysAbbrTest()
        => Assert.Equal(DayOfWeek.Friday, PersianDateTime.PersianWeekDaysAbbrs.ElementAt(6).Day);

    [Fact]
    public void DayOfWeekTest()
        => Assert.Equal(DateTime.Now.DayOfWeek.ToInt(), PersianDateTime.Now.DayOfWeek.ToInt());

    [Fact]
    public void ParsePersianTest()
    {
        var date = PersianDateTime.TryParse("1400/7/10").Value;
        Assert.Equal("1400/07/10", date.ToDateString());
    }

    [Fact]
    public void ParseEnglishTest()
    {
        var date = PersianDateTime.ParseEnglish("2021/10/2");
        Assert.Equal("1400/07/10", date.ToDateString());
    }

    [Fact]
    public void DayOfWeekTitleTest()
    {
        var date = PersianDateTime.ParsePersian("1400/7/10 17:18:19");
        var actual = date.DayOfWeekTitle;
        Assert.Equal("Shanbe", actual);
    }

    [Fact]
    public void PersianCultureInfoTest()
    {
        var culture = new PersianCultureInfo();
        Assert.NotNull(culture);
        Assert.NotNull(culture.Calendar);
    }

    [Fact]
    public void DateTimeToPersianTest()
    {
        var actual = PersianDateTime.ToPersian(DateTime.Parse("2021/10/2"));

        Assert.Equal("1400/07/10 00:00:00 AM", actual);
    }

    [Fact]
    public void DateTimeToPersianTest3()
    {
        var actual = PersianDateTime.ToPersian(DateTime.Parse("2021/10/2 5:4:3"));

        Assert.Equal("1400/07/10 05:04:03 AM", actual);
    }

    [Fact]
    public void DateTimeToPersianTest4()
    {
        var actual = PersianDateTime.ToPersian(DateTime.Parse("2021/10/2 15:4:3"));

        Assert.Equal("1400/07/10 03:04:03 PM", actual);
    }
}
