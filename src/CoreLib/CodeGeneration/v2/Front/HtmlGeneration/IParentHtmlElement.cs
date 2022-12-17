using Library.Interfaces;

namespace Library.CodeGeneration.v2.Front.HtmlGeneration;

public interface IParentHtmlElement : IHtmlElementInfo, IParent<IHtmlElementInfo>
{
    static new IParentHtmlElement New(string name)
        => new HtmlElement(name);
}