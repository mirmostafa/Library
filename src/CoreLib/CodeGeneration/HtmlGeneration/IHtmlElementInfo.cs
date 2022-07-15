using Library.Interfaces;

namespace Library.CodeGeneration.HtmlGeneration;

public interface IHtmlElementInfo : IParentHtmlElement
{
    IEnumerable<(string Key, string? Value)> Attributes { get; }
    string Name { get; }

    ClosingTagType ClosingTagType { get; }
}

public interface IParentHtmlElement : IHasChild<IHtmlElementInfo>
{ }

public interface IAutoCoder
{
    string GenerateCode();
}

public enum ClosingTagType
{
    Full,
    Slash,
    None
}