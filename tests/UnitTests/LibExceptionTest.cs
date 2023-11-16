using Library.Exceptions;

namespace UnitTests;

[Trait("Category", nameof(Library.Exceptions))]
public class LibExceptionTest
{
    [Fact]
    public void ThrowableExceptionNoParam()
        => Assert.Throws<NoItemSelectedException>(NoItemSelectedException.Throw);

    [Fact]
    public void ThrowableExceptionTest()
    {
        var ex = Assert.Throws<NoItemSelectedException>(() => NoItemSelectedException.Throw("message"));
        Assert.Equal("message", ex.Message);
    }
}