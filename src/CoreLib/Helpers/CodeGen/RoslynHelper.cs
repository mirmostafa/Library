using Library.CodeGeneration;
using Library.DesignPatterns.Markers;
using Library.Helpers.Models;
using Library.Results;
using Library.Validations;

using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

using static Microsoft.CodeAnalysis.CSharp.SyntaxFactory;

using MethodParameterInfo = (Library.CodeGeneration.TypePath Type, string Name);
using PropertyAccessorInfo = (bool Has, System.Collections.Generic.IEnumerable<Microsoft.CodeAnalysis.CSharp.SyntaxKind>? AccessModifiers);
using RosClass = Microsoft.CodeAnalysis.CSharp.Syntax.ClassDeclarationSyntax;
using RosFld = Microsoft.CodeAnalysis.CSharp.Syntax.FieldDeclarationSyntax;
using RosMember = Microsoft.CodeAnalysis.CSharp.Syntax.MemberDeclarationSyntax;
using RosMethod = Microsoft.CodeAnalysis.CSharp.Syntax.BaseMethodDeclarationSyntax;
using RosProp = Microsoft.CodeAnalysis.CSharp.Syntax.PropertyDeclarationSyntax;

namespace Library.Helpers.CodeGen;

public static class RoslynHelper
{
    public static RosClass AddAttribute(this RosClass member, string attributeName, IEnumerable<(string? propName, string propValue)>? properties = null)
    {
        Check.MustBeArgumentNotNull(member);
        Check.MustBeArgumentNotNull(attributeName);

        var attributeArgumentList = AttributeArgumentList();
        if (properties != null)
        {
            foreach (var (propName, propValue) in properties)
            {
                var nameEquals = propName == null ? default : NameEquals(propName);
                var stringLiteral = LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(propValue));
                var attributeArgument = AttributeArgument(nameEquals, null, stringLiteral);
                attributeArgumentList = attributeArgumentList.AddArguments(attributeArgument);
            }
        }

        var attributeList = AttributeList(SingletonSeparatedList(Attribute(IdentifierName(attributeName), attributeArgumentList)));

