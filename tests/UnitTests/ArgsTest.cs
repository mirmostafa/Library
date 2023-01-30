using Library.Coding;

namespace UnitTests;

[Trait("Category", "Code Helpers")]
public class ArgsTest
{
    [Fact]
    public void MyTestMethod()
    {
        var sum = (Args<int> nums) => nums.Sum();
        var r1 = sum(1);
        var r2 = sum((1, 2));
        var r3 = sum((1, 2, 3));
        var r4 = sum((1, 2, 3, 4));

        Assert.Equal(1, r1);
        Assert.Equal(3, r2);
        Assert.Equal(6, r3);
        Assert.Equal(10, r4);
    }

    [Fact]
    public void MyTestMethod1()
    {
        var display = (Args<int> nums) =>
        {
            using var enumerator = nums.GetEnumerator();
            While(enumerator.MoveNext, () => Console.WriteLine(enumerator.Current));
        };
        display(1);
        display((1, 2, 3, 4, 5));
    }

    [Fact]
    public void MyTestMethod3()
    {
        var display = (Args<int> nums) =>
        {
            For(nums, (int item, int _) => Console.WriteLine(item));
        };
        display((1, 2, 3, 4, 5));
    }

    [Fact]
    public void MyTestMethod4()
    {
        var display = (Args<int> nums) =>
        {
            For(nums, (int item, int _) => Console.WriteLine(item));
        };
        display((1, 2, 3, 4, 5, 6, 7));
        Assert.Equal(2, new Args<int>(1, 2).Count);
        Assert.Equal(2, new Args<int>(1, 2)[1]);
    }
}