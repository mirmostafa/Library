using System.CodeDom;

namespace Library.CodeGeneration.Models;

public interface ICodeGenType : ISupportPartial
{
    string Name { get; set; }
    string NameSpace { get; set; }
    IList<string> UsingNameSpaces { get; }
    IList<string> BaseTypes { get; }
    IList<IMemberInfo> Members { get; }
    MemberAttributes? AccessModifier { get; set; }
    InhertaceKind InhertaceKind { get; set; }
}
