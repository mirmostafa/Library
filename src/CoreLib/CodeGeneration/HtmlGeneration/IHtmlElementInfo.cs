using Library.Interfaces;

namespace Library.CodeGeneration.HtmlGeneration;

public interface IHtmlElementInfo
{
    IEnumerable<(string Key, string? Value)> Attributes { get; }
    string Name { get; }
}

public interface IParentHtmlElement : IHasChild<IHtmlElementInfo>
{ }

public interface IAutoCoder
{
    string GenerateCode();
}