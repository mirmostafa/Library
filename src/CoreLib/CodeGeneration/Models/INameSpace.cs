namespace Library.CodeGeneration.Models;

public interface INameSpace
{
    public IList<ICodeGenType> CodeGenTypes { get; }
    public string FullName { get; set; }
    public IList<string> UsingNameSpaces { get; }
}