        return member.WithAttributeLists(member.AttributeLists.Add(attributeList));
    }
    public static RosMember AddAttribute(this RosMember member, string attributeName, IEnumerable<(string? propName, string propValue)>? properties = null)
    {
        Check.MustBeArgumentNotNull(member);
        Check.MustBeArgumentNotNull(attributeName);

        var attributeArgumentList = AttributeArgumentList();
        if (properties != null)
        {
            foreach (var (propName, propValue) in properties)
            {
                var nameEquals = propName == null ? default : NameEquals(propName);
                var stringLiteral = LiteralExpression(SyntaxKind.StringLiteralExpression, Literal(propValue));
                var attributeArgument = AttributeArgument(nameEquals, null, stringLiteral);
                attributeArgumentList = attributeArgumentList.AddArguments(attributeArgument);
            }
        }

        var attributeList = AttributeList(SingletonSeparatedList(Attribute(IdentifierName(attributeName), attributeArgumentList)));

        return member.WithAttributeLists(member.AttributeLists.Add(attributeList));
    }

    public static RosClass AddBase(this RosClass type, string baseClassName)
    {
        Checker.MustBeArgumentNotNull(type);

        return type.WithBaseList(BaseList(new SeparatedSyntaxList<BaseTypeSyntax>().Add(SimpleBaseType(ParseTypeName(baseClassName)))));
    }

    public static RosClass AddConstructor(this RosClass type, IEnumerable<MethodParameterInfo>? parameters = null, string? body = null, IEnumerable<SyntaxKind>? modifiers = null) =>
        type.AddConstructor(out _, parameters, body);

    public static RosClass AddConstructor(this RosClass type, out RosMethod ctor, IEnumerable<MethodParameterInfo>? parameters = null, string? body = null, IEnumerable<SyntaxKind>? modifiers = null)
    {
        Checker.MustBeArgumentNotNull(type);

        ctor = CreateConstructor(type.GetName(), modifiers, parameters, body);
        return type.AddMethod(ctor);
    }

    public static RosClass AddField(this RosClass type, RosFieldInfo fieldInfo) =>
        type.AddField(fieldInfo, out _);

    public static RosClass AddField(this RosClass type, RosFieldInfo fieldInfo, out RosFld field)
    {
        Checker.MustBeArgumentNotNull(type);

        field = CreateField(fieldInfo);
        return type.AddMembers(field);
    }

    public static RosClass AddMethod(this RosClass type, RosMethodInfo methodInfo, out RosMethod method)
    {
        Checker.MustBeArgumentNotNull(type);

        method = CreateMethod(methodInfo);
        return type.AddMethod(method);
    }

    public static RosClass AddMethod(this RosClass type, RosMethodInfo methodInfo) =>
        type.AddMethod(methodInfo, out _);

    public static RosClass AddMethod(this RosClass type, RosMethod method)
    {
        Checker.MustBeArgumentNotNull(type);

        return type.AddMembers(method);
    }

    public static CompilationUnitSyntax AddNameSpace(this CompilationUnitSyntax root, BaseNamespaceDeclarationSyntax nameSpace) =>
        root.ArgumentNotNull().WithMembers(SingletonList<MemberDeclarationSyntax>(nameSpace));

    public static CompilationUnitSyntax AddNameSpace(this CompilationUnitSyntax root, string nameSpaceName, out BaseNamespaceDeclarationSyntax nameSpace)
    {
        nameSpace = CreateNamespace(nameSpaceName);
        return root.ArgumentNotNull().WithMembers(SingletonList<MemberDeclarationSyntax>(nameSpace));
    }

    public static RosClass AddProperty<TPropertyType>(this RosClass type, string name, bool hasSetAccessor = true, bool hasGetAccessor = true) =>
                type.AddProperty<TPropertyType>(name, out _, hasSetAccessor, hasGetAccessor);

    public static RosClass AddProperty(this RosClass type, string name, TypePath typePath, bool hasSetAccessor = true, bool hasGetAccessor = true) =>
        type.AddProperty(name, typePath, out _, hasSetAccessor, hasGetAccessor);

    public static RosClass AddProperty(this RosClass type, string name, TypePath typePath, out RosProp prop, bool hasSetAccessor = true, bool hasGetAccessor = true)
    {
        Checker.MustBeArgumentNotNull(type);

        prop = CreateProperty(new(name, typePath, getAccessor: (hasGetAccessor, null), setAccessor: (hasSetAccessor, null)));
        return type.AddMembers(prop);
    }

    public static RosClass AddProperty<TPropertyType>(this RosClass type, string name, out RosProp prop, bool hasSetAccessor = true, bool hasGetAccessor = true) =>
        type.AddProperty(name, typeof(TPropertyType), out prop, hasSetAccessor, hasGetAccessor);

    public static RosClass AddProperty(this RosClass type, [DisallowNull] RosPropertyInfo propertyInfo) =>
        type.AddProperty(propertyInfo, out _);

    public static RosClass AddProperty([DisallowNull] this RosClass type, [DisallowNull] RosPropertyInfo propertyInfo, [DisallowNull] out RosProp property)
    {
        Checker.MustBeArgumentNotNull(type);

        property = CreateProperty(propertyInfo);
        return type.AddMembers(property);
    }

    public static RosClass AddPropertyWithBackingField<TPropertyType>(this RosClass type, string propertyName) =>
        type.AddPropertyWithBackingField(new RosPropertyInfo(propertyName, typeof(TPropertyType)));

    public static RosClass AddPropertyWithBackingField([DisallowNull] this RosClass type, [DisallowNull] RosPropertyInfo propertyInfo, RosFieldInfo? fieldInfo = null) =>
        type.AddPropertyWithBackingField(propertyInfo, out _, fieldInfo);

    public static RosClass AddPropertyWithBackingField([DisallowNull] this RosClass type, [DisallowNull] RosPropertyInfo propertyInfo, out (RosProp Property, RosFld Field) fullProperty, RosFieldInfo? fieldInfo = null)
    {
        Checker.MustBeArgumentNotNull(type);
        Checker.MustBeArgumentNotNull(propertyInfo);

        fieldInfo ??= new(TypeMemberNameHelper.ToFieldName(propertyInfo.Name), propertyInfo.Type);
        fullProperty = CreatePropertyWithBackingField(propertyInfo, fieldInfo);
        return type.AddMembers(fullProperty.Field, fullProperty.Property);
    }

    public static RosClass AddPropertyWithBackingField([DisallowNull] this RosClass type, [DisallowNull] RosPropertyInfo propertyInfo, [DisallowNull] RosFld field) =>
        type.AddPropertyWithBackingField(propertyInfo, field, out _);

    public static RosClass AddPropertyWithBackingField([DisallowNull] this RosClass type, [DisallowNull] RosPropertyInfo propertyInfo, [DisallowNull] RosFld field, out RosProp property)
    {
        Checker.MustBeArgumentNotNull(type);

        property = CreatePropertyWithBackingField(propertyInfo, field);
        return type.AddMembers(property);
    }

    public static BaseNamespaceDeclarationSyntax AddType(this BaseNamespaceDeclarationSyntax nameSpace, RosClass type)
    {
        Checker.MustBeArgumentNotNull(nameSpace);

        return nameSpace.AddMembers(type);
    }

    public static BaseNamespaceDeclarationSyntax AddType(this BaseNamespaceDeclarationSyntax nameSpace, string typeName) =>
        nameSpace.AddType(typeName, out _);

    public static BaseNamespaceDeclarationSyntax AddType(this BaseNamespaceDeclarationSyntax nameSpace, string typeName, out RosClass type, IEnumerable<SyntaxKind>? modifiers = null)
    {
        Checker.MustBeArgumentNotNull(nameSpace);

        type = CreateType(typeName, modifiers);
        return nameSpace.AddType(type);
    }

    public static CompilationUnitSyntax AddUsingNameSpace(this CompilationUnitSyntax root, string usingNamespace) =>
        root.ArgumentNotNull().AddUsings(CreateUsingNameSpace(usingNamespace));

    public static BaseNamespaceDeclarationSyntax AddUsingNameSpace(this BaseNamespaceDeclarationSyntax nameSpace, string usingNamespace)
    {
        Checker.MustBeArgumentNotNull(nameSpace);

        var usingDirective = UsingDirective(ParseName(usingNamespace));
        return nameSpace.AddUsings(usingDirective);
    }

    public static Result CompileOnFly(CompileOnFlyOptionsBase options)
    {
        var source = options switch
        {
            CompileOnFlyByFileOptions file => File.ReadAllText(file.SourceFile),
            CompileOnFlyBySourceOptions src => src.Source,
            _ => throw new NotImplementedException(),
        };
        var parsedSyntaxTree = ParseSyntaxTree(SourceText.From(source, Encoding.UTF8), CSharpParseOptions.Default.WithLanguageVersion(LanguageVersion.Latest));
        _ = options.FrameworkReferences.Add("mscorlib");
        _ = options.FrameworkReferences.Add("System");
        _ = options.FrameworkReferences.Add("System.Core.dll");
        var defaultReferences = options.FrameworkReferences.Select(x => MetadataReference.CreateFromFile(x));
        var defaultCompilationOptions = new CSharpCompilationOptions(OutputKind.DynamicallyLinkedLibrary)
            .WithOverflowChecks(options.WithOverflowChecks)
            .WithOptimizationLevel(options.IsReleaseMode ? OptimizationLevel.Release : OptimizationLevel.Debug);
        var compilation = CSharpCompilation.Create($"{Guid.NewGuid}.dll", [parsedSyntaxTree], defaultReferences, defaultCompilationOptions);
        try
        {
            var result = string.IsNullOrEmpty(options.OutputFile) ? compilation.Emit(Stream.Null) : compilation.Emit(options.OutputFile);
            return result.Success
                ? Result.Success
                : Result.CreateFailure(errors: result.Diagnostics.Select(x => ((object)x.Id, (object)x.GetMessage())));
        }
        catch (Exception ex)
        {
            return Result.CreateFailure(ex);
        }
    }

    public static Result CompileOnFlyByFile(string inputFile, string? outputFile = null) =>
        CompileOnFly(new CompileOnFlyByFileOptions(inputFile, outputFile));

    public static Result CompileOnFlyBySource(string source, string? outputFile = null) =>
        CompileOnFly(new CompileOnFlyBySourceOptions(source, outputFile));

    public static RosMethod CreateConstructor(string className, IEnumerable<SyntaxKind>? modifiers = null, IEnumerable<MethodParameterInfo>? parameters = null, string? body = null)
    {
        modifiers ??= EnumerableHelper.AsEnumerable(SyntaxKind.PublicKeyword);
        var ctor = ConstructorDeclaration(className).WithModifiers(modifiers.ToSyntaxTokenList());
        return InnerCreateBaseMethod(new(modifiers, null, TypePath.GetName(className), parameters, body), ctor);
    }

    public static RosFld CreateField(RosFieldInfo fieldInfo)
    {
        Checker.MustBeArgumentNotNull(fieldInfo);

        var result = FieldDeclaration(
            VariableDeclaration(
                ParseTypeName(fieldInfo.Type.FullName),
                SeparatedList(new[] {
                    VariableDeclarator(Identifier(fieldInfo.Name))
        })));
        if (fieldInfo.AccessModifiers?.Any() ?? false)
        {
            result = result.AddModifiers(fieldInfo.AccessModifiers.ToSyntaxTokenArray());
        }

        return result;
    }

    public static RosMethod CreateMethod(RosMethodInfo methodInfo)
    {
        Checker.MustBeArgumentNotNull(methodInfo);

        var modifiers = methodInfo.Modifiers;
        if (methodInfo.IsExtensionMethod)
        {
            modifiers = modifiers.AddImmuted(SyntaxKind.StaticKeyword);
        }

        RosMethod result = MethodDeclaration(ParseTypeName(methodInfo.ReturnType?.FullName ?? "void"), methodInfo.Name).WithModifiers(modifiers.ToSyntaxTokenList());

        result = InnerCreateBaseMethod(methodInfo, result);
        return result;
    }

    public static BaseNamespaceDeclarationSyntax CreateNamespace(string nameSpaceName)
    {
        var result = FileScopedNamespaceDeclaration(ParseName(nameSpaceName));
        return result;
    }

    public static RosProp CreateProperty(RosPropertyInfo propertyInfo)
    {
        Checker.MustBeArgumentNotNull(propertyInfo);
        var result = InnerCreatePropertyBase(propertyInfo);

        if (propertyInfo.GetAccessor.Has || propertyInfo.SetAccessor.Has)
        {
            if (propertyInfo.GetAccessor.Has)
            {
                result = result.AddAccessorListAccessors(AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)));
            }
            if (propertyInfo.SetAccessor.Has)
            {
                result = result.AddAccessorListAccessors(AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)));
            }
        }
        else
        {
            result = result.AddAccessorListAccessors(AccessorDeclaration(SyntaxKind.GetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)));
            result = result.AddAccessorListAccessors(AccessorDeclaration(SyntaxKind.SetAccessorDeclaration).WithSemicolonToken(Token(SyntaxKind.SemicolonToken)));
        }

        return result;
    }

    public static (RosProp Property, RosFld Field) CreatePropertyWithBackingField(RosPropertyInfo propertyInfo, RosFieldInfo fieldInfo)
    {
        var field = CreateField(fieldInfo);
        var property = CreatePropertyWithBackingField(propertyInfo, field);
        return (property, field);
    }

    public static RosProp CreatePropertyWithBackingField([DisallowNull] RosPropertyInfo propertyInfo, [DisallowNull] RosFld field)
    {
        Checker.MustBeArgumentNotNull(propertyInfo);
        Checker.MustBeArgumentNotNull(field);
        var result = InnerCreatePropertyBase(propertyInfo);
        if (propertyInfo.GetAccessor.Has || propertyInfo.SetAccessor.Has)
        {
            var accessors = List<AccessorDeclarationSyntax>();
            if (propertyInfo.GetAccessor.Has)
            {
                accessors = accessors.Add(AccessorDeclaration(
                        SyntaxKind.GetAccessorDeclaration,
                        Block(
                            SingletonList<StatementSyntax>(
                                ReturnStatement(IdentifierName(field.GetName()))
                            )
                        )
                    ));
            }
            if (propertyInfo.SetAccessor.Has)
            {
                accessors = accessors.Add(AccessorDeclaration(
                        SyntaxKind.SetAccessorDeclaration,
                        Block(
                            SingletonList<StatementSyntax>(
                                ExpressionStatement(
                                    AssignmentExpression(
                                        SyntaxKind.SimpleAssignmentExpression,
                                        IdentifierName(field.GetName()),
                                        IdentifierName("value")
                                    )
                                )
                            )
                        )
                    ));
            }
            result = result.WithAccessorList(AccessorList(accessors));
        }
        return result;
    }

    public static CompilationUnitSyntax CreateRoot() =>
        CompilationUnit();

    public static RosClass CreateType(TypePath typeName, IEnumerable<SyntaxKind>? modifiers = null)
    {
        Checker.MustBeArgumentNotNull(typeName?.Name);

        modifiers ??= new[] { SyntaxKind.PublicKeyword, SyntaxKind.SealedKeyword };
        return ClassDeclaration(typeName.Name).WithModifiers(modifiers.ToSyntaxTokenList());
    }

    public static UsingDirectiveSyntax CreateUsingNameSpace(string usingNameSpace) =>
        UsingDirective(ParseName(usingNameSpace));

    public static string GenerateCode(this SyntaxNode syntaxNode) =>
        syntaxNode.NormalizeWhitespace().ToFullString();

    public static string GetName(this RosFld field) =>
        field.ArgumentNotNull().Declaration.Variables.First().Identifier.ValueText;

    public static string GetName(this RosClass type) =>
        type.ArgumentNotNull().Identifier.ValueText;

    public static string ReformatCode(string sourceCode) =>
        CSharpSyntaxTree.ParseText(sourceCode)
            .GetRoot()
            .NormalizeWhitespace()
            .SyntaxTree
            .GetText()
            .ToString();

    private static RosMethod InnerCreateBaseMethod(RosMethodInfo methodInfo, RosMethod result)
    {
        Checker.MustBeArgumentNotNull(methodInfo);
        Checker.MustBeArgumentNotNull(result);

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
                    nodes[++nodeIndex] = Token(SyntaxKind.CommaToken);
                }
                nodeIndex++;
            }
            result = result.WithParameterList(ParameterList(SeparatedList<ParameterSyntax>(nodes)));
        }
        if (!methodInfo.Body.IsNullOrEmpty())
        {
            var lines = methodInfo.Body.ReadLines().ToArray();

            result = lines switch
            {
                //{ Length: > 1 } => result.WithBody(Block(lines.Select(x => ParseStatement(x)))),
                //{ Length: 1 } => result.WithExpressionBody(ArrowExpressionClause(ParseExpression(lines[0]))),
                //_ => throw new NotImplementedException()
                _ => result.WithBody(Block(lines.Select(x => ParseStatement(x)))),
            };
        }
        return result;

        static ParameterSyntax createParam(MethodParameterInfo p) =>
            Parameter(Identifier(p.Name)).WithType(ParseTypeName(p.Type.FullName));
    }

    private static RosProp InnerCreatePropertyBase(RosPropertyInfo propertyInfo)
    {
        var result = PropertyDeclaration(ParseTypeName(propertyInfo.Type.FullName), propertyInfo.Name);
        if (propertyInfo.Modifiers?.Any() ?? false)
        {
            result = result.AddModifiers(propertyInfo.Modifiers.ToSyntaxTokenArray());
        }

        return result;
    }
}

