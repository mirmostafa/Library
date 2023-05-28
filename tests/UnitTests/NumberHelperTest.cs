namespace UnitTests;

public class NumberHelperTest
{
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

    [Fact]
    public void RandomNumber_WithNullMax_ThrowsNotSupportedException()
    {
        // Arrange
        int? min = 0, max = null;

        // Act & Assert
        _ = Assert.Throws<NotSupportedException>(() => NumberHelper.RandomNumber(min, max));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(10)]
    public void RandomNumbers_NoMinMax_ReturnsRandomNumbers(int count)
    {
        // Arrange

        // Act
        var result = NumberHelper.RandomNumbers(count);

        // Assert
        Assert.Equal(count, result.Count());
        Assert.All(result, num => Assert.InRange(num, int.MinValue, int.MaxValue));
    }

    [Theory]
    [InlineData(0, 0, 10)]
    [InlineData(1, -5, 5)]
    [InlineData(10, int.MinValue, int.MaxValue)]
    public void RandomNumbers_WithMinMax_ReturnsRandomNumbersInRange(int count, int min, int max)
    {
        // Arrange

        // Act
        var result = NumberHelper.RandomNumbers(count, min, max);

        // Assert
        Assert.Equal(count, result.Count());
        Assert.All(result, num => Assert.InRange(num, min, max));
    }

    [Fact]
    public void RandomNumbers_WithNullMinMax_ThrowsNotSupportedException()
    {
        // Arrange
        int? min = 0, max = null;
        var count = 10;

        // Act & Assert
        _ = Assert.Throws<NotSupportedException>(() => NumberHelper.RandomNumbers(count, min, max).Build());
    }

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