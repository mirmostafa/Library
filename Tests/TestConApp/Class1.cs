using System.Diagnostics;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace TestConApp
{
    [Generator]
    public class ControllerGenerator : ISourceGenerator
    {
        public void Execute(GeneratorExecutionContext context)
        {
            var syntaxTrees = context.Compilation.SyntaxTrees;
            var handlers = syntaxTrees.Where(x => x.GetText().ToString().Contains("[Http"));
            foreach (var handler in handlers)
            {
                var usingDirectives = handler.GetRoot().DescendantNodes().OfType<UsingDirectiveSyntax>();
                var usingDirectivesAsText = string.Join("\r\n", usingDirectives);
                var sourceBuilder = new StringBuilder(usingDirectivesAsText);
                var classDeclarationSyntax =
                handler.GetRoot().DescendantNodes().OfType<ClassDeclarationSyntax>().First();
                var className = classDeclarationSyntax.Identifier.ToString();
                var generatedclassName = $"{className}Controller";
                var splitClass = classDeclarationSyntax.ToString().Split(new[] { '{' }, 2);

                sourceBuilder.Append($@"
namespace GeneratedControllers
{{
[ApiController]
public class {generatedclassName} : ControllerBase
{{
");
                sourceBuilder.AppendLine(splitClass[1].Replace(className, generatedclassName));
                sourceBuilder.AppendLine("}");
                context.AddSource("ControllerGenerator", SourceText.From(sourceBuilder.ToString(), Encoding.UTF8));
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
}