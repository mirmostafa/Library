using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.Models;

[Fluent]
public record class BlazorCode : Code
{
    public List<string> Usings => ExtraProperties.Usings ??= new List<string>();
    public List<TypePath> Injects => ExtraProperties.Injects ??= new List<TypePath>();
    public TypePath? Inherit { get => ExtraProperties.Inherit; set => ExtraProperties.Inherit = value; }

    public BlazorCode SetInherite(TypePath? inherit)
        => this.Fluent(x => x.Inherit = inherit);
    public BlazorCode AddUsingNameSpace(string nameSpace)
        => this.Fluent(() => this.Usings.Add(nameSpace));
    public BlazorCode AddUsingNameSpace(IEnumerable<string> nameSpaces)
        => this.Fluent(() => this.Usings.AddRange(nameSpaces));
    public BlazorCode AddInjection(TypePath inject)
        => this.Fluent(() => this.Injects.Add(inject));
    public BlazorCode AddInjection(IEnumerable<TypePath> injects)
        => this.Fluent(() => this.Injects.AddRange(injects));

    public BlazorCode(in (string Name, string Statement, bool IsPartial) data)
        : base((data.Name, Languages.Blazor, data.Statement, data.IsPartial))
    {
    }

    public BlazorCode(in string name, in string statement, in bool isPartial = false)
        : base(name, Languages.Blazor, statement, isPartial)
    {
    }

    protected BlazorCode(BlazorCode original)
        : base(original)
        => this.AddUsingNameSpace(original.Usings).AddInjection(original.Injects).SetInherite(original.Inherit);
}