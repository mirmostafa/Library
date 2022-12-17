namespace Library.CodeGeneration.v2.Front.HtmlGeneration;

public interface IHtmlElementInfo
{
    IEnumerable<(string Key, string? Value)> Attributes { get; }

    ClosingTagType ClosingTagType { get; }

    string? InnerHtml { get; set; }

    string Name { get; }

    static IHtmlElementInfo New(string name)
        => new HtmlElement(name);
}