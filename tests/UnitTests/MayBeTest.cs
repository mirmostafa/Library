using Library.DesignPatterns;

namespace UnitTests;

public class MayBeTest
{
    [Fact]
    public void Valid()
    {
        string? name = null;
        var actual = name.Maybe()
            .Do(x => string.Concat(x, "mm"))
            .Do(x => string.Concat(x, "ad"));
        Assert.Null(actual.Value); ;
    }

    [Fact]
    public void Invalid()
    {
        var name = "Moha";
        var game = name.Maybe()
            .Do(x => string.Concat(x, "mm"))
            .Do(x => string.Concat(x, "ad"));
        Assert.Equal("Mohammad", game);
    }
}