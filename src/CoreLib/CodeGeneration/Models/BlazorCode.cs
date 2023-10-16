using Library.DesignPatterns.Markers;

namespace Library.CodeGeneration.Models;

[Fluent]
public sealed class BlazorCode(in string name, in string statement, in bool isPartial = false)
    : Code(name, Languages.BlazorCodeBehind, statement, isPartial)
{
    public BlazorCode(in (string Name, string Statement, bool IsPartial) data) : this(data.Name, data.Statement, data.IsPartial)
    {
    }

    public TypePath? Inherit { get => this.ExtraProperties.Inherit; set => this.ExtraProperties.Inherit = value; }
    public List<TypePath> Injects => this.ExtraProperties.Injects ??= new List<TypePath>();
    public List<string> Usings => this.ExtraProperties.Usings ??= new List<string>();

    public BlazorCode AddInjection(params TypePath[] injects) =>
        this.Fluent(() => this.Injects.AddRange(injects));

    public BlazorCode AddInjection(IEnumerable<TypePath> injects) =>
        this.Fluent(() => this.Injects.AddRange(injects));

    public BlazorCode AddUsingNameSpace(params string[] nameSpaces) =>
        this.Fluent(() => this.Usings.AddRange(nameSpaces));

    public BlazorCode AddUsingNameSpace(IEnumerable<string> nameSpaces) =>
        this.Fluent(() => this.Usings.AddRange(nameSpaces));

    public BlazorCode SetInherit(TypePath? inherit) =>
        this.Fluent(x => x.Inherit = inherit);
}