[Immutable]
public sealed class RosFieldInfo(
    in string name,
    in TypePath type,
    in IEnumerable<SyntaxKind>? accessModifiers = null) : IEquatable<RosFieldInfo>
{
    public IEnumerable<SyntaxKind>? AccessModifiers { get; } = accessModifiers ?? EnumerableHelper.AsEnumerable(SyntaxKind.PrivateKeyword);
    public string Name { get; } = name;
    public TypePath Type { get; } = type;

    public static bool operator !=(RosFieldInfo left, RosFieldInfo right) =>
        !(left == right);

    public static bool operator ==(RosFieldInfo left, RosFieldInfo right) =>
        left?.Equals(right) ?? right is null;

    public override bool Equals(object? obj) =>
        obj is RosFieldInfo other && this.Equals(other);

    public bool Equals(RosFieldInfo? other) =>
        other is { } o && o.Name == this.Name && o.Type == this.Type;

    public override int GetHashCode() =>
        HashCode.Combine(this.Type, this.Name.GetHashCode(StringComparison.CurrentCulture));
}

[Immutable]
public sealed class RosMethodInfo(
    in IEnumerable<SyntaxKind>? modifiers,
    in TypePath? returnType,
    in string name,
    in IEnumerable<MethodParameterInfo>? parameters,
    in string? body = null,
    in bool isExtensionMethod = false) : IEquatable<RosMethodInfo>
{
    public string? Body { get; } = body ?? "throw new NotImplementedException();";
    public bool IsExtensionMethod { get; } = isExtensionMethod;
    public IEnumerable<SyntaxKind> Modifiers { get; } = modifiers ?? EnumerableHelper.AsEnumerable(SyntaxKind.PublicKeyword);
    public string Name { get; } = name;
    public IEnumerable<MethodParameterInfo>? Parameters { get; } = parameters;
    public TypePath? ReturnType { get; } = returnType;

    public static bool operator !=(RosMethodInfo left, RosMethodInfo right) =>
        !(left == right);

    public static bool operator ==(RosMethodInfo left, RosMethodInfo right) =>
        left?.Equals(right) ?? right is null;

    public override bool Equals(object? obj) =>
        obj is RosMethodInfo other && this.Equals(other);

    public bool Equals(RosMethodInfo? other) =>
        other is { } o && o.Name == this.Name && o.Parameters == this.Parameters;

    public override int GetHashCode() =>
        HashCode.Combine(this.Name, this.Parameters?.GetHashCode(), this.Name.GetHashCode(StringComparison.CurrentCulture));
}

