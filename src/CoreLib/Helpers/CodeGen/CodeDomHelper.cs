using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;

using Library.CodeGeneration;
using Library.CodeGeneration.Models;
using Library.DesignPatterns.Markers;
using Library.Validations;

using Microsoft.CSharp;

using static Library.Helpers.CodeGen.TypeMemberNameHelper;

namespace Library.Helpers.CodeGen;

[Fluent]
public static class CodeDomHelper
{
    public static CodeTypeDeclaration AddAttribute(this CodeTypeDeclaration type, string attributeName, (string Key, string Value) prop)
    {
        var securityAttribute = new CodeAttributeDeclaration(attributeName, new CodeAttributeArgument(prop.Key, new CodePrimitiveExpression(prop.Value)));
        _ = type.CustomAttributes.Add(securityAttribute);
        return type;
    }

    /// <summary>
    /// Adds a constructor to the given <see cref="CodeTypeDeclaration"/>.
    /// </summary>
    /// <param name="c">The <see cref="CodeTypeDeclaration"/> to add the constructor to.</param>
    /// <param name="arguments">The arguments of the constructor.</param>
    /// <param name="body">The body of the constructor.</param>
    /// <param name="accessModifiers">The access modifiers of the constructor.</param>
    /// <param name="comment">The comment of the constructor.</param>
    /// <returns>The <see cref="CodeTypeDeclaration"/> with the added constructor.</returns>
    public static CodeTypeDeclaration AddConstructor(this CodeTypeDeclaration c,
            in IEnumerable<(string Type, string Name, string DataMemberName)> arguments,
            in string? body = null,
            in MemberAttributes? accessModifiers = null,
            in string? comment = null)
    {
        Check.MustBeArgumentNotNull(arguments);
        Check.MustBeArgumentNotNull(c);
        var constructor = new CodeConstructor
        {
            Attributes = accessModifiers ?? MemberAttributes.Public | MemberAttributes.Final
        };
        if (comment is not null)
        {
            _ = constructor.AddSummary(comment);
        }

        foreach ((var argType, var argName, _) in arguments)
        {
            _ = constructor.Parameters.Add(new(argType, ToArgName(argName)));
            var widthValueReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), ToPropName(argName));
            _ = constructor.Statements.Add(new CodeAssignStatement(widthValueReference, new CodeArgumentReferenceExpression(ToArgName(argName))));
        }

        if (body is not null)
        {
            _ = constructor.Statements.Add(new CodeSnippetStatement(body));
        }

        _ = c.Members.Add(constructor);
        return c;
    }

    /// <summary>
    /// Adds a constructor to the given <see cref="CodeTypeDeclaration"/>.
    /// </summary>
    /// <param name="c">The <see cref="CodeTypeDeclaration"/> to add the constructor to.</param>
    /// <param name="arguments">The arguments of the constructor.</param>
    /// <param name="body">The body of the constructor.</param>
    /// <param name="accessModifiers">The access modifiers of the constructor.</param>
    /// <param name="comment">The comment of the constructor.</param>
    /// <returns>The <see cref="CodeTypeDeclaration"/> with the added constructor.</returns>
    public static CodeTypeDeclaration AddConstructor(this CodeTypeDeclaration c,
            in IEnumerable<(string Type, string Name, string DataMemberName, bool IsPropery)> arguments,
            in string? body = null,
            in MemberAttributes? accessModifiers = null,
            in string? comment = null)
    {
        Check.MustBeArgumentNotNull(c);
        Check.MustBeArgumentNotNull(arguments);
        var constructor = new CodeConstructor
        {
            Attributes = accessModifiers ?? MemberAttributes.Public | MemberAttributes.Final
        };
        if (comment is not null)
        {
            _ = constructor.AddSummary(comment);
        }

        foreach ((var argType, var argName, _, var isProperty) in arguments)
        {
            _ = constructor.Parameters.Add(new(argType, ToArgName(argName)));
            var widthValueReference = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), isProperty ? ToPropName(argName) : ToFieldName(argName));
            _ = constructor.Statements.Add(new CodeAssignStatement(widthValueReference, new CodeArgumentReferenceExpression(ToArgName(argName))));
        }

        if (body is not null)
        {
            _ = constructor.Statements.Add(new CodeSnippetStatement(body));
        }

        _ = c.Members.Add(constructor);
        return c;
    }

    /// <summary>
    /// Adds new field to specific CodeTypeDeclaration.
    /// </summary>
    /// <param name="c">The CodeTypeDeclaration.</param>
    /// <param name="type">The type.</param>
    /// <param name="name">The name.</param>
    /// <param name="comment">The comment.</param>
    /// <param name="accessModifier">The access modifier.</param>
    /// <returns></returns>
    public static CodeTypeDeclaration AddField(this CodeTypeDeclaration c,
        in string type,
        in string name,
        in string? comment = null,
        in MemberAttributes? accessModifier = null,
        in bool isReadOnly = false,
        in bool isPartial = false)
    {
        Check.MustBeArgumentNotNull(c);

        _ = c.Members.Add(NewField(type, name, comment, accessModifier, isPartial));
        return c;
    }

    /// <summary>
    /// Adds a method.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="name">The name.</param>
    /// <param name="body">The body.</param>
    /// <param name="returnType">Type of the return.</param>
    /// <param name="accessModifiers">The access modifiers.</param>
    /// <param name="isPartial">if set to <c>true</c> [is partial].</param>
    /// <param name="arguments">The arguments.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">nameof(c)</exception>
    public static CodeTypeDeclaration AddMethod(this CodeTypeDeclaration c,
        in string name,
        in string? body = null,
        in string? returnType = null,
        in MemberAttributes? accessModifiers = null,
        bool isPartial = false, params (string Type, string Name)[] arguments)
    {
        Check.MustBeArgumentNotNull(c);
        _ = c.Members.Add(NewMethod(name, body, returnType, accessModifiers ?? MemberAttributes.Public | MemberAttributes.Final, isPartial, arguments));
        return c;
    }

    /// <summary>
    ///     Creates new namespace.
    /// </summary>
    /// <param name="unit">The CodeCompileUnit unit.</param>
    /// <param name="nameSpace">The name space.</param>
    /// <returns></returns>
    public static CodeNamespace AddNewNameSpace(this CodeCompileUnit unit, in string? nameSpace = null)
    {
        Check.MustBeArgumentNotNull(unit);

        CodeNamespace? result = nameSpace is null ? new() : new(nameSpace);
        _ = unit.Namespaces.Add(result);
        return result;
    }

    /// <summary>
    ///     Adds a new class.
    /// </summary>
    /// <param name="ns">The namespace.</param>
    /// <param name="className">Name of the class.</param>
    /// <param name="baseTypes">The base types.</param>
    /// <param name="isPartial">if set to <c>true</c> [is partial].</param>
    /// <returns></returns>
    public static CodeTypeDeclaration AddNewType(
        this CodeNamespace ns,
        in string className,
        in IEnumerable<string>? baseTypes = null,
        bool isClass = true,
        bool isPartial = false,
        bool isStatic = false,
        TypeAttributes typeAttributes = TypeAttributes.Public | TypeAttributes.Sealed)
    {
        Check.MustBeArgumentNotNull(ns);

        var result = new CodeTypeDeclaration(className)
        {
            IsClass = isClass,
            IsPartial = isPartial,
            TypeAttributes = typeAttributes,
        };
        if (isStatic)
        {
            result.Attributes |= MemberAttributes.Static;
        }

        var bts = baseTypes?.ToList();
        if (bts?.Any() == true)
        {
            bts.ForEach(result.BaseTypes.Add);
        }

        _ = ns.Types.Add(result);
        return result;
    }
    /// <summary>
    /// Adds the partial method.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="name">The name.</param>
    /// <param name="returnType">Type of the return.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">nameof(c)</exception>
    public static CodeTypeDeclaration AddPartialMethod(this CodeTypeDeclaration c, in string name, in string? returnType = null)
    {
        Check.MustBeArgumentNotNull(c);
        var method = NewPartialMethodAsField(name, returnType);
        _ = c.Members.Add(method);
        return c;
    }

    /// <summary>
    /// Adds the property.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="propertyInfo">The property information.</param>
    /// <returns></returns>
    public static CodeTypeDeclaration AddProperty(this CodeTypeDeclaration c, in CodeGeneration.Models.PropertyInfo propertyInfo)
    {
        Check.MustBeArgumentNotNull(c);
        Check.MustBeArgumentNotNull(propertyInfo);

        const int INDENT_SIZE = 4;
        var nullableSign = propertyInfo.IsNullable ? "?" : "";

        var indent = new string(' ', INDENT_SIZE);
        var signature = $"{indent}public {propertyInfo.Type.FullPath}{nullableSign} {ToPropName(propertyInfo.Name.Trim())}";
        var getterStatement = $"{indent}{indent}";
        var setterStatement = $"{indent}{indent}";
        var g = propertyInfo.Getter ?? new(true, false);
        var s = propertyInfo.Setter ?? new(true, false);
        if (g.Has)
        {
            if (g.IsPrivate is true)
            {
                getterStatement = "private ";
            }

            if (propertyInfo.HasBackingField)
            {
                var bf = propertyInfo.BackingFieldName ?? ToFieldName(propertyInfo.Name);
                getterStatement = $"{getterStatement}get => this.{bf};";
            }
            else
            {
                getterStatement = $"{getterStatement}get";
                getterStatement = g.Code.IsNullOrEmpty()
                    ? $"{getterStatement};"
                    : new StringBuilder(getterStatement)
                        .AppendLine()
                        .Append($"{indent}{indent}")
                        .Append('{')
                        .AppendLine()
                        .Append($"{indent}{indent}{indent}")
                        .Append(g.Code)
                        .AppendLine()
                        .Append($"{indent}{indent}")
                        .Append('}').ToString();
            }
        }
        if (s.Has)
        {
            if (s.IsPrivate is true)
            {
                setterStatement = "private ";
            }

            if (s.Code.IsNullOrEmpty())
            {
                if (propertyInfo.HasBackingField)
                {
                    var bf = propertyInfo.BackingFieldName ?? ToFieldName(propertyInfo.Name);
                    setterStatement = $"{setterStatement}set => this.{bf} = value;";
                }
                else
                {
                    setterStatement = $"{setterStatement}set;";
                }
            }
            else
            {
                setterStatement = $"{setterStatement}set";
                setterStatement = $"{setterStatement}{Environment.NewLine}{indent}{indent}{{";
                if (propertyInfo.HasBackingField)
                {
                    var bf = propertyInfo.BackingFieldName ?? ToFieldName(propertyInfo.Name);
                    setterStatement = $"{setterStatement}{Environment.NewLine}{indent}{indent}{indent}this.{bf} = value;";
                }
                setterStatement = $"{setterStatement}{Environment.NewLine}{indent}{indent}{indent}{s.Code}";
                setterStatement = $"{setterStatement}{Environment.NewLine}{indent}{indent}}}";
            }
        }
        var statement = $"{signature}{Environment.NewLine}{indent}{{{Environment.NewLine}{getterStatement}{Environment.NewLine}{setterStatement}{Environment.NewLine}{indent}}}";
        if (!propertyInfo.InitCode.IsNullOrEmpty())
        {
            statement = $"{statement} = {propertyInfo.InitCode}";
        }

        if (propertyInfo.HasBackingField)
        {
            var bf = propertyInfo.BackingFieldName ?? ToFieldName(propertyInfo.Name);
            _ = c.AddField(propertyInfo.Type.FullName, bf);
        }
        if (propertyInfo.Attributes?.Any() is true)
        {
            var attrStatements = string.Empty;
            foreach (var attribute in propertyInfo.Attributes.Compact())
            {
                attrStatements = $"{new string(' ', INDENT_SIZE)}[{attribute}]{Environment.NewLine}{attrStatements}";
            }
            statement = $"{attrStatements.Trim([.. Environment.NewLine])}{Environment.NewLine}{statement}";
        }
        var prop = new CodeSnippetTypeMember(statement)
        {
            Attributes = propertyInfo.AccessModifier ?? MemberAttributes.Public | MemberAttributes.Final
        };
        if (propertyInfo.Comment is not null)
        {
            //_ = prop.Comments.Add(new CodeCommentStatement(propertyInfo.Comment));
            _ = prop.AddSummary(propertyInfo.Comment);
        }
        _ = c.Members.Add(prop);
        return c;
    }

    /// <summary>
    /// Adds new property to specific CodeTypeDeclaration.
    /// </summary>
    /// <param name="c">The CodeTypeDeclaration.</param>
    /// <param name="type">The type.</param>
    /// <param name="name">The name.</param>
    /// <param name="comment">The comment.</param>
    /// <param name="accessModifier">The access modifier.</param>
    /// <param name="getter">The getter.</param>
    /// <param name="setter">The setter.</param>
    /// <param name="initCode">The initialize code.</param>
    /// <param name="isNullable">if set to <c>true</c> [is nullable].</param>
    /// <returns></returns>
    public static CodeTypeDeclaration AddProperty(this CodeTypeDeclaration c,
        in string type,
        in string name,
        in string? comment = null,
        in MemberAttributes? accessModifier = null,
        in PropertyAccessor? getter = null,
        in PropertyAccessor? setter = null,
        in string? initCode = null,
        bool isNullable = false,
        in IEnumerable<string>? attributes = null)
    {
        CodeGeneration.Models.PropertyInfo prop = new()
        {
            Type = type,
            Name = name,
            Comment = comment,
            AccessModifier = accessModifier,
            Getter = getter,
            Setter = setter,
            InitCode = initCode,
            IsNullable = isNullable
        };
        if (attributes?.Any() is true)
        {
            prop.Attributes.AddRange(attributes);
        }

        return AddProperty(c, prop);
    }

    /// <summary>
    /// Adds the region.
    /// </summary>
    /// <param name="unit">The unit.</param>
    /// <param name="statement">The statement.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">nameof(unit)</exception>
    public static CodeCompileUnit AddRegion(this CodeCompileUnit unit, string statement)
    {
        _ = unit.NotNull().StartDirectives.Add(new CodeRegionDirective(CodeRegionMode.Start, statement));
        _ = unit.EndDirectives.Add(new CodeRegionDirective(CodeRegionMode.End, string.Empty));
        return unit;
    }

    /// <summary>
    /// Adds a summary comment to the specified CodeTypeMember.
    /// </summary>
    /// <param name="t">The CodeTypeMember to add the summary comment to.</param>
    /// <param name="comment">The comment to add.</param>
    /// <returns>The CodeTypeMember with the summary comment added.</returns>
    public static T AddSummary<T>(this T t, in string comment)
            where T : CodeTypeMember
    {
        Check.MustBeArgumentNotNull(t);

        _ = t.Comments.Add(new CodeCommentStatement("<summary>", true));
        _ = t.Comments.Add(new CodeCommentStatement(comment, true));
        _ = t.Comments.Add(new CodeCommentStatement("</summary>", true));
        return t;
    }

    public static CodeCompileUnit Begin() =>
        new();

    /// <summary>
    /// Generates a string of C# code from a CodeCompileUnit and optional directives.
    /// </summary>
    /// <param name="unit">The CodeCompileUnit to generate code from.</param>
    /// <param name="directives">Optional directives to include in the generated code.</param>
    /// <returns>A string of C# code.</returns>
    public static string GenerateCode(this CodeCompileUnit unit, params string[] directives)
    {
        var options = new CodeGeneratorOptions { BracingStyle = "C", BlankLinesBetweenMembers = true, VerbatimOrder = true, ElseOnClosing = false, IndentString = "    " };
        using var writer = new StringWriter();
        using var provider = new CSharpCodeProvider();
        foreach (var directive in directives)
        {
            provider.GenerateCodeFromCompileUnit(new CodeSnippetCompileUnit(directive), writer, options);
        }

        provider.GenerateCodeFromCompileUnit(unit, writer, options);
        var result = writer.ToString();
        result = RoslynHelper.ReformatCode(result);
        return result;
    }

    /// <summary>
    /// Creates a new CodeMemberField from the given FieldInfo.
    /// </summary>
    /// <param name="fieldInfo">The FieldInfo to create the CodeMemberField from.</param>
    /// <returns>A new CodeMemberField.</returns>
    public static CodeMemberField NewField(
        in string type,
        in string name,
        in string? comment = null,
        in MemberAttributes? accessModifier = null,
        in bool isReadOnly = false,
        in bool isPartial = false)
    {
        var fieldName = ToFieldName(name.ArgumentNotNull());
        var result = new CodeMemberField
        {
            Attributes = accessModifier ?? MemberAttributes.Private,
            Name = fieldName,
            Type = new(new CodeTypeParameter(type))
        };
        if (comment is not null)
        {
            _ = result.Comments.Add(new CodeCommentStatement(comment));
        }

        return result;
    }

    /// <summary>
    /// Creates a new CodeMemberMethod with the given parameters.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="body">The body of the method.</param>
    /// <param name="returnType">The return type of the method.</param>
    /// <param name="accessModifiers">The access modifiers of the method.</param>
    /// <param name="isPartial">Whether the method is partial.</param>
    /// <param name="arguments">The arguments of the method.</param>
    /// <returns>A new CodeMemberMethod.</returns>
    public static CodeMemberMethod NewMethod(
        in string name,
        in string? body = null,
        in string? returnType = null,
        in MemberAttributes? accessModifiers = null,
        in bool isPartial = false,
        IEnumerable<(string Type, string Name)>? arguments = null)
    {
        Check.MustBeArgumentNotNull(name);
        var method = new CodeMemberMethod
        {
            Attributes = isPartial ? MemberAttributes.ScopeMask : (accessModifiers ?? MemberAttributes.Public | MemberAttributes.Final),
            Name = name,

        };
        if (returnType is not null)
        {
            method.ReturnType = new(returnType);
        }

        if (body is not null)
        {
            _ = method.Statements.Add(new CodeSnippetStatement(body));
        }

        if (arguments?.Any() == true)
        {
            foreach ((var argType, var argName) in arguments)
            {
                _ = method.Parameters.Add(new CodeParameterDeclarationExpression(TypePath.New(argType).FullName, ToArgName(argName)));
            }
        }

        return method;
    }

    /// <summary>
    /// Creates a new partial field with the specified name and return type.
    /// </summary>
    /// <param name="name">The name of the field.</param>
    /// <param name="returnTypeFullName">The full name of the return type.</param>
    /// <returns>A <see cref="CodeMemberField"/> representing the partial field.</returns>
    public static CodeMemberField NewPartialField(string name, string? returnTypeFullName)
    {
        var accessModifiers = MemberAttributes.ScopeMask;

        var method = new CodeMemberField
        {
            Name = $"{name}",
            Attributes = accessModifiers,
            Type = returnTypeFullName.IsNullOrEmpty()
                ? new("partial void")
                : new($"partial {returnTypeFullName}")
        };
        return method;
    }

    /// <summary>
    /// Creates a new partial method as a field.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="returnType">The return type of the method.</param>
    /// <returns>A CodeMemberField representing the partial method.</returns>
    public static CodeMemberField NewPartialMethodAsField(in string name, in string? returnType = null)
    {
        var accessModifiers = MemberAttributes.ScopeMask;

        var method = new CodeMemberField
        {
            Name = $"{name}()",
            Attributes = accessModifiers,
            Type = returnType is null
                ? new("partial void")
                : new($"partial {returnType}")
        };
        return method;
    }

    /// <summary>
    /// Creates a new partial method as a field.
    /// </summary>
    /// <param name="name">The name of the method.</param>
    /// <param name="returnTypeFullName">The full name of the return type.</param>
    /// <returns>A <see cref="CodeMemberField"/> representing the partial method.</returns>
    public static CodeMemberField NewPartialMethodAsField(string name, string? returnTypeFullName)
    {
        var accessModifiers = MemberAttributes.ScopeMask;

        var method = new CodeMemberField
        {
            Name = $"{name}()",
            Attributes = accessModifiers,
            Type = returnTypeFullName.IsNullOrEmpty()
                ? new("partial void")
                : new($"partial {returnTypeFullName}")
        };
        return method;
    }

    /// <summary>
    /// Removes the auto-generated tag from the given code statement.
    /// </summary>
    /// <param name="codeStatement">The code statement.</param>
    /// <returns>The code statement without the auto-generated tag.</returns>
    public static string RemoveAutoGeneratedTag(in string codeStatement)
            => codeStatement.ArgumentNotNull(nameof(codeStatement)).Remove(@"//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated by a tool.
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------
") ?? string.Empty;

    /// <summary>
    /// Adds a namespace import to the given CodeNamespace.
    /// </summary>
    /// <param name="ns">The CodeNamespace to add the import to.</param>
    /// <param name="nameSpace">The namespace to import.</param>
    /// <returns>The CodeNamespace with the added import.</returns>
    public static CodeNamespace UseNameSpace(this CodeNamespace ns, in string nameSpace)
    {
        Check.MustBeArgumentNotNull(ns);

        if (!nameSpace.IsNullOrEmpty())
        {
            ns.Imports.Add(new CodeNamespaceImport(nameSpace));
        }

        return ns;
    }

    /// <summary>
    /// Adds the specified namespaces to the given CodeNamespace.
    /// </summary>
    /// <param name="ns">The CodeNamespace to add the namespaces to.</param>
    /// <param name="nameSpaces">The namespaces to add.</param>
    /// <returns>The CodeNamespace with the added namespaces.</returns>
    public static CodeNamespace UseNameSpace(this CodeNamespace ns, in IEnumerable<string> nameSpaces)
    {
        foreach (var nameSpace in nameSpaces.ArgumentNotNull())
        {
            _ = ns.UseNameSpace(nameSpace);
        }
        return ns;
    }
}

