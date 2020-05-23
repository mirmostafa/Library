#region

using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.IO;
using System.Reflection;

#endregion

namespace Mohammad.Helpers
{
    public static class CodeDomHelper
    {
        public static void AddDocComment(this CodeTypeMember result, string docComments)
        {
            result.Comments.Add(new CodeCommentStatement(string.Format("<summary>{0}</summary>", docComments), true));
        }

        public static void AddNamespace(this CodeCompileUnit unit, params CodeNamespace[] namespaces) { namespaces.ForEach(ns => unit.Namespaces.Add(ns)); }

        public static void AddUsing(this CodeNamespace codeNamespace, params string[] references)
        {
            references.ForEach(reference => codeNamespace.Imports.Add(new CodeNamespaceImport(reference)));
        }

        public static void GenerateCSharpCode(this CodeCompileUnit unit, FileInfo outputFile)
        {
            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions {BracingStyle = "C"};
            if (!outputFile.Exists)
                outputFile.CreateText().Close();
            outputFile.Attributes = EnumHelper.RemoveFlag(outputFile.Attributes, FileAttributes.ReadOnly);
            outputFile.Attributes = EnumHelper.RemoveFlag(outputFile.Attributes, FileAttributes.System);
            outputFile.Attributes = EnumHelper.RemoveFlag(outputFile.Attributes, FileAttributes.Hidden);
            using (var sourceWriter = new StreamWriter(outputFile.FullName))
                provider.GenerateCodeFromCompileUnit(unit, sourceWriter, options);
        }

        public static void GenerateCSharpCode(this CodeCompileUnit unit, TextWriter writer)
        {
            var provider = CodeDomProvider.CreateProvider("CSharp");
            var options = new CodeGeneratorOptions {BracingStyle = "C"};
            provider.GenerateCodeFromCompileUnit(unit, writer, options);
        }

        public static CodeTypeDeclaration InitClass(string name, string docComments = null, CodeNamespace parent = null, params string[] baseTypeNames)
        {
            var result = new CodeTypeDeclaration(name) {IsClass = true, TypeAttributes = TypeAttributes.Public, IsPartial = true};
            if (!docComments.IsNullOrEmpty())
                result.AddDocComment(docComments);
            if (parent != null)
                parent.Types.Add(result);
            baseTypeNames.Compact().ForEach(result.BaseTypes.Add);
            return result;
        }

        public static CodeMemberField InitField(MemberAttributes modifiers, string initValue, Type type, string docComment, CodeTypeDeclaration parent, string name)
        {
            var result = new CodeMemberField {Attributes = modifiers};
            if (!initValue.IsNullOrEmpty())
                result.InitExpression = new CodeSnippetExpression(initValue);
            result.Name = name;
            result.Type = new CodeTypeReference(type);
            if (!string.IsNullOrEmpty(docComment))
                result.AddDocComment(docComment);
            parent.Members.Add(result);
            return result;
        }

        public static CodeMemberField InitField(MemberAttributes modifiers, string initValue, string typeName, string docComment, CodeTypeDeclaration parent,
            string name)
        {
            var result = new CodeMemberField {Attributes = modifiers};
            if (!initValue.IsNullOrEmpty())
                result.InitExpression = new CodeSnippetExpression(initValue);
            result.Name = name;
            result.Type = new CodeTypeReference(typeName);
            if (!docComment.IsNullOrEmpty())
                result.AddDocComment(docComment);
            parent.Members.Add(result);
            return result;
        }

        public static CodeTypeDeclaration InitInterface(string name, string docComments, CodeNamespace parent)
        {
            var result = new CodeTypeDeclaration(name) {TypeAttributes = TypeAttributes.Public, IsInterface = true};
            if (!docComments.IsNullOrEmpty())
                result.AddDocComment(docComments);
            parent.Types.Add(result);
            return result;
        }

        public static CodeNamespace InitNamespace(string name) { return new CodeNamespace(name); }

        public static CodeMemberProperty InitProperty(MemberAttributes modifiers, bool hasGet, bool hasSet, Type type, string docComment, CodeTypeDeclaration parent,
            string name, string backingFieldName)
        {
            if (string.IsNullOrEmpty(backingFieldName))
                backingFieldName = string.Concat("_", name);
            var result = new CodeMemberProperty {Attributes = modifiers, Name = name, HasGet = hasGet, HasSet = hasSet, Type = new CodeTypeReference(type)};
            if (!docComment.IsNullOrEmpty())
                result.AddDocComment(docComment);
            if (hasGet)
            {
                //CodeMethodReturnStatement t = new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), backingFieldName));
                //result.GetStatements.Add(t);
                var getMethod = new CodeMethodReturnStatement(new CodeFieldReferenceExpression {FieldName = backingFieldName});
                result.GetStatements.Add(getMethod);
            }
            if (hasSet)
                result.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), backingFieldName),
                    new CodePropertySetValueReferenceExpression()));
            parent.Members.Add(result);
            return result;
        }

        public static CodeMemberProperty InitProperty(MemberAttributes modifiers, bool hasGet, bool hasSet, string typeName, string docComment,
            CodeTypeDeclaration parent, string name, string backingFieldName)
        {
            if (string.IsNullOrEmpty(backingFieldName))
                backingFieldName = string.Concat("_", name);
            var result = new CodeMemberProperty {Attributes = modifiers, Name = name, HasGet = hasGet, HasSet = hasSet, Type = new CodeTypeReference(typeName)};
            if (!docComment.IsNullOrEmpty())
                result.AddDocComment(docComment);
            if (hasGet)
            {
                //CodeMethodReturnStatement getMethod = new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), backingFieldName));
                var getMethod = new CodeMethodReturnStatement(new CodeFieldReferenceExpression {FieldName = backingFieldName});
                result.GetStatements.Add(getMethod);
            }
            if (hasSet)
            {
                var setMethod = new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), backingFieldName),
                    new CodePropertySetValueReferenceExpression());
                result.SetStatements.Add(setMethod);
            }
            parent.Members.Add(result);
            return result;
        }

        public static CodeAssignStatement CreatePropSet(CodeMemberField backingField)
        {
            return new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), backingField.Name),
                new CodePropertySetValueReferenceExpression());
        }

        public static CodeMethodReturnStatement CreatePropGet(CodeMemberField backingField)
        {
            return new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), backingField.Name));
        }

        public static CodeMemberProperty CreateProperty(Type type, string name)
        {
            var result = new CodeMemberProperty {Attributes = MemberAttributes.Public, Type = new CodeTypeReference(type), Name = name, HasGet = true, HasSet = true};
            return result;
        }

        [Obsolete]
        public static CodeMemberProperty CreateAutoProp(Type type, string name, bool isNullable = false)
        {
            var result = new CodeMemberProperty
                         {
                             Attributes = MemberAttributes.Public,
                             Type = new CodeTypeReference(type),
                             Name = name,
                             HasGet = false,
                             HasSet = false
                         };
            return result;
        }

        public static CodeMemberField CreateAutoField(Type type, string name, bool isNullable = false)
        {
            var cfield = new CodeMemberField {Attributes = MemberAttributes.Public, Name = name, Type = new CodeTypeReference(type)};
            cfield.Name += " { get; set; }";
            return cfield;
        }

        public static CodeMemberField CreateAutoField(string type, string name, bool isNullable = false)
        {
            var cfield = new CodeMemberField {Attributes = MemberAttributes.Public, Name = name, Type = new CodeTypeReference(type)};
            cfield.Name += " { get; set; }";
            return cfield;
        }

        [Obsolete]
        public static CodeMemberProperty AddAutoProp(this CodeTypeDeclaration typeDeclaration, Type type, string name, bool isNullable = false)
        {
            var prop = CreateAutoProp(type, name, isNullable);
            typeDeclaration.Members.Add(prop);
            return prop;
        }

        public static CodeMemberField AddAutoField(this CodeTypeDeclaration typeDeclaration, Type type, string name, bool isNullable = false)
        {
            var prop = CreateAutoField(type, name, isNullable);
            typeDeclaration.Members.Add(prop);
            return prop;
        }

        public static CodeMemberField AddAutoField(this CodeTypeDeclaration typeDeclaration, string type, string name, bool isNullable = false)
        {
            var prop = CreateAutoField(type, name, isNullable);
            typeDeclaration.Members.Add(prop);
            return prop;
        }

        public static CodeMemberProperty AddPropertyWithBackingField(this CodeTypeDeclaration typeDeclaration, Type type, string name)
        {
            var result = CreateProperty(type, name);
            var backingField = new CodeMemberField(new CodeTypeReference(type), "_" + result.Name);
            typeDeclaration.Members.Add(backingField);

            result.GetStatements.Add(CreatePropGet(backingField));
            result.SetStatements.Add(CreatePropSet(backingField));
            typeDeclaration.Members.Add(result);
            return result;
        }

        public static CodeMemberProperty AddPropertyWithBackingField(this CodeTypeDeclaration typeDeclaration, string typeName, string name,
            string backingFieldValue = null, bool isReadOnly = false)
        {
            var result = CreateProperty(typeName, name);
            var backingField = new CodeMemberField(new CodeTypeReference(typeName), "_" + result.Name);

            if (!backingFieldValue.IsNullOrEmpty())
                backingField.InitExpression = new CodeSnippetExpression(backingFieldValue);

            typeDeclaration.Members.Add(backingField);

            result.GetStatements.Add(CreatePropGet(backingField));
            result.SetStatements.Add(CreatePropSet(backingField));
            typeDeclaration.Members.Add(result);
            result.HasSet = !isReadOnly;
            return result;
        }

        public static CodeMemberProperty AddPropertyWithBackingField<TType>(this CodeTypeDeclaration typeDeclaration, string name)
        {
            return AddPropertyWithBackingField(typeDeclaration, typeof(TType), name);
        }

        public static CodeMemberProperty CreateProperty(string typeName, string name)
        {
            var result = new CodeMemberProperty
                         {
                             Attributes = MemberAttributes.Public,
                             Type = new CodeTypeReference(typeName),
                             Name = name,
                             HasGet = true,
                             HasSet = true
                         };
            return result;
        }

        public static CodeMemberProperty AddProperty(this CodeTypeDeclaration typeDeclaration, string typeName, string name)
        {
            var result = CreateProperty(typeName, name);
            typeDeclaration.Members.Add(result);
            return result;
        }

        public static CodeMemberMethod AddMethod(this CodeTypeDeclaration typeDeclaration, string name, Dictionary<string, string> nameTypeParams = null,
            string returnName = null, MemberAttributes attributes = MemberAttributes.Public | MemberAttributes.Final)
        {
            var result = new CodeMemberMethod {Name = name};
            if (returnName != null)
                result.ReturnType = new CodeTypeReference(returnName);
            if (nameTypeParams != null)
                foreach (var param in nameTypeParams)
                    result.Parameters.Add(new CodeParameterDeclarationExpression(param.Value, param.Key));
            result.Attributes = attributes;
            typeDeclaration.Members.Add(result);
            result.Statements.Add(new CodeMethodInvokeExpression(null, "throw new NotImplementedException"));
            return result;
        }

        public static CodeMemberEvent CreateEvent(CodeTypeDeclaration eventArgs, string name)
        {
            var propertyChanged = new CodeMemberEvent
                                  {
                                      Attributes = MemberAttributes.Public,
                                      Type =
                                          eventArgs.Name == null
                                              ? new CodeTypeReference("System.EventHandler")
                                              : new CodeTypeReference(string.Format("System.EventHandler<{0}>", eventArgs.Name)),
                                      Name = name
                                  };
            return propertyChanged;
        }

        public static CodeMemberField AddField(this CodeTypeDeclaration typeDeclaration, Type type, string name, string defaultValue, bool isStatic, bool isConst,
            bool isPublic)
        {
            var result = new CodeMemberField(new CodeTypeReference(type), name);
            MemberAttributes attributes = 0;
            if (isStatic)
                attributes |= MemberAttributes.Static;
            if (isConst)
                attributes |= MemberAttributes.Const;
            if (isPublic)
                attributes |= MemberAttributes.Public;
            result.Attributes = attributes;
            result.InitExpression = new CodeSnippetExpression(defaultValue);
            typeDeclaration.Members.Add(result);
            return result;
        }
    }
}