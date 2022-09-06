namespace UnitTest;

[TestClass]
public class ArchitecturalTests
{
    [TestMethod]
    public void MyTestMethod()
    {
        var isInNameSpace = (Type type, string ns) => type?.Namespace?.StartsWith(ns) is true;
        var asm = typeof(StringHelper).Assembly;
        var types = asm.GetTypes();

        var helpers = types.Where(x => isInNameSpace(x, "Library.Helpers") is true && isInNameSpace(x, "Library.Helpers.Model") is false);
        var allSealed = helpers.Where(x => x.IsClass).Where(x => !x.IsSealed).ToList();
        Assert.IsTrue(!allSealed.Any());
    }
}