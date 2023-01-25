using Library.Coding;

namespace Library.UnitTest;

[TestClass]
public class ArgsTest
{
    [TestMethod]
    public void MyTestMethod()
    {
        var sum = (Args<int> nums) => nums.Sum();
        var r1 = sum(1);
        var r2 = sum((1, 2));
        var r3 = sum((1, 2, 3));
        var r4 = sum((1, 2, 3, 4));

        Assert.AreEqual(1, r1);
        Assert.AreEqual(3, r2);
        Assert.AreEqual(6, r3);
        Assert.AreEqual(10, r4);
    }

    [TestMethod]
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
    [TestMethod]
    public void MyTestMethod3()
    {
        var display = (Args<int> nums) =>
        {
            For(nums, (int item, int _) => Console.WriteLine(item));
        };
        display((1, 2, 3, 4, 5));
    }
    [TestMethod]
    public void MyTestMethod4()
    {
        var display = (Args<int> nums) =>
        {
            For(nums, (int item, int _) => Console.WriteLine(item));
        };
        display((1, 2, 3, 4, 5, 6, 7));
        Assert.AreEqual(2, new Args<int>(1, 2).Count);
        Assert.AreEqual(2, new Args<int>(1, 2)[1]);
    }
}
