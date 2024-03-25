using Library.Coding;

using Xunit.Abstractions;

namespace UnitTests;

[Trait("Category", nameof(Library.Coding))]
[Obsolete("Subject to remove", true)]
public sealed class ArgsTest
{
    private readonly ITestOutputHelper _output;

    public ArgsTest(ITestOutputHelper output) => this._output = output;

    public static IEnumerable<object[]> Data => new[] {
        new object[] { new Args<int>(1), 1 },
        new object[] { new Args<int>(1, 2), 3 },
        new object[] { new Args<int>(1, 2,3), 6 },
        new object[] { new Args<int>(1, 2, 3, 4), 10 }
        };

    [Theory(Skip =$"Subject to remote taste case: {nameof(Args<int>)}")]
    [MemberData(nameof(Data))]
    public void MyTestMethod(Args<int> nums, int expected)
    {
        var sum = (Args<int> n) => n.Sum();
        var actual = sum(nums);

        Assert.Equal(expected, actual);
    }

    [Fact(Skip = $"Subject to remote taste case: {nameof(Args<int>)}")]
    public void MyTestMethod1()
    {
        var display = (Args<int> nums) =>
        {
            using var enumerator = nums.GetEnumerator();
            While(enumerator.MoveNext, () => this.WriteLine(enumerator.Current));
        };
        display(1);
        display((1, 2, 3, 4, 5));
    }

    [Fact(Skip = $"Subject to remote taste case: {nameof(Args<int>)}")]
    public void MyTestMethod3()
    {
        var display = (Args<int> nums) =>
        {
            For(nums, (int item, int _) => this.WriteLine(item));
        };
        display((1, 2, 3, 4, 5));
    }

    [Fact(Skip = $"Subject to remote taste case: {nameof(Args<int>)}")]
    public void MyTestMethod4()
    {
        var display = (Args<int> nums) =>
        {
            For(nums, (int item, int _) => this.WriteLine(item));
        };
        display((1, 2, 3, 4, 5, 6, 7));
        Assert.Equal(2, new Args<int>(1, 2).Count);
        Assert.Equal(2, new Args<int>(1, 2)[1]);
    }

    private void WriteLine(object? o)
        => this._output.WriteLine(o?.ToString() ?? string.Empty);
}