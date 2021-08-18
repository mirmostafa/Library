namespace Library.Coding;
public class CodeRunner
{
    private readonly List<Action> _Statements = new();

    public static CodeRunner StartWith(Action statement)
    {
        var result = new CodeRunner();
        result._Statements.Add(statement);
        return result;
    }

    public CodeRunner Then(Action statement) => this.Fluent(() => this._Statements.Add(statement));

    public CodeRunner Run() => ForEach(this, this._Statements, statement => statement?.Invoke());
}
