namespace UnitTests;
#endregion

public class NumberHelperTest
{
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
        string result = number.ToString(format, defaultValue);

        // Assert
        Assert.Equal(expected, result);
    }
}