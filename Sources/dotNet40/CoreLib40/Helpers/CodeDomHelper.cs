#region File Notice
// Created at: 2013/12/24 3:43 PM
// Last Update time: 2013/12/24 4:04 PM
// Last Updated by: Mohammad Mir mostafa
#endregion

#region
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Reflection;
#endregion

namespace Library40.Helpers
{
	public static class CodeDomHelper
	{
		public static void AddDocComment(this CodeTypeMember result, string docComments)
		{
			result.Comments.Add(new CodeCommentStatement(string.Format("<summary>{0}</summary>", docComments), true));
		}

		public static void AddNamespace(this CodeCompileUnit unit, params CodeNamespace[] namespaces)
		{
			namespaces.ForEach<CodeNamespace>(ns => unit.Namespaces.Add(ns));
		}

		public static void AddUsing(this CodeNamespace codeNamespace, params string[] references)
		{
			references.ForEach(reference => codeNamespace.Imports.Add(new CodeNamespaceImport(reference)));
		}

		public static void GenerateCSharpCode(this CodeCompileUnit unit, FileInfo outputFile)
		{
			var provider = CodeDomProvider.CreateProvider("CSharp");
			var options = new CodeGeneratorOptions
			              {
				              BracingStyle = "C"
			              };
			if (!outputFile.Exists)
				outputFile.CreateText().Close();
			outputFile.Attributes = outputFile.Attributes.RemoveFlag(FileAttributes.ReadOnly);
			outputFile.Attributes = outputFile.Attributes.RemoveFlag(FileAttributes.System);
			outputFile.Attributes = outputFile.Attributes.RemoveFlag(FileAttributes.Hidden);
			using (var sourceWriter = new StreamWriter(outputFile.FullName))
				provider.GenerateCodeFromCompileUnit(unit, sourceWriter, options);
		}

		public static void GenerateCSharpCode(this CodeCompileUnit unit, StringWriter writer)
		{
			var provider = CodeDomProvider.CreateProvider("CSharp");
			var options = new CodeGeneratorOptions
			              {
				              BracingStyle = "C"
			              };
			provider.GenerateCodeFromCompileUnit(unit, writer, options);
		}

		//public static CodeTypeDeclaration InitClass(string name, string docComments, CodeTypeDeclaration parent)
		//{
		//    var result = new CodeTypeDeclaration(name) { IsClass = true, TypeAttributes = TypeAttributes.Public, IsPartial = true };
		//    if (!docComments.IsNullOrEmpty())
		//        result.AddDocComment(docComments);
		//    parent.Members.Add(result);
		//    return result;
		//}

		//public static CodeTypeDeclaration InitClass(string name, string docComments, CodeNamespace parent)
		//{
		//    var result = new CodeTypeDeclaration(name) { IsClass = true, TypeAttributes = TypeAttributes.Public, IsPartial = true };
		//    if (!docComments.IsNullOrEmpty())
		//        result.AddDocComment(docComments);
		//    parent.Types.Add(result);
		//    return result;
		//}

		public static CodeTypeDeclaration InitClass(string name, string docComments = null, CodeNamespace parent = null, params string[] baseTypeNames)
		{
			var result = new CodeTypeDeclaration(name)
			             {
				             IsClass = true,
				             TypeAttributes = TypeAttributes.Public,
				             IsPartial = true
			             };
			if (!docComments.IsNullOrEmpty())
				result.AddDocComment(docComments);
			if (parent != null)
				parent.Types.Add(result);
			baseTypeNames.ForEach(result.BaseTypes.Add);
			return result;
		}

		public static CodeMemberField InitField(MemberAttributes modifiers, string initValue, Type type, string docComment, CodeTypeDeclaration parent, string name)
		{
			var result = new CodeMemberField
			             {
				             Attributes = modifiers
			             };
			if (!initValue.IsNullOrEmpty())
				result.InitExpression = new CodeSnippetExpression(initValue);
			result.Name = name;
			result.Type = new CodeTypeReference(type);
			if (!string.IsNullOrEmpty(docComment))
				result.AddDocComment(docComment);
			parent.Members.Add(result);
			return result;
		}

		public static CodeMemberField InitField(MemberAttributes modifiers, string initValue, string typeName, string docComment, CodeTypeDeclaration parent, string name)
		{
			var result = new CodeMemberField
			             {
				             Attributes = modifiers
			             };
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
			var result = new CodeTypeDeclaration(name)
			             {
				             TypeAttributes = TypeAttributes.Public,
				             IsInterface = true
			             };
			if (!docComments.IsNullOrEmpty())
				result.AddDocComment(docComments);
			parent.Types.Add(result);
			return result;
		}

		public static CodeNamespace InitNamespace(string name)
		{
			return new CodeNamespace(name);
		}

		public static CodeMemberProperty InitProperty(MemberAttributes modifiers,
			bool hasGet,
			bool hasSet,
			Type type,
			string docComment,
			CodeTypeDeclaration parent,
			string name,
			string backingFieldName)
		{
			if (string.IsNullOrEmpty(backingFieldName))
				backingFieldName = string.Concat("_", name);
			var result = new CodeMemberProperty
			             {
				             Attributes = modifiers,
				             Name = name,
				             HasGet = hasGet,
				             HasSet = hasSet,
				             Type = new CodeTypeReference(type)
			             };
			if (!docComment.IsNullOrEmpty())
				result.AddDocComment(docComment);
			if (hasGet)
			{
				//CodeMethodReturnStatement t = new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), backingFieldName));
				//result.GetStatements.Add(t);
				var getMethod = new CodeMethodReturnStatement(new CodeFieldReferenceExpression
				                                              {
					                                              FieldName = backingFieldName
				                                              });
				result.GetStatements.Add(getMethod);
			}
			if (hasSet)
				result.SetStatements.Add(new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), backingFieldName),
					new CodePropertySetValueReferenceExpression()));
			parent.Members.Add(result);
			return result;
		}

		public static CodeMemberProperty InitProperty(MemberAttributes modifiers,
			bool hasGet,
			bool hasSet,
			string typeName,
			string docComment,
			CodeTypeDeclaration parent,
			string name,
			string backingFieldName)
		{
			if (string.IsNullOrEmpty(backingFieldName))
				backingFieldName = string.Concat("_", name);
			var result = new CodeMemberProperty
			             {
				             Attributes = modifiers,
				             Name = name,
				             HasGet = hasGet,
				             HasSet = hasSet,
				             Type = new CodeTypeReference(typeName)
			             };
			if (!docComment.IsNullOrEmpty())
				result.AddDocComment(docComment);
			if (hasGet)
			{
				//CodeMethodReturnStatement getMethod = new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), backingFieldName));
				var getMethod = new CodeMethodReturnStatement(new CodeFieldReferenceExpression
				                                              {
					                                              FieldName = backingFieldName
				                                              });
				result.GetStatements.Add(getMethod);
			}
			if (hasSet)
			{
				var setMethod = new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), backingFieldName), new CodePropertySetValueReferenceExpression());
				result.SetStatements.Add(setMethod);
			}
			parent.Members.Add(result);
			return result;
		}

		public static CodeAssignStatement CreatePropSet(CodeMemberField backingField)
		{
			return new CodeAssignStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), backingField.Name), new CodePropertySetValueReferenceExpression());
		}

		public static CodeMethodReturnStatement CreatePropGet(CodeMemberField backingField)
		{
			return new CodeMethodReturnStatement(new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), backingField.Name));
		}

		public static CodeMemberProperty CreateProperty(Type type, string name)
		{
			var result = new CodeMemberProperty
			             {
				             Attributes = MemberAttributes.Public,
				             Type = new CodeTypeReference(type),
				             Name = name,
				             HasGet = true,
				             HasSet = true,
			             };
			return result;
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

		public static CodeMemberProperty AddPropertyWithBackingField(this CodeTypeDeclaration typeDeclaration, string typeName, string name)
		{
			var result = CreateProperty(typeName, name);
			var backingField = new CodeMemberField(new CodeTypeReference(typeName), "_" + result.Name);
			typeDeclaration.Members.Add(backingField);

			result.GetStatements.Add(CreatePropGet(backingField));
			result.SetStatements.Add(CreatePropSet(backingField));
			typeDeclaration.Members.Add(result);
			return result;
		}

		public static CodeMemberProperty AddPropertyWithBackingField<TType>(this CodeTypeDeclaration typeDeclaration, string name)
		{
			return AddPropertyWithBackingField(typeDeclaration, typeof (TType), name);
		}

		private static CodeMemberProperty CreateProperty(string typeName, string name)
		{
			var result = new CodeMemberProperty
			             {
				             Attributes = MemberAttributes.Public,
				             Type = new CodeTypeReference(typeName),
				             Name = name,
				             HasGet = true,
				             HasSet = true,
			             };
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
				                      Name = name,
			                      };
			return propertyChanged;
		}

		public static CodeMemberField AddField(this CodeTypeDeclaration typeDeclaration, Type type, string name, string defaultValue, bool isConst, bool isPublic)
		{
			var result = new CodeMemberField(new CodeTypeReference(type), name);
			MemberAttributes attributes = 0;
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