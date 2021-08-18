using Library.Coding;

namespace LibraryTest;
[TestClass]
public class CodeRunnerTest
{
    [TestMethod]
    public void CodeTest()
    {
        var i = 0;
        void add() => i++;
        CodeRunner
            .StartWith(add)
            .Then(add)
            .Then(add)
            .Then(add)
            .Then(add)
            .Run();
        Assert.AreEqual(5, i);
    }
}
