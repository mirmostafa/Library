#region Code Identifications

// Created on     2018/04/21
// Last update on 2018/04/21 by Mohammad Mir mostafa 

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using Mohammad.Helpers;

namespace Mohammad.CodeGeneration.Generators
{
    public class GeneratorBasedOnString
    {
        private readonly Namespace _Namespace = new Namespace();
        private readonly LanguageDeclarationTemplateBase _Template;
        public GeneratorBasedOnString(LanguageDeclarationTemplateBase template) { this._Template = template; }
        public string FindClass(string className) => this._Namespace.Classes.Find(c=>c.Name.EqualsTo(className)).Write(this._Template.Class);
        public string AddProperty(string classCode, string propName, string propType) => throw new NotImplementedException();
        public string CreateClass(string ownerName, string @namespace = null) => throw new NotImplementedException();

        public void WriteClass(string classCode) { }
    }

    public abstract class CodeSnippet
    {
        
    }

    public class Namespace : CodeSnippet
    {
        public List<Class> Classes { get; } = new List<Class>();
    }

    public class Class : CodeSnippet
    {
        public Class(string name) { this.Name = name; }
        public string Name { get; }
        public List<Property> Properties { get; } = new List<Property>();
        public List<Field> Fields { get; } = new List<Field>();
        public List<Method> Methods { get; } = new List<Method>();
        public List<Event> Events { get; } = new List<Event>();
    }

    public abstract class ClassMember : CodeSnippet
    {
        public string AccessModifier { get; set; }
        public bool IsVirtual { get; set; }
        public bool IsOverride { get; set; }
        public bool IsSealed { get; set; }
    }

    public class Property : ClassMember
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public bool HasGetter { get; set; } = true;
        public bool HasSetter { get; set; } = true;
    }

    public class Field : ClassMember
    {
        public string Type { get; set; }
        public string Name { get; set; }
        public bool IsReadOnly { get; set; }
        public bool IsConst { get; set; }
    }

    public class Method : ClassMember { }

    public class Event : ClassMember { }

    public abstract class LanguageDeclarationTemplateBase
    {
        public string Namespace { get; }
        public string Class { get; }
        public string Property { get; }
        public string Field { get; }
        public string Event { get; }

        protected LanguageDeclarationTemplateBase(string @namespace, string @class, string property, string field, string @event)
        {
            this.Namespace = @namespace;
            this.Class = @class;
            this.Property = property;
            this.Field = field;
            this.Event = @event;
        }
    }

    public sealed class CSharpLanguageDeclarationTemplate : LanguageDeclarationTemplateBase
    {
        /// <inheritdoc />
        public CSharpLanguageDeclarationTemplate()
            : base(@"namespace
            {
    %classes%
}",
                @"%accessmodifier% class{
    %fields%
    %ctor%
    %properties%
    %methods%

}",
                @"
%accessmodifier% %type% %name% { get; set; }",
                @"%accessmodifier% %type% %name%",
                null) { }
    }

    public static class CodeSnippetWriter
    {
        public static string Write(this Class @class, string template) { throw new NotImplementedException(); }
    }
}