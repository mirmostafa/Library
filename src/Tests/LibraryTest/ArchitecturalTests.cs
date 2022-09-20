using System.Diagnostics;

namespace UnitTest;

[TestClass]
public class ArchitecturalTests
{
    [TestMethod]
    public void HelpersMustBeStatic()
    {
        var asm = typeof(StringHelper).Assembly;
        var types = asm.GetTypes();
        var isInNameSpace = (Type type, string ns) => type?.Namespace?.StartsWith(ns) is true;

        var helpers = types.Where(x => isInNameSpace(x, "Library.Helpers") is true && isInNameSpace(x, "Library.Helpers.Model") is false);
        var allSealed = helpers.Where(x => x.IsClass).Where(x => !x.IsSealed).ToList();
        Assert.IsTrue(!allSealed.Any());
    }

    [TestMethod]
    public void TypesMustBeSealed()
    {
        var asm = typeof(StringHelper).Assembly;
        var types = asm.GetTypes();
        var offenders = types.Where(x => !x.IsInterface && !x.IsInstanceOfType(typeof(object))).Where(x => !x.IsSealed);
        Console.WriteLine("Hello");
        Debug.WriteLine("Hi");
        Assert.IsFalse(offenders.Any());
    }
}