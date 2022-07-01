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
    /// Adds the constructor.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="arguments">The arguments.</param>
    /// <param name="body">The body.</param>
    /// <param name="accessModifiers">The access modifiers.</param>
    /// <param name="comment">The comment.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// nameof(c)
    /// or
    /// nameof(c)
    /// </exception>
    public static CodeTypeDeclaration AddConstructor(this CodeTypeDeclaration c,
        in IEnumerable<(string Type, string Name, string DataMemberName)> arguments,
        in string? body = null,
        in MemberAttributes? accessModifiers = null,
        in string? comment = null)
    {
        Check.IfArgumentNotNull(arguments);
        Check.IfArgumentNotNull(c);
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
    /// Adds the constructor.
    /// </summary>
    /// <param name="c">The c.</param>
    /// <param name="arguments">The arguments.</param>
    /// <param name="body">The body.</param>
    /// <param name="accessModifiers">The access modifiers.</param>
    /// <param name="comment">The comment.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">
    /// nameof(c)
    /// or
    /// nameof(c)
    /// </exception>
    public static CodeTypeDeclaration AddConstructor(this CodeTypeDeclaration c,
        in IEnumerable<(string Type, string Name, string DataMemberName, bool IsPropery)> arguments,
        in string? body = null,
        in MemberAttributes? accessModifiers = null,
        in string? comment = null)
    {
        Check.IfArgumentNotNull(c);
        Check.IfArgumentNotNull(arguments);
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

    public static CodeTypeDeclaration AddField(this CodeTypeDeclaration c, in CodeGeneration.Models.FieldInfo fieldInfo)
    {
        Check.IfArgumentNotNull(c);

        var field = NewField(fieldInfo);

        _ = c.Members.Add(field);
        return c;
    }

    public static CodeTypeDeclaration AddInterfaces(this CodeTypeDeclaration codeTypeDeclaration, in IEnumerable<string> interfaces)
    {
        Check.IfArgumentNotNull(codeTypeDeclaration);
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
        Check.IfArgumentNotNull(c);
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
        Check.IfArgumentNotNull(ns);

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
        Check.IfArgumentNotNull(unit);

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
        Check.IfArgumentNotNull(c);
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

        var inednt = new string(' ', INDENT_SIZE);
        var signatue = $"{inednt}public {propertyInfo.Type}{nullableSign} {ToPropName(propertyInfo.Name.Trim())}";
        var getterStatement = $"{inednt}{inednt}";
        var setterStatement = $"{inednt}{inednt}";
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
                                            .Append($"{inednt}{inednt}")
                                            .Append('{')
                                            .AppendLine()
                                            .Append($"{inednt}{inednt}{inednt}")
                                            .Append(g.Code)
                                            .AppendLine()
                                            .Append($"{inednt}{inednt}")
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
                setterStatement = $"{setterStatement}{Environment.NewLine}{inednt}{inednt}{{";
                if (pi.HasBackingField)
                {
                    var bf = pi.BackingFieldName ?? ToFieldName(pi.Name);
                    setterStatement = $"{setterStatement}{Environment.NewLine}{inednt}{inednt}{inednt}this.{bf} = value;";
                }
                setterStatement = $"{setterStatement}{Environment.NewLine}{inednt}{inednt}{inednt}{s.Code}";
                setterStatement = $"{setterStatement}{Environment.NewLine}{inednt}{inednt}}}";
            }
        }
        var statement = $"{signatue}{Environment.NewLine}{inednt}{{{Environment.NewLine}{getterStatement}{Environment.NewLine}{setterStatement}{Environment.NewLine}{inednt}}}";
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

    public static T AddSummary<T>(this T t, in string comment)
            where T : CodeTypeMember
    {
        Check.IfArgumentNotNull(t);

        _ = t.Comments.Add(new CodeCommentStatement("<summary>", true));
        _ = t.Comments.Add(new CodeCommentStatement(comment, true));
        _ = t.Comments.Add(new CodeCommentStatement("</summary>", true));
        return t;
    }

    /// <summary>
    /// Generates the code.
    /// </summary>
    /// <param name="unit">The unit.</param>
    /// <param name="directives">The directives.</param>
    /// <returns></returns>
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
    /// Adds the comment.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="t">The t.</param>
    /// <param name="comment">The comment.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">nameof(t)</exception>
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
    /// Creates new method.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="body">The body.</param>
    /// <param name="returnType">Type of the return.</param>
    /// <param name="accessModifiers">The access modifiers.</param>
    /// <param name="isPartial">if set to <c>true</c> [is partial].</param>
    /// <param name="arguments">The arguments.</param>
    /// <returns></returns>
    public static CodeMemberMethod NewMethod(
        in string name,
        in string? body = null,
        in string? returnType = null,
        in MemberAttributes? accessModifiers = null,
        in bool isPartial = false,
        params MethodArgument[] arguments)
    {
        Check.IfArgumentNotNull(name);
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
    /// Creates new partialfield.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="returnTypeFullName">Full name of the return type.</param>
    /// <returns></returns>
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
    /// Creates new partialmethodasfield.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="returnType">Type of the return.</param>
    /// <returns></returns>
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
    /// Creates new partialmethodasfield.
    /// </summary>
    /// <param name="name">The name.</param>
    /// <param name="returnTypeFullName">Full name of the return type.</param>
    /// <returns></returns>
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
    /// Removes the automatic generated tag.
    /// </summary>
    /// <param name="codeStatement">The code statement.</param>
    /// <returns></returns>
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
    /// Uses the name space.
    /// </summary>
    /// <param name="ns">The ns.</param>
    /// <param name="nameSpace">The name space.</param>
    /// <returns></returns>
    public static CodeNamespace UseNameSpace(this CodeNamespace ns, in string nameSpace)
    {
        Check.IfArgumentNotNull(ns);

        if (!nameSpace.IsNullOrEmpty())
        {
            ns.Imports.Add(new CodeNamespaceImport(nameSpace));
        }

        return ns;
    }

    /// <summary>
    /// Uses the name space.
    /// </summary>
    /// <param name="ns">The ns.</param>
    /// <param name="nameSpaces">The name spaces.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentNullException">ns</exception>
    public static CodeNamespace UseNameSpace(this CodeNamespace ns, in IEnumerable<string> nameSpaces)
    {
        foreach (var nameSpace in nameSpaces.ArgumentNotNull())
        {
            _ = ns.UseNameSpace(nameSpace);
        }
        return ns;
    }
    //public static MemberAttributes ToMemberAttributes(this AccessModifier modifier)
    //    => modifier switch
    //    {
    //        AccessModifier.Private => MemberAttributes.Private,
    //        AccessModifier.Protected => MemberAttributes.Family,
    //        AccessModifier.Internal => MemberAttributes.Assembly,
    //        AccessModifier.InternalProtected => MemberAttributes.FamilyAndAssembly,
    //        AccessModifier.Public => MemberAttributes.Public,
    //        AccessModifier.None => throw new System.NotImplementedException(),
    //        _ => throw new System.NotImplementedException(),
    //    };
}
