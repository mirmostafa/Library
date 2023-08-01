using System.CodeDom;
using System.CodeDom.Compiler;
using System.Reflection;

using Library.CodeGeneration.Models;
using Library.DesignPatterns.Markers;
using Library.Validations;

using Microsoft.CSharp;

using static Library.Helpers.CodeGen.TypeMemberNameHelper;

namespace Library.Helpers.CodeGen;

[Fluent]
public static class CodeDomHelper
{
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
                                               in MemberAttributes? accessModifier = null)
        => AddField(c, new(type, name, comment, accessModifier ?? MemberAttributes.Private));

    /// <summary>
    /// Adds a field to the given CodeTypeDeclaration.
    /// </summary>
    /// <param name="c">The CodeTypeDeclaration to add the field to.</param>
    /// <param name="fieldInfo">The information about the field to add.</param>
    /// <returns>The CodeTypeDeclaration with the added field.</returns>
    public static CodeTypeDeclaration AddField(this CodeTypeDeclaration c, in CodeGeneration.Models.FieldInfo fieldInfo)
    {
        Check.MustBeArgumentNotNull(c);

        _ = c.Members.Add(NewField(fieldInfo));
        return c;
    }

    /// <summary>
    /// Adds the given interfaces to the given CodeTypeDeclaration.
    /// </summary>
    /// <param name="codeTypeDeclaration">The CodeTypeDeclaration to add the interfaces to.</param>
    /// <param name="interfaces">The interfaces to add.</param>
    /// <returns>The CodeTypeDeclaration with the added interfaces.</returns>
    //This method adds interfaces to a CodeTypeDeclaration object
    public static CodeTypeDeclaration AddInterfaces(this CodeTypeDeclaration codeTypeDeclaration, IEnumerable<string> interfaces)
    {
        Check.MustBeArgumentNotNull(codeTypeDeclaration);
        interfaces?.ToList().ForEach(codeTypeDeclaration.BaseTypes.Add);
        return codeTypeDeclaration;
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
        bool isPartial = false, params MethodArgument[] arguments)
    {
        Check.MustBeArgumentNotNull(c);
        _ = c.Members.Add(NewMethod(name, body, returnType, accessModifiers ?? MemberAttributes.Public | MemberAttributes.Final, isPartial, arguments));
        return c;
    }

    /// <summary>
    ///     Adds a new class.
    /// </summary>
    /// <param name="ns">The namespace.</param>
    /// <param name="className">Name of the class.</param>
    /// <param name="baseTypes">The base types.</param>
    /// <param name="isPartial">if set to <c>true</c> [is partial].</param>
    /// <returns></returns>
    public static CodeTypeDeclaration AddNewClass(this CodeNamespace ns, in string className, in IEnumerable<string>? baseTypes = null, bool isPartial = false)
    {
        Check.MustBeArgumentNotNull(ns);

        CodeTypeDeclaration? result = new(className)
        {
            IsClass = true,
            IsPartial = isPartial,
            TypeAttributes = TypeAttributes.Public | TypeAttributes.Sealed
        };
        var bts = baseTypes?.ToList();
        if (bts?.Any() == true)
        {
            bts.ForEach(result.BaseTypes.Add);
        }

        _ = ns.Types.Add(result);
        return result;
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
    /// Adds the partial method.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="name">The name.</param>
    /// <param name="returnType">Type of the return.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">nameof(c)</exception>
    public static CodeTypeDeclaration AddPartialMethod(this CodeTypeDeclaration c, in string name, in Type? returnType = null)
    {
        Check.MustBeArgumentNotNull(c);
        var method = NewPartialMethodAsField(name, returnType);
        _ = c.Members.Add(method);
        return c;
    }

    /// <summary>
    /// Adds ae property and creates backing-field.
    /// </summary>
    /// <param name="c">The CodeTypeDeclaration.</param>
    /// <param name="type">The type.</param>
    /// <param name="name">The name.</param>
    /// <param name="backingField">The backing field.</param>
    /// <param name="comment">The comment.</param>
    /// <param name="accessModifier">The access modifier.</param>
    /// <param name="hasGet">if set to <c>true</c> creates getter.</param>
    /// <param name="hasSet">if set to <c>true</c> creates setter.</param>
    /// <returns></returns>
    /// <remarks>
    /// This method creates a backing field using <paramref name="backingField"/> data
    /// </remarks>
    public static CodeTypeDeclaration AddPropCreateField(this CodeTypeDeclaration c,
        in string type,
        in string name,
        in string? backingField = null,
        in string? comment = null,
        in MemberAttributes? accessModifier = null,
        in bool hasGet = true,
        in bool hasSet = true)
        => AddProperty(c, new()
        {
            Type = type,
            Name = name,
            Comment = comment,
            AccessModifier = accessModifier,
            Getter = new(hasGet, false),
            Setter = new(hasSet, false),
            HasBackingField = true,
            BackingFieldName = backingField,
            ShouldCreateBackingField = true
        });

    /// <summary>
    /// Adds the property.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="propertyInfo">The property information.</param>
    /// <returns></returns>
    public static CodeTypeDeclaration AddProperty(this CodeTypeDeclaration c, in CodeGeneration.Models.PropertyInfo propertyInfo)
    {
        _ = c.ArgumentNotNull(nameof(c));
        var pi = propertyInfo.ArgumentNotNull(nameof(propertyInfo));
        const int INDENT_SIZE = 4;
        var nullableSign = propertyInfo.IsNullable ? "?" : "";

        var indent = new string(' ', INDENT_SIZE);
        var signature = $"{indent}public {propertyInfo.Type}{nullableSign} {ToPropName(propertyInfo.Name.Trim())}";
        var getterStatement = $"{indent}{indent}";
        var setterStatement = $"{indent}{indent}";
        var g = pi.Getter ?? new(true, false);
        var s = pi.Setter ?? new(true, false);
        if (g.Has)
        {
            if (g.IsPrivate is true)
            {
                getterStatement = "private ";
            }

            if (pi.HasBackingField)
            {
                var bf = pi.BackingFieldName ?? ToFieldName(pi.Name);
                getterStatement = $"{getterStatement}get => this.{bf};";
            }
            else
            {
                getterStatement = $"{getterStatement}get";
                if (g.Code.IsNullOrEmpty())
                {
                    getterStatement = $"{getterStatement};";
                }
                else
                {
                    var buffer = new StringBuilder(getterStatement);
                    getterStatement = buffer.AppendLine()
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
        }
        if (s.Has)
        {
            if (s.IsPrivate is true)
            {
                setterStatement = "private ";
            }

            if (s.Code.IsNullOrEmpty())
            {
                if (pi.HasBackingField)
                {
                    var bf = pi.BackingFieldName ?? ToFieldName(pi.Name);
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
                if (pi.HasBackingField)
                {
                    var bf = pi.BackingFieldName ?? ToFieldName(pi.Name);
                    setterStatement = $"{setterStatement}{Environment.NewLine}{indent}{indent}{indent}this.{bf} = value;";
                }
                setterStatement = $"{setterStatement}{Environment.NewLine}{indent}{indent}{indent}{s.Code}";
                setterStatement = $"{setterStatement}{Environment.NewLine}{indent}{indent}}}";
            }
        }
        var statement = $"{signature}{Environment.NewLine}{indent}{{{Environment.NewLine}{getterStatement}{Environment.NewLine}{setterStatement}{Environment.NewLine}{indent}}}";
        if (!pi.InitCode.IsNullOrEmpty())
        {
            statement = $"{statement} = {pi.InitCode}";
        }

        if (pi.HasBackingField)
        {
            var bf = pi.BackingFieldName ?? ToFieldName(pi.Name);
            if (pi.ShouldCreateBackingField is true)
            {
                _ = c.AddField(pi.Type, bf);
            }
        }
        if (propertyInfo.Attributes?.Any() is true)
        {
            var attrStatements = string.Empty;
            foreach (var attribute in propertyInfo.Attributes.Compact())
            {
                attrStatements = $"{new string(' ', INDENT_SIZE)}[{attribute}]{Environment.NewLine}{attrStatements}";
            }
            statement = $"{attrStatements.Trim(Environment.NewLine.ToArray())}{Environment.NewLine}{statement}";
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
    /// Adds the property with field.
    /// </summary>
    /// <param name="c">The CodeTypeDeclaration.</param>
    /// <param name="type">The type.</param>
    /// <param name="name">The name.</param>
    /// <param name="backingField">The backing field.</param>
    /// <param name="comment">The comment.</param>
    /// <param name="accessModifier">The access modifier.</param>
    /// <param name="hasGet">if set to <c>true</c> [has get].</param>
    /// <param name="hasSet">if set to <c>true</c> [has set].</param>
    /// <returns></returns>
    /// <remarks>
    /// This method DOES NOT create <paramref name="backingField"/>
    /// </remarks>
    public static CodeTypeDeclaration AddPropWithField(this CodeTypeDeclaration c,
        in string type,
        in string name,
        in string? backingField = null,
        in string? comment = null,
        in MemberAttributes? accessModifier = null,
        in bool hasGet = true,
        in bool hasSet = true)
        => AddProperty(c, new()
        {
            Type = type,
            Name = name,
            Comment = comment,
            AccessModifier = accessModifier,
            Getter = new(hasGet, false),
            Setter = new(hasSet, false),
            HasBackingField = true,
            BackingFieldName = backingField
        });

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

    /// <summary>
    /// Generates a string of C# code from a CodeCompileUnit and optional directives.
    /// </summary>
    /// <param name="unit">The CodeCompileUnit to generate code from.</param>
    /// <param name="directives">Optional directives to include in the generated code.</param>
    /// <returns>A string of C# code.</returns>
    public static string GenerateCode(this CodeCompileUnit unit, params string[] directives)
    {
        var options = new CodeGeneratorOptions { BracingStyle = "C", BlankLinesBetweenMembers = true, VerbatimOrder = true, ElseOnClosing = false, IndentString = "    " };
        using var result = new StringWriter();
        using var provider = new CSharpCodeProvider();
        foreach (var directive in directives)
        {
            provider.GenerateCodeFromCompileUnit(new CodeSnippetCompileUnit(directive), result, options);
        }

        provider.GenerateCodeFromCompileUnit(unit, result, options);
        return result.ToString();
    }

    /// <summary>
    /// Creates a new CodeMemberField from the given FieldInfo.
    /// </summary>
    /// <param name="fieldInfo">The FieldInfo to create the CodeMemberField from.</param>
    /// <returns>A new CodeMemberField.</returns>
    public static CodeMemberField NewField(CodeGeneration.Models.FieldInfo fieldInfo)
    {
        var fieldName = ToFieldName(fieldInfo.Name.ArgumentNotNull(nameof(fieldInfo)));
        var result = new CodeMemberField
        {
            Attributes = fieldInfo.AccessModifier ?? MemberAttributes.Private,
            Name = fieldName,
            Type = new(new CodeTypeParameter(fieldInfo.Type))
        };
        if (fieldInfo.Comment is not null)
        {
            _ = result.Comments.Add(new CodeCommentStatement(fieldInfo.Comment));
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
        params MethodArgument[] arguments)
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

        foreach ((var argType, var argName) in arguments)
        {
            _ = method.Parameters.Add(new CodeParameterDeclarationExpression(argType, ToArgName(argName)));
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
    public static CodeMemberField NewPartialMethodAsField(in string name, in Type? returnType = null)
    {
        var accessModifiers = MemberAttributes.ScopeMask;

        var method = new CodeMemberField
        {
            Name = $"{name}()",
            Attributes = accessModifiers,
            Type = returnType is null
                ? new("partial void")
                : new($"partial {returnType.FullName}")
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