[Immutable]
public sealed class RosPropertyInfo(
    in string name,
    in TypePath type,
    in IEnumerable<SyntaxKind>? modifiers = null,
    in PropertyAccessorInfo? getAccessor = null,
    in PropertyAccessorInfo? setAccessor = null) : IEquatable<RosPropertyInfo>
{
    public PropertyAccessorInfo GetAccessor { get; } = getAccessor == null ? (true, EnumerableHelper.AsEnumerable(SyntaxKind.PublicKeyword)) : getAccessor.Value;
    public IEnumerable<SyntaxKind> Modifiers { get; } = modifiers ?? EnumerableHelper.AsEnumerable(SyntaxKind.PublicKeyword);
    public string Name { get; } = name;
    public PropertyAccessorInfo SetAccessor { get; } = setAccessor == null ? (true, EnumerableHelper.AsEnumerable(SyntaxKind.PublicKeyword)) : setAccessor.Value;
    public TypePath Type { get; } = type;

    public static bool operator !=(RosPropertyInfo left, RosPropertyInfo right) =>
        !(left == right);

    public static bool operator ==(RosPropertyInfo left, RosPropertyInfo right) =>
        left?.Equals(right) ?? right is null;

    public override bool Equals(object? obj) =>
        obj is RosPropertyInfo other && this.Equals(other);

    public bool Equals(RosPropertyInfo? other) =>
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
                    yield return Token(k);
                }
            }
        }
    }

    public static SyntaxTokenList ToSyntaxTokenList(this IEnumerable<SyntaxKind>? syntaxKinds)
    {
        var tokenList = new SyntaxTokenList();

        foreach (var kind in syntaxKinds ?? Enumerable.Empty<SyntaxKind>())
        {
            var token = Token(kind);
            tokenList = tokenList.Add(token);
        }

        return tokenList;
    }
}