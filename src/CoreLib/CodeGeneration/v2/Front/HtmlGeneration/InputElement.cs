namespace Library.CodeGeneration.v2.Front.HtmlGeneration;

public sealed class InputElement : HtmlElement<InputElement>
{
    public InputElement(string type = "text") : base("input")
        => this.AttributeList.Add("type", type);
}