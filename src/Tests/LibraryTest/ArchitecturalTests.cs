using System.Text;

using Library;
using Library.Cqrs;
using Library.DesignPatterns.Markers;

namespace UnitTest;

[TestClass]
public class ArchitecturalTests
{
    private static (IEnumerable<Type> CoreLibTypes, IEnumerable<Type> CqrsLibTypes, IEnumerable<Type> WebLibTypes, IEnumerable<Type> WpfLibTypes) _libraryTypes;

    [TestMethod]
    public void HelpersMustBeStatic()
    {
        var types = _libraryTypes.CoreLibTypes;
        var helpers = types.Where(x => IsInNameSpace(x, "Library.Helpers")).Except(x => IsInNameSpace(x, "Library.Helpers.Model"));
        var notSealeds = helpers.Where(x => x.IsClass).Where(x => !x.IsSealed).ToList();
        Assert.IsTrue(!notSealeds.Any());
    }

    [TestMethod]
    public void ImmutablesMustBeImmutable()
    {
        var props = GetAllTypes()
                    .Where(ObjectHelper.HasAttribute<ImmutableAttribute>)
                    .SelectMany(t => t.GetProperties())
                    .Where(x => x.SetMethod is { } m and { IsPublic: true });

        if (!props.Any())
        {
            return;
        }
        
        var result = new StringBuilder("The following classes are marked as Immutable but they have muted properties.");
        foreach (var prop in props)
        {
            result.AppendLine($"Type: `{prop.DeclaringType}`. Property: {prop}");
        }
        Assert.Fail(result.ToString());
    }

    [TestInitialize]
    public void InitializeTest()
    {
        var coreLibAsm = typeof(CoreLibModule).Assembly;
        var codeLibTypes = coreLibAsm.GetTypes();

        var cqrsLibAsm = typeof(CqrsLibModule).Assembly;
        var cqrsLibTypes = cqrsLibAsm.GetTypes();

        var webLibAsm = typeof(WebLibModule).Assembly;
        var webLibTypes = webLibAsm.GetTypes();

        var wpfLibAsm = typeof(CoreLibModule).Assembly;
        var wpfLibTypes = wpfLibAsm.GetTypes();

        _libraryTypes = new(codeLibTypes, cqrsLibTypes, webLibTypes, wpfLibTypes);
    }

    private static IEnumerable<Type> GetAllTypes()
    {
        foreach (var type in _libraryTypes.CoreLibTypes)
        {
            yield return type;
        }
        foreach (var type in _libraryTypes.CqrsLibTypes)
        {
            yield return type;
        }
        foreach (var type in _libraryTypes.WebLibTypes)
        {
            yield return type;
        }
        foreach (var type in _libraryTypes.WpfLibTypes)
        {
            yield return type;
        }
    }

    private static bool IsInNameSpace(Type type, string ns)
            => type?.Namespace?.StartsWith(ns) is true;
}