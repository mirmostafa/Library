using Library.CodeGeneration;
using Library.DesignPatterns.Markers;
using Library.Helpers.CodeGen;
using Library.Validations;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

using MethodParameterInfo = (Library.CodeGeneration.TypePath Type, string Name);
using RosClass = Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax;
using RosProp = Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax;

namespace TestConApp;

public static class RoslynHelper
{
    public static RosClass AddMethod(this RosClass type, MethodInfo methodInfo)
    {
        var method = CreateMethod(methodInfo);
        var result = type.AddMethod(method);
        return result;
    }
    public static string GetName(this FieldDeclarationSyntax field) =>
        field.ArgumentNotNull().Declaration.Variables.First().Identifier.ValueText;

    public static string GetName(this RosClass type) =>
        type.ArgumentNotNull().Identifier.ValueText;

    public static RosClass AddConstructor(this RosClass type, IEnumerable<MethodParameterInfo>? parameters = null, string? body = null, IEnumerable<SyntaxKind>? modifiers = null)
    {
        var ctor = CreateConstructor(type.GetName(), parameters, body, modifiers);
        type = type.AddMethod(ctor);
        return type;
    }

    public static BaseMethodDeclarationSyntax CreateConstructor(string className, IEnumerable<MethodParameterInfo>? parameters = null, string? body = null, IEnumerable<SyntaxKind>? modifiers = null)
    {
        modifiers ??= EnumerableHelper.ToEnumerable(SyntaxKind.PublicKeyword);
        BaseMethodDeclarationSyntax result = SyntaxFactory.ConstructorDeclaration(className)
            .WithModifiers(modifiers.ToSyntaxTokenList());
        result = InnerCreateBaseMethod(new(modifiers, null, TypePath.GetName(className), parameters, body), result);
        return result;
    }

    public static RosClass AddMethod(this RosClass type, BaseMethodDeclarationSyntax method)
    {
        Check.MustBeArgumentNotNull(type);
        var result = type.AddMembers(method);
        return result;
    }

    public static RosClass AddProperty<TPropertyType>(this RosClass type, string name, bool hasSetAccessor = true, bool hasGetAccessor = true)
    {
        Check.MustBeArgumentNotNull(type);
        var prop = CreateProperty(new(name, typeof(TPropertyType), getAccessor: (hasGetAccessor, null), setAccessor: (hasSetAccessor, null)));
        return type.AddMembers(prop);
    }

    public static RosClass AddProperty(this RosClass type, PropertyInfo propertyInfo)
    {
        Check.MustBeArgumentNotNull(type);
        var prop = CreateProperty(propertyInfo);
        return type.AddMembers(prop);
    }

    public static RosClass AddPropertyWithBackingField<TPropertyType>(this RosClass classDeclarationSyntax, string propertyName) =>
        AddPropertyWithBackingField(classDeclarationSyntax, new PropertyInfo(propertyName, typeof(TPropertyType)));

    public static RosClass AddField(this RosClass type, FieldInfo fieldInfo, out FieldDeclarationSyntax field)
    {
        Check.MustBeArgumentNotNull(type);
        field = CreateField(fieldInfo);
        var result = type.AddMembers(field);
        return result;
    }

    public static RosClass AddPropertyWithBackingField(this RosClass classDeclarationSyntax, PropertyInfo propertyInfo, FieldInfo? fieldInfo = null)
    {
        Check.MustBeArgumentNotNull(classDeclarationSyntax);
        Check.MustBeArgumentNotNull(propertyInfo);

        var (property, field) = CreatePropertyWithBackingField(propertyInfo, fieldInfo ?? new(TypeMemberNameHelper.ToFieldName(propertyInfo.Name), propertyInfo.Type));
        var result = classDeclarationSyntax.AddMembers(field, property);
        return result;
    }

    public static NamespaceDeclarationSyntax AddType(this NamespaceDeclarationSyntax namespaceDeclaration, RosClass type)
    {
        Check.MustBeArgumentNotNull(namespaceDeclaration);

        var result = namespaceDeclaration.AddMembers(type);
        return result;
    }

    public static NamespaceDeclarationSyntax AddType(this NamespaceDeclarationSyntax namespaceDeclaration, string typeName)
    {
        Check.MustBeArgumentNotNull(namespaceDeclaration);
        Check.MustBeArgumentNotNull(typeName);

        var type = CreateType(typeName);
        return namespaceDeclaration.AddType(type);
    }

