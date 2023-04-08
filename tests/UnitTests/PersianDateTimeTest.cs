using Library.Globalization;

namespace UnitTests;


public sealed class PersianDateTimeTest
{
    [Theory]
    [InlineData(2021, 9, 3, "1400/06/12")]
    [InlineData(1977, 9, 3, "1356/06/12")]
    [InlineData(2020, 1, 31, "1398/11/11")]
    public void DateTimeToPersianDate(int year, int month, int day, string expected)
    {
        var date = new DateTime(year, month, day);
        var actual = PersianDateTime.ParseDateTime(date).ToDateString();
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(1356, 06, 12, 01, 00, 00, "1356/06/12 01:00:00 AM")]
    [InlineData(1357, 07, 13, 10, 00, 00, "1357/07/13 10:00:00 AM")]
    [InlineData(1358, 09, 14, 13, 00, 00, "1358/09/14 01:00:00 PM")]
    public void DateTimeToPersianDateTime(int year, int month, int day, int hour, int min, int sec, string expected)
    {
        var actual = new PersianDateTime(year, month, day, hour, min, sec, 0);
        Assert.Equal(expected, actual.ToString());
    }

    [Fact]
    public void LocalizationTest()
        => Assert.Equal("Doshanbeh", PersianDateTime.DaysOfWeek.ElementAt(1));

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
        => Assert.Equal(DateTime.Now.DayOfWeek.Cast().ToInt(), PersianDateTime.Now.DayOfWeek.Cast().ToInt());

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
