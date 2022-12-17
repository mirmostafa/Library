namespace Library.CodeGeneration.v2.Front.HtmlGeneration;

public sealed class HtmlElement : HtmlElement<HtmlElement>
{
    public HtmlElement(string name) : base(name)
    {
    }

    public static HtmlElement New(string name)
        => new(name);
}