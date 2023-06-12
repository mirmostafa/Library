using System.CodeDom;

namespace Library.CodeGeneration.Models;

public interface ICodeGenType : ISupportPartial
{
    MemberAttributes? AccessModifier { get; set; }
    IList<string> BaseTypes { get; }
    InheritanceKind InheritanceKind { get; set; }
    IList<IMemberInfo> Members { get; }
    string Name { get; set; }
    string NameSpace { get; set; }
    IList<string> UsingNameSpaces { get; }
}