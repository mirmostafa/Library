using Library.Interfaces;

namespace Library.CodeGeneration.HtmlGeneration;

public interface IAutoCoder
{
    string GenerateCodeStatement();
}

public interface IHtmlElementInfo
{
    IEnumerable<(string Key, string? Value)> Attributes { get; }
    ClosingTagType ClosingTagType { get; }
    string Name { get; }
}

public interface IParentHtmlElement : IHtmlElementInfo, IHasChild<IHtmlElementInfo>
{ }

public enum ClosingTagType
{
    Full,
    Slash,
    None
}