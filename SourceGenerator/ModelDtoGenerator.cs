using System.Diagnostics;
using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace Library.SourceGenerator;

[Generator]
public class ModelDtoGenerator : ISourceGenerator
{
    private const string attributeText = @"
namespace Library.SourceGenerator.Contracts;

[AttributeUsage(AttributeTargets.Class)]
public sealed class GenerateDtoAttribute : System.Attribute
{
    public string? DtoClassName { get; set; }
}";
    public void Execute(GeneratorExecutionContext context)
    {
        var syntaxTrees = context.Compilation.SyntaxTrees;
        foreach (var syntaxTree in syntaxTrees)
        {
            var generateDtoClasses = syntaxTree.GetRoot().DescendantNodes().OfType<TypeDeclarationSyntax>()
                .Where(x => x.AttributeLists.Any(xx => xx.ToString().StartsWith("[GenerateDto")));
            foreach (var generateDtoClass in generateDtoClasses)
            {
                var usingDirectives = syntaxTree.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();
                var usingDirectivesAsText = string.Join("\r\n", usingDirectives);
                var sourceBuilder = new StringBuilder(usingDirectivesAsText);
                var className = generateDtoClass.Identifier.ToString();
                var dtoClassName = $"{className}Dto";
                var splitClass = generateDtoClass.ToString().Split(new[] { '{' }, 2);
                sourceBuilder.Append($@"
namespace Models
{{
    public class {dtoClassName}
    {{
");

                sourceBuilder.AppendLine(splitClass[1].Replace(className, dtoClassName));
                sourceBuilder.AppendLine("    }");
                context.AddSource($"{dtoClassName}.Generated.cs", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
                Debug.WriteLine($"{dtoClassName} generated.");
            }
        }
    }
    public void Initialize(GeneratorInitializationContext context)
    {
#if DEBUG1
        if (!Debugger.IsAttached)
        {
            Debugger.Launch();
        }
#endif
        // Register the attribute source
        context.RegisterForPostInitialization((i) => i.AddSource("GenerateDtoAttribute", attributeText));

        // Register a syntax receiver that will be created for each generation pass
        context.RegisterForSyntaxNotifications(() => new SyntaxReceiver("Library.SourceGenerator.Contracts.GenerateDtoAttribute"));
    }
}
