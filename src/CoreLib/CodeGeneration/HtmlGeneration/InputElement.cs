namespace Library.CodeGeneration.HtmlGeneration;

public class InputElement : HtmlElement<InputElement>
{
    public InputElement(string type = "text") : base("input")
        => this.AttributeList.Add("type", type);
}