using System.Reflection;
using System.Text;

using Library.DesignPatterns.Markers;

namespace UnitTests;

[Trait("Category", nameof(ArchitecturalTests))]
public sealed class ArchitecturalTests
{
    private static (IEnumerable<Type> CoreLibTypes, IEnumerable<Type> CqrsLibTypes, IEnumerable<Type> WebLibTypes, IEnumerable<Type> WpfLibTypes) _libraryTypes;

    public ArchitecturalTests()
    {
        var defaultPredicate = (Type x) => x.Namespace?.StartsWith("System.") is not true;

        var coreLibAsm = typeof(CoreLibModule).Assembly;
        var codeLibTypes = coreLibAsm.GetTypes().Where(defaultPredicate);

        var cqrsLibAsm = typeof(CqrsLibModule).Assembly;
        var cqrsLibTypes = cqrsLibAsm.GetTypes().Where(defaultPredicate);

        var webLibAsm = typeof(WebLibModule).Assembly;
        var webLibTypes = webLibAsm.GetTypes().Where(defaultPredicate);

        var wpfLibAsm = typeof(CoreLibModule).Assembly;
        var wpfLibTypes = wpfLibAsm.GetTypes().Where(defaultPredicate);

        _libraryTypes = new(codeLibTypes, cqrsLibTypes, webLibTypes, wpfLibTypes);
    }

    [Fact(Skip = "Not always is required.")]
    public void EveryClassMustBeAbstractOrSealedOrStatic()
    {
        var types = GetAllTypes().ToArray();
        var notOkTypes = types.Where(x => x is not null and { IsClass: true } and { IsAbstract: false } and { IsSealed: false }).ToArray();
        if (notOkTypes.Length == 0)
        {
            return;
        }
        var result = new StringBuilder("The following classes are not abstract or sealed or static.");
        foreach (var notOkType in notOkTypes)
        {
            _ = result.AppendLine($"Type: `{notOkType}`");
        }
        Assert.Fail(result.ToString());
    }

    [Fact]
    public void FluentShouldBeFluent()
    {
        var methods = GetAllTypes()
            .Where(ObjectHelper.HasAttribute<FluentAttribute>)
            .SelectMany(t => t.GetMethods())
            .Where(x => x?.DeclaringType?.Namespace?.StartsWith("System.") is not true)
            .Where(x => x.Name != "Deconstruct" && !x.Name.StartsWith("set_") && x.IsPublic && x.ReturnType?.Name == "Void");

        if (!methods.Any())
        {
            return;
        }

        var result = new StringBuilder("The following classes are marked as Fluent but they have voided methods.");
        foreach (var method in methods)
        {
            _ = result.AppendLine($"Type: `{method.DeclaringType}`. Method: {method}");
        }
        Assert.Fail(result.ToString());
    }

    [Fact]
    public void HelpersMustBeStatic()
    {
        var types = _libraryTypes.CoreLibTypes;
        var helpers = types.Where(x => IsInNameSpace(x, "Library.Helpers")).Except(x => IsInNameSpace(x, "Library.Helpers.Model"));
        var notSealed = helpers.Where(x => x.IsClass).Where(x => !x.IsSealed);
        Assert.True(!notSealed.Any());
    }

    [Fact]
    public void ImmutableShouldBeImmutable()
    {
        var mutableProperties = getMutableProperties(GetAllTypes());

        if (!mutableProperties.Any())
        {
            return;
        }

        var result = new StringBuilder("The following classes are marked as Immutable but they have muted properties.");
        foreach (var prop in mutableProperties)
        {
            _ = result.AppendLine($"Type: `{prop.DeclaringType}`. Property: {prop}");
        }
        Assert.Fail(result.ToString());

        static IEnumerable<System.Reflection.PropertyInfo> getMutableProperties(IEnumerable<Type> types)
            => types
                .Where(ObjectHelper.HasAttribute<ImmutableAttribute>)
                .SelectMany(t => t.GetProperties())
                .Where(x => x.SetMethod is { } and { IsPublic: true });
    }

    [Fact]
    public void ImmutableShouldBeImmutable_DeepLookIn()
    {
        var immutableTypes = getImmutableTypes(GetAllTypes());
        var mutableProperties = getMutableProperties(immutableTypes);
        if (mutableProperties.Any())
        {
            // Fail
            return;
        }
        var libProps = getLibraryTypeProperties(immutableTypes);
        var mutableTypeProperties = getMutableTypeProperties(libProps);
        foreach (var property in mutableTypeProperties)
        {
            // Fail
            return;
        }

        IEnumerable<Type> getImmutableTypes(IEnumerable<Type> types) 
            => types.Where(ObjectHelper.HasAttribute<ImmutableAttribute>);
        IEnumerable<PropertyInfo> getMutableProperties(IEnumerable<Type> types)
            => types.SelectMany(t => t.GetProperties()).Where(x => x.SetMethod is { } and { IsPublic: true });
        IEnumerable<PropertyInfo> getLibraryTypeProperties(IEnumerable<Type> types) 
            => throw new NotImplementedException();
        IEnumerable<PropertyInfo> getMutableTypeProperties(IEnumerable<PropertyInfo> properties) => throw new NotImplementedException();
    }

    private static IEnumerable<Type> GetAllTypes()
        => EnumerableHelper.Merge(
            _libraryTypes.CoreLibTypes,
            _libraryTypes.CqrsLibTypes,
            _libraryTypes.WebLibTypes,
            _libraryTypes.WpfLibTypes
            );

    private static bool IsInNameSpace(Type type, string ns)
        => type?.Namespace?.StartsWith(ns) is true;
}