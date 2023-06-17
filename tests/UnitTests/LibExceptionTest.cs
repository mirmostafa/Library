using Library.Exceptions;
using Library.Interfaces;

namespace UnitTests;

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

    [Fact]
    public void ThrowsExceptionNoParam()
        => Assert.Throws<NoItemSelectedException>(IThrowsException<NoItemSelectedException>.Throw);

    [Fact]
    public void ThrowsExceptionWithMessage()
    {
        var ex = Assert.Throws<NoItemSelectedException>(() => IThrowsException<NoItemSelectedException>.Throw("message"));
        Assert.Equal("message", ex.Message);
    }
}