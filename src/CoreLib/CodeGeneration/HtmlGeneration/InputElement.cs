namespace Library.CodeGeneration.HtmlGeneration;

public class InputElement : HtmlElement<InputElement>
{
    public InputElement(string type) : base("input")
        => this.AttributeList.Add("type", type);
}