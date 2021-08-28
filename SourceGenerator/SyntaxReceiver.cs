using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace Library.SourceGenerator;

/// <summary>
/// Created on demand before each generation pass
/// </summary>
public class SyntaxReceiver : ISyntaxContextReceiver
{
    private readonly string _Attribute;

    public SyntaxReceiver(string attribute) => this._Attribute = attribute;

    public List<IFieldSymbol> Fields => new();

    /// <summary>
    /// Called for every syntax node in the compilation, we can inspect the nodes and save any information useful for generation
    /// </summary>
    public void OnVisitSyntaxNode(GeneratorSyntaxContext context)
    {
        // any field with at least one attribute is a candidate for property generation
        if (context.Node is FieldDeclarationSyntax fieldDeclarationSyntax && fieldDeclarationSyntax.AttributeLists.Count > 0)
        {
            foreach (var variable in fieldDeclarationSyntax.Declaration.Variables)
            {
                // Get the symbol being declared by the field, and keep it if its annotated
                var fieldSymbol = context.SemanticModel.GetDeclaredSymbol(variable) as IFieldSymbol;
                if (fieldSymbol?.GetAttributes().Any(ad => ad.AttributeClass?.ToDisplayString() == this._Attribute) is true)
                {
                    this.Fields.Add(fieldSymbol);
                }
            }
        }
    }
}