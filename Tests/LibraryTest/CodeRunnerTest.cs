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
        Assert.AreEqual(5, i);
    }
}
