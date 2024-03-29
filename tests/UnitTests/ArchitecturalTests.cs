﻿using System.Reflection;
using System.Text;

using Library.DesignPatterns.Markers;

namespace UnitTests;

[Trait("Category", nameof(ArchitecturalTests))]
public sealed class ArchitecturalTests
{
    private static (IEnumerable<Type> CoreLibTypes, IEnumerable<Type> CqrsLibTypes, IEnumerable<Type> WebLibTypes, IEnumerable<Type> WpfLibTypes) _libraryTypes;

    public ArchitecturalTests()
    {
        var coreLibAsm = typeof(CoreLibModule).Assembly;
        var codeLibTypes = coreLibAsm.GetTypes().Where(defaultPredicate);

        var cqrsLibAsm = typeof(CqrsLibModule).Assembly;
        var cqrsLibTypes = cqrsLibAsm.GetTypes().Where(defaultPredicate);

        var webLibAsm = typeof(WebLibModule).Assembly;
        var webLibTypes = webLibAsm.GetTypes().Where(defaultPredicate);

        var wpfLibAsm = typeof(CoreLibModule).Assembly;
        var wpfLibTypes = wpfLibAsm.GetTypes().Where(defaultPredicate);

        _libraryTypes = new(codeLibTypes, cqrsLibTypes, webLibTypes, wpfLibTypes);

        static bool defaultPredicate(Type x) => x.Namespace?.StartsWith("System.") is not true;
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
        var notSealed = helpers.Where(x => x.IsClass).Where(x => !x.IsSealed && !x.IsAbstract).ToArray();
        if(notSealed.Any())
        {
            var result = new StringBuilder();
            result.AppendLine("The following classes are not `sealed` or `abstract` or `static`.");
            foreach (var type in notSealed)
            {
                _ = result.AppendLine($"Type: `{type}`");
            }
            Assert.Fail(result.ToString());
        }
    }

    [Fact]
    public void ImmutableShouldBeImmutable()
    {
        var immutableTypes = GetAllTypes().Where(ObjectHelper.HasAttribute<ImmutableAttribute>);
        RuleNo1(immutableTypes);
        RuleNo2(immutableTypes);

        static void RuleNo1(IEnumerable<Type> immutableTypes)
        {
            var mutableProperties = immutableTypes.SelectMany(t => t.GetProperties()).Where(x => (x.SetMethod?.IsPublic ?? false) && !x.IsSetMethodInit()).ToArray();
            if (mutableProperties.Any())
            {
                var result = new StringBuilder();
                result.AppendLine("The following classes are marked as Immutable but they have muted properties.");
                foreach (var prop in mutableProperties)
                {
                    _ = result.AppendLine($"Type: `{prop.DeclaringType}`. Property: {prop}");
                }
                Assert.Fail(result.ToString());
            }
        }

        static void RuleNo2(IEnumerable<Type> immutableTypes)
        {
            var notReadOnlyFields = immutableTypes.SelectMany(x => x.GetFields()).Where(x => !x.IsInitOnly);
            var constFields = immutableTypes.SelectMany(x => x.GetFields(BindingFlags.Static)).Where(x => x.IsLiteral && !x.IsInitOnly);
            notReadOnlyFields = notReadOnlyFields.Except(constFields);

            if (notReadOnlyFields.Any())
            {
                var result = new StringBuilder();
                result.AppendLine("The following classes are marked as Immutable but they have non-readonly fields.");
                foreach (var field in notReadOnlyFields)
                {
                    _ = result.AppendLine($"Type: `{field.DeclaringType}`. Field: {field}");
                }
                Assert.Fail(result.ToString());
            }
        }
    }

    [Fact]
    public void ImmutableShouldBeImmutableDeepLookIn()
    {
        var immutableTypes = getImmutableTypes(GetAllTypes());
        var allProps = getAllPropertiesInTypes(immutableTypes);
        var mutableProperties = getMutableProperties(allProps);
        if (mutableProperties.Any())
        {
            Assert.Fail("Found some mutable properties in immutable types");
            return;
        }
        var libProps = getLibraryTypeProperties(allProps);
        var mutableTypeProperties = getMutableTypeProperties(libProps);
        foreach (var property in mutableTypeProperties)
        {
            Assert.Fail("Found some immutable properties which have mutable library types.");
            return;
        }

        IEnumerable<Type> getImmutableTypes(IEnumerable<Type> types)
            => types.Where(ObjectHelper.HasAttribute<ImmutableAttribute>).Build();
        IEnumerable<PropertyInfo> getAllPropertiesInTypes(IEnumerable<Type> types)
            => types.SelectMany(t => t.GetProperties()).Build();
        IEnumerable<PropertyInfo> getMutableProperties(IEnumerable<PropertyInfo> properties)
            => properties.Where(x => (x.SetMethod?.IsPublic ?? false) && !x.IsSetMethodInit()).Build();
        IEnumerable<PropertyInfo> getLibraryTypeProperties(IEnumerable<PropertyInfo> properties)
            => properties.Where(x => x.PropertyType?.Namespace?.StartsWith("Library") ?? false).Build();
        IEnumerable<PropertyInfo> getMutableTypeProperties(IEnumerable<PropertyInfo> properties)
            => properties.Where(x => (x.PropertyType?.IsClass ?? false) && !ObjectHelper.HasAttribute<ImmutableAttribute>(x.PropertyType)).Build();
    }

    private static IEnumerable<Type> GetAllTypes()
        => EnumerableHelper.Merge(
            _libraryTypes.CoreLibTypes,
            _libraryTypes.CqrsLibTypes,
            _libraryTypes.WebLibTypes,
            _libraryTypes.WpfLibTypes
            ).Build();

    private static bool IsInNameSpace(Type type, string ns)
        => type?.Namespace?.StartsWith(ns) is true;
}