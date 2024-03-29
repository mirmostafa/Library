﻿namespace UnitTests;

[Trait("Category", nameof(Library.Helpers))]
[Trait("Category", nameof(NumberHelper))]
public class NumberHelperTest
{
    [Theory]
    [InlineData(5, 1, 10, true)]
    [InlineData(0, -10, 10, true)]
    [InlineData(100, 0, 200, true)]
    [InlineData(0, 0, 200, true)]
    [InlineData(200, 0, 200, true)]
    [InlineData(-5, 1, 10, false)]
    [InlineData(-100, 0, 200, false)]
    [InlineData(15, 1, 10, false)]
    [InlineData(1000, 0, 200, false)]
    public void IsBetween_NumberInRange_ReturnsTrue(int num, int min, int max, bool expected)
    {
        // Arrange

        // Act
        var result = num.IsBetween(min, max);

        // Assert
        Assert.Equal(result, expected);
    }

    [Fact]
    public void RandomNumber_NoMinMax_ReturnsRandomNumber()
    {
        // Arrange

        // Act
        var result = NumberHelper.RandomNumber();

        // Assert
        Assert.InRange(result, int.MinValue, int.MaxValue);
    }

    [Theory]
    [InlineData(0, 10)]
    [InlineData(-5, 5)]
    [InlineData(int.MinValue, int.MaxValue)]
    public void RandomNumber_WithMinMax_ReturnsRandomNumberInRange(int min, int max)
    {
        // Arrange

        // Act
        var result = NumberHelper.RandomNumber(min, max);

        // Assert
        Assert.InRange(result, min, max);
    }

    [Theory]
    [InlineData(0, 10, 1, new int[] { 0, 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 })]
    [InlineData(10, 0, -1, new int[] { 10, 9, 8, 7, 6, 5, 4, 3, 2, 1, 0 })]
    public void Range_ReturnsExpectedValues(int start, int end, int step, int[] expectedValues)
    {
        var actualValues = NumberHelper.Range(start, end, step).ToArray();
        Assert.Equal(expectedValues, actualValues);
    }

    [Fact]
    public void Range_ThrowsArgumentException_ForNegativeStepAndStartSmallerThanEnd()
        => Assert.Throws<Library.Exceptions.InvalidArgumentException>(() => NumberHelper.Range(0, 10, -1).ToArray());

    [Fact]
    public void Range_ThrowsArgumentException_ForPositiveStepAndStartLargerThanEnd()
        => Assert.Throws<Library.Exceptions.InvalidArgumentException>(() => NumberHelper.Range(10, 0, 1).ToArray());

    [Fact]
    public void Range_ThrowsArgumentOutOfRangeException_ForZeroStep()
        => Assert.Throws<ArgumentOutOfRangeException>(() => NumberHelper.Range(0, 10, 0).ToArray());


    [Fact]
    public void Range_WithEndOnly_ThrowsArgumentException_ForNegativeStepAndStartSmallerThanEnd()
        => Assert.Throws<Library.Exceptions.InvalidArgumentException>(() => NumberHelper.Range(0, 10, -1).ToArray());

    [Fact]
    public void Range_WithEndOnly_ThrowsArgumentException_ForPositiveStepAndStartLargerThanEnd()
        => Assert.Throws<Library.Exceptions.InvalidArgumentException>(() => NumberHelper.Range(10, 0, 1).ToArray());

    [Fact]
    public void Range_WithEndOnly_ThrowsArgumentOutOfRangeException_ForZeroStep()
        => Assert.Throws<ArgumentOutOfRangeException>(() => NumberHelper.Range(0, 10, 0).ToArray());

    [Theory]
    [InlineData(null, "0", 0, "0")]
    [InlineData(null, "N/A", 0, "N/A")]
    [InlineData(123, "000", 0, "123")]
    [InlineData(123, "00000", 0, "00123")]
    [InlineData(123, "0,0", 0, "123")]
    [InlineData(123456789, "0,0", 0, "123,456,789")]
    public void ToStringTest(int? number, string format, int defaultValue, string expected)
    {
        // Arrange

        // Act
        var result = number.ToString(format, defaultValue);

        // Assert
        Assert.Equal(expected, result);
    }
}