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
namespace GeneratedMappers
{{
    public class {dtoClassName}
    {{
");

                sourceBuilder.AppendLine(splitClass[1].Replace(className, dtoClassName));
                sourceBuilder.AppendLine("}");
                context.AddSource($"MapperGenerator_{dtoClassName}", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));


            }
        }
    }
    public void Initialize(GeneratorInitializationContext context)
    {
#if DEBUG
        if (!Debugger.IsAttached)
        {
            Debugger.Launch();
        }
#endif
    }
}
