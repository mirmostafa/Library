namespace Library.CodeGeneration.HtmlGeneration;

public sealed class DivElement : HtmlElement<DivElement>, IParentHtmlElement
{
    public DivElement()
        : base("div")
    {
    }
}