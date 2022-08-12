using Library.Interfaces;

namespace Library.CodeGeneration.V2.HtmlGeneration;

public interface ISelfCoder
{
    string GenerateCodeStatement();
}

public interface IHtmlElementInfo
{
    IEnumerable<(string Key, string? Value)> Attributes { get; }
    ClosingTagType ClosingTagType { get; }
    string? InnerHtml { get; set; }
    string Name { get; }

    static IHtmlElementInfo New(string name)
        => new HtmlElement(name);
}

public interface IParentHtmlElement : IHtmlElementInfo, IHasChild<IHtmlElementInfo>
{
    static new IParentHtmlElement New(string name)
        => new HtmlElement(name);
}

public enum ClosingTagType
{
    Full,
    Slash,
    None
}