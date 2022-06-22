namespace Library.CodeGeneration.Models;

public interface INameSpace
{
    public string FullName { get; set; }
    public IList<ICodeGenType> CodeGenTypes { get; }
    public IList<string> UsingNameSpaces { get; }
}
