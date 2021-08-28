using System;
using Microsoft.CodeAnalysis;

namespace Library.SourceGenerator
{
    public abstract class AttibuteBasedGeneratorBase : ISourceGenerator
    {
        public TypePath Attribute { get; }
        public string AttributeCode { get; }

        protected AttibuteBasedGeneratorBase(string attribute, string attributeCode)
        {
            this.Attribute = attribute ?? throw new ArgumentNullException(nameof(attribute));
            this.AttributeCode = attributeCode ?? throw new ArgumentNullException(nameof(attributeCode));
        }
        public void Execute(GeneratorExecutionContext context)
        {
            // retrieve the populated receiver 
            if (context.SyntaxContextReceiver is SyntaxReceiver receiver)
            {
                this.OnExecuting(context, receiver);
            }
        }

        protected abstract void OnExecuting(GeneratorExecutionContext context, SyntaxReceiver receiver);

        public void Initialize(GeneratorInitializationContext context)
        {
#if DEBUG1
        if (!Debugger.IsAttached)
        {
            Debugger.Launch();
        }
#endif
            // Register the attribute source
            context.RegisterForPostInitialization((i) => i.AddSource(this.Attribute.Name!, this.AttributeCode));

            // Register a syntax receiver that will be created for each generation pass
            context.RegisterForSyntaxNotifications(() => new SyntaxReceiver(this.Attribute!));

            this.OnInitializing(context);
        }

        protected virtual void OnInitializing(GeneratorInitializationContext context) { }
    }
}