    public static FieldDeclarationSyntax CreateField(FieldInfo fieldInfo)
    {
        Check.MustBeArgumentNotNull(fieldInfo);

        var result = SyntaxFactory.FieldDeclaration(
            SyntaxFactory.VariableDeclaration(
                SyntaxFactory.ParseTypeName(fieldInfo.Type.FullName),
                SyntaxFactory.SeparatedList(new[] {
                    SyntaxFactory.VariableDeclarator(SyntaxFactory.Identifier(fieldInfo.Name))
        })));
        if (fieldInfo.AccessModifiers?.Any() ?? false)
        {
            result = result.AddModifiers(fieldInfo.AccessModifiers.ToSyntaxTokenArray());
        }

        return result;
    }

    public static BaseMethodDeclarationSyntax InnerCreateBaseMethod(MethodInfo methodInfo, BaseMethodDeclarationSyntax result)
    {
        Check.MustBeArgumentNotNull(methodInfo);

        if (methodInfo.Parameters?.Any() ?? false)
        {
            var paramArray = methodInfo.Parameters.ToArray();
            var nodes = new SyntaxNodeOrToken[(paramArray.Length * 2) - 1];
            var nodeIndex = 0;
            for (var paramIndex = 0; paramIndex < paramArray.Length; paramIndex++)
            {
                var p = paramArray[paramIndex];
                nodes[nodeIndex] = paramIndex == 0 && methodInfo.IsExtensionMethod ?
                    createParam(p).WithModifiers((new[] { SyntaxKind.ThisKeyword }).ToSyntaxTokenList()) :
                    createParam(p);
                if (paramIndex != paramArray.Length - 1)
                {
                    nodes[++nodeIndex] = SyntaxFactory.Token(SyntaxKind.CommaToken);
                }
                nodeIndex++;
            }
            result = result.WithParameterList(SyntaxFactory.ParameterList(SyntaxFactory.SeparatedList<ParameterSyntax>(nodes)));
        }
        if (!methodInfo.Body.IsNullOrEmpty())
        {
            var lines = methodInfo.Body.ReadLines().ToArray();

            result = lines switch
            {
                //{ Length: > 1 } => result.WithBody(SyntaxFactory.Block(lines.Select(x => SyntaxFactory.ParseStatement(x)))),
                //{ Length: 1 } => result.WithExpressionBody(SyntaxFactory.ArrowExpressionClause(SyntaxFactory.ParseExpression(lines[0]))),
                //_ => throw new NotImplementedException()
                _ => result.WithBody(SyntaxFactory.Block(lines.Select(x => SyntaxFactory.ParseStatement(x)))),
            };
        }
        return result;

        static ParameterSyntax createParam(MethodParameterInfo p) =>
            SyntaxFactory.Parameter(SyntaxFactory.Identifier(p.Name)).WithType(SyntaxFactory.ParseTypeName(p.Type.FullName));
    }

    public static BaseMethodDeclarationSyntax CreateMethod(MethodInfo methodInfo)
    {
        Check.MustBeArgumentNotNull(methodInfo);

        var modifiers = methodInfo.Modifiers;
        if (methodInfo.IsExtensionMethod)
        {
            modifiers = modifiers.AddImmuted(SyntaxKind.StaticKeyword);
        }

        BaseMethodDeclarationSyntax result = SyntaxFactory.MethodDeclaration(SyntaxFactory.ParseTypeName(methodInfo.ReturnType?.Name ?? "void"), methodInfo.Name)
            .WithModifiers(modifiers.ToSyntaxTokenList());

        result = InnerCreateBaseMethod(methodInfo, result);
        return result;
    }

    public static NamespaceDeclarationSyntax CreateNamespace(string nameSpaceName)
    {
        var result = SyntaxFactory.NamespaceDeclaration(SyntaxFactory.ParseName(nameSpaceName));
        return result;
    }

    public static RosProp CreateProperty(PropertyInfo propertyInfo)
    {
        Check.MustBeArgumentNotNull(propertyInfo);
        var result = InnerCreatePropertyBase(propertyInfo);

        if (propertyInfo.GetAccessor.Has)
        {
            result = result.AddAccessorListAccessors(SyntaxFactory.AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));
        }
        if (propertyInfo.SetAccessor.Has)
        {
            result = result.AddAccessorListAccessors(SyntaxFactory.AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(SyntaxFactory.Token(SyntaxKind.SemicolonToken)));
        }

        return result;
    }
    public static (RosProp Property, FieldDeclarationSyntax Field) CreatePropertyWithBackingField(PropertyInfo propertyInfo, FieldInfo fieldInfo)
    {
        var field = CreateField(fieldInfo);
        var property = CreatePropertyWithBackingField(propertyInfo, field);
        return (property, field);
    }
    public static RosClass AddPropertyWithBackingField(this RosClass type, PropertyInfo propertyInfo, FieldDeclarationSyntax field)
    {
        var property = CreatePropertyWithBackingField(propertyInfo, field);
        var result = type.AddMembers(property);
        return result;
    }
    public static RosProp CreatePropertyWithBackingField(PropertyInfo propertyInfo, FieldDeclarationSyntax field)
    {
        Check.MustBeArgumentNotNull(propertyInfo);
        Check.MustBeArgumentNotNull(field);
        var result = InnerCreatePropertyBase(propertyInfo);
        if (propertyInfo.GetAccessor.Has || propertyInfo.SetAccessor.Has)
        {
            var accessors = SyntaxFactory.List<AccessorDeclarationSyntax>();
            if (propertyInfo.GetAccessor.Has)
            {
                accessors = accessors.Add(SyntaxFactory.AccessorDeclaration(
                        SyntaxKind.GetAccessorDeclaration,
                        SyntaxFactory.Block(
                            SyntaxFactory.SingletonList<StatementSyntax>(
                                SyntaxFactory.ReturnStatement(SyntaxFactory.IdentifierName(field.GetName()))
                            )
                        )
                    ));
            }
            if (propertyInfo.SetAccessor.Has)
            {
                accessors = accessors.Add(SyntaxFactory.AccessorDeclaration(
                        SyntaxKind.SetAccessorDeclaration,
                        SyntaxFactory.Block(
                            SyntaxFactory.SingletonList<StatementSyntax>(
                                SyntaxFactory.ExpressionStatement(
                                    SyntaxFactory.AssignmentExpression(
                                        SyntaxKind.SimpleAssignmentExpression,
                                        SyntaxFactory.IdentifierName(field.GetName()),
                                        SyntaxFactory.IdentifierName("value")
                                    )
                                )
                            )
                        )
                    ));
            }
            result = result.WithAccessorList(SyntaxFactory.AccessorList(accessors));
        }
        return result;
    }

    public static RosClass CreateType(string typeName, IEnumerable<SyntaxKind>? modifiers = null)
    {
        modifiers ??= new[] { SyntaxKind.PublicKeyword, SyntaxKind.SealedKeyword };
        var result = SyntaxFactory.ClassDeclaration(typeName)
            .WithModifiers(modifiers.ToSyntaxTokenList());
        return result;
    }

    public static string GenerateCode(this SyntaxNode syntaxNode)
    {
        var result = syntaxNode.NormalizeWhitespace().ToFullString();
        return result;
    }

    private static RosProp InnerCreatePropertyBase(PropertyInfo propertyInfo)
    {
        var result = SyntaxFactory.PropertyDeclaration(SyntaxFactory.ParseTypeName(propertyInfo.Type.Name), propertyInfo.Name);
        if (propertyInfo.Modifiers?.Any() ?? false)
        {
            result = result.AddModifiers(propertyInfo.Modifiers.ToSyntaxTokenArray());
        }

        return result;
    }
}

[Immutable]
public sealed class FieldInfo(
    string name,
    TypePath type,
    IEnumerable<SyntaxKind>? accessModifiers = null) : IEquatable<FieldInfo>
{
    public IEnumerable<SyntaxKind>? AccessModifiers { get; } = accessModifiers ?? EnumerableHelper.ToEnumerable(SyntaxKind.PrivateKeyword);
    public string Name { get; } = name;
    public TypePath Type { get; } = type;

    public static bool operator !=(FieldInfo left, FieldInfo right) =>
        !(left == right);

    public static bool operator ==(FieldInfo left, FieldInfo right) =>
        left?.Equals(right) ?? right is null;

    public override bool Equals(object? obj) =>
        obj is FieldInfo other && this.Equals(other);

    public bool Equals(FieldInfo? other) =>
        other is { } o && o.Name == this.Name && o.Type == this.Type;

    public override int GetHashCode() =>
        HashCode.Combine(this.Type, this.Name.GetHashCode(StringComparison.CurrentCulture));
}

[Immutable]
public sealed class MethodInfo(
    IEnumerable<SyntaxKind>? modifiers,
    TypePath? returnType,
    string name,
    IEnumerable<MethodParameterInfo>? parameters, string? body = null, bool isExtensionMethod = false) : IEquatable<MethodInfo>
{
    public string? Body { get; } = body ?? "throw new NotImplementedException();";
    public bool IsExtensionMethod { get; } = isExtensionMethod;
    public IEnumerable<SyntaxKind> Modifiers { get; } = modifiers ?? EnumerableHelper.ToEnumerable(SyntaxKind.PublicKeyword);
    public string Name { get; } = name;
    public IEnumerable<MethodParameterInfo>? Parameters { get; } = parameters;
    public TypePath? ReturnType { get; } = returnType;

    public static bool operator !=(MethodInfo left, MethodInfo right) =>
        !(left == right);

    public static bool operator ==(MethodInfo left, MethodInfo right) =>
        left?.Equals(right) ?? right is null;

    public override bool Equals(object? obj) =>
        obj is MethodInfo other && this.Equals(other);

    public bool Equals(MethodInfo? other) =>
        other is { } o && o.Name == this.Name && o.Parameters == this.Parameters;

    public override int GetHashCode() =>
        HashCode.Combine(this.Name, this.Parameters?.GetHashCode(), this.Name.GetHashCode(StringComparison.CurrentCulture));
}

[Immutable]
public sealed class PropertyInfo(
    in string name,
    in TypePath type,
    in IEnumerable<SyntaxKind>? modifiers = null,
    in (bool Has, IEnumerable<SyntaxKind>? AccessModifiers)? getAccessor = null,
    in (bool Has, IEnumerable<SyntaxKind>? AccessModifiers)? setAccessor = null) : IEquatable<PropertyInfo>
{
    public (bool Has, IEnumerable<SyntaxKind>? AccessModifiers) GetAccessor { get; } = getAccessor == null ? (true, EnumerableHelper.ToEnumerable(SyntaxKind.PublicKeyword)) : getAccessor.Value;
    public IEnumerable<SyntaxKind> Modifiers { get; } = modifiers ?? EnumerableHelper.ToEnumerable(SyntaxKind.PublicKeyword);
    public string Name { get; } = name;
    public (bool Has, IEnumerable<SyntaxKind>? AccessModifiers) SetAccessor { get; } = setAccessor == null ? (true, EnumerableHelper.ToEnumerable(SyntaxKind.PublicKeyword)) : setAccessor.Value;
    public TypePath Type { get; } = type;

    public static bool operator !=(PropertyInfo left, PropertyInfo right) =>
        !(left == right);

    public static bool operator ==(PropertyInfo left, PropertyInfo right) =>
        left?.Equals(right) ?? right is null;

    public override bool Equals(object? obj) =>
        obj is PropertyInfo other && this.Equals(other);

    public bool Equals(PropertyInfo? other) =>
        other is { } o && o.Name == this.Name && o.Type == this.Type;

    public override int GetHashCode() =>
        HashCode.Combine(this.Type, this.Name.GetHashCode(StringComparison.CurrentCulture));
}

internal static partial class Helpers
{
    public static SyntaxToken[] ToSyntaxTokenArray(this IEnumerable<SyntaxKind>? syntaxKinds)
    {
        return iterate(syntaxKinds).ToArray();

        static IEnumerable<SyntaxToken> iterate(IEnumerable<SyntaxKind>? syntaxKinds)
        {
            if (!(syntaxKinds?.Any() ?? false))
            {
                yield break;
            }
            foreach (var kind in syntaxKinds)
            {
                if (kind is { } k)
                {
                    yield return SyntaxFactory.Token(k);
                }
            }
        }
    }

    public static SyntaxTokenList ToSyntaxTokenList(this IEnumerable<SyntaxKind>? syntaxKinds)
    {
        var tokenList = new SyntaxTokenList();

        foreach (var kind in syntaxKinds ?? Enumerable.Empty<SyntaxKind>())
        {
            var token = SyntaxFactory.Token(kind);
            tokenList = tokenList.Add(token);
        }

        return tokenList;
    }
}