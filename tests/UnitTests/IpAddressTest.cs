namespace UnitTests;

public class IpAddressTest
{
    public int[] SumIPSegments(int[] segment1, int[] segment2)
    {
        var result = new int[4];
        var carry = 0;

        for (var i = 3; i >= 0; i--)
        {
            var sum = segment1[i] + segment2[i] + carry;
            result[i] = sum % 256;
            carry = sum / 256;
        }

        return result;
    }

    [Theory]
    [InlineData(new[] { 192, 168, 0, 1 }, new[] { 10, 0, 0, 1 }, new[] { 202, 168, 0, 2 })]
    [InlineData(new[] { 0, 0, 0, 255 }, new[] { 0, 0, 0, 3 }, new[] { 0, 0, 1, 2 })]
    //[InlineData(new[] { 255, 255, 255, 255 }, new[] { 0, 0, 0, 1 }, new[] { 0, 0, 0, 1 })]
    [InlineData(new[] { 128, 0, 0, 0 }, new[] { 0, 0, 0, 128 }, new[] { 128, 0, 0, 128 })]
    public void SumIPSegments_ValidInput_ReturnsExpectedResult(int[] segment1, int[] segment2, int[] expectedResult)
    {
        // Arrange

        // Act
        var result = this.SumIPSegments(segment1, segment2);

        // Assert
        Assert.Equal(expectedResult, result);
    }
}