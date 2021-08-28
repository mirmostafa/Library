using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace Library.SourceGenerator;

[Generator]
public class AutoNotifyGenerator : AttibuteBasedGeneratorBase
{
    private const string attributeText = @"
namespace Library.SourceGenerator.Contracts;

[AttributeUsage(AttributeTargets.Field, Inherited = false, AllowMultiple = false)]
[System.Diagnostics.Conditional(""AutoNotifyGenerator_DEBUG"")]
internal sealed class AutoNotifyAttribute : Attribute
{
    public AutoNotifyAttribute()
    {
    }
    public string PropertyName { get; set; }
}";
    private const string _Attribute = "Library.SourceGenerator.Contracts.AutoNotifyAttribute";

    public AutoNotifyGenerator()
        : base(_Attribute, attributeText)
    {
    }
    protected override void OnExecuting(GeneratorExecutionContext context, SyntaxReceiver receiver)
    {
        // get the added attribute, and INotifyPropertyChanged
        var attributeSymbol = context.Compilation.GetTypeByMetadataName(this._Attribute);
        var notifySymbol = context.Compilation.GetTypeByMetadataName("System.ComponentModel.INotifyPropertyChanged");

        // group the fields by class, and generate the source
        foreach (var group in receiver.Fields.GroupBy(f => f.ContainingType))
        {
            var classSource = this.ProcessClass(group.Key, group.ToList(), attributeSymbol, notifySymbol, context);
            context.AddSource($"{group.Key.Name}.Generated.cs", SourceText.From(classSource, Encoding.UTF8));
        }
    }

    private string ProcessClass(INamedTypeSymbol classSymbol, List<IFieldSymbol> fields, ISymbol attributeSymbol, ISymbol notifySymbol, GeneratorExecutionContext context)
    {
        if (!classSymbol.ContainingSymbol.Equals(classSymbol.ContainingNamespace, SymbolEqualityComparer.Default))
        {
            return null; //TODO: issue a diagnostic that it must be top level
        }

        var namespaceName = classSymbol.ContainingNamespace.ToDisplayString();

        // begin building the generated source
        var source = new StringBuilder($@"
namespace {namespaceName}
{{
    public partial class {classSymbol.Name} : {notifySymbol.ToDisplayString()}
    {{
");

        // if the class doesn't implement INotifyPropertyChanged already, add it
        if (!classSymbol.Interfaces.Contains(notifySymbol))
        {
            source.Append("public event System.ComponentModel.PropertyChangedEventHandler PropertyChanged;");
        }

        // create properties for each field 
        foreach (var fieldSymbol in fields)
        {
            this.ProcessField(source, fieldSymbol, attributeSymbol);
        }

        source.Append("} }");
        return source.ToString();
    }

    private void ProcessField(StringBuilder source, IFieldSymbol fieldSymbol, ISymbol attributeSymbol)
    {
        // get the name and type of the field
        var fieldName = fieldSymbol.Name;
        var fieldType = fieldSymbol.Type;

        // get the AutoNotify attribute from the field, and any associated data
        var attributeData = fieldSymbol.GetAttributes().Single(ad => ad.AttributeClass.Equals(attributeSymbol, SymbolEqualityComparer.Default));
        var overridenNameOpt = attributeData.NamedArguments.SingleOrDefault(kvp => kvp.Key == "PropertyName").Value;

        var propertyName = chooseName(fieldName, overridenNameOpt);
        if (propertyName.Length == 0 || propertyName == fieldName)
        {
            //TODO: issue a diagnostic that we can't process this field
            return;
        }

        source.Append($@"
public {fieldType} {propertyName} 
{{
    get 
    {{
        return this.{fieldName};
    }}

    set
    {{
        this.{fieldName} = value;
        this.PropertyChanged?.Invoke(this, new System.ComponentModel.PropertyChangedEventArgs(nameof({propertyName})));
    }}
}}

");

        string chooseName(string fieldName, TypedConstant overridenNameOpt)
        {
            if (!overridenNameOpt.IsNull)
            {
                return overridenNameOpt.Value.ToString();
            }

            fieldName = fieldName.TrimStart('_');
            if (fieldName.Length == 0)
            {
                return string.Empty;
            }

            if (fieldName.Length == 1)
            {
                return fieldName.ToUpper();
            }

            return fieldName.Substring(0, 1).ToUpper() + fieldName.Substring(1);
        }
    }
}
