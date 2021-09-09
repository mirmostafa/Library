//using Library.Web;

//namespace Library.Coding;

//public class CodeRunner
//{
//    private readonly List<(Func<Result> Func, bool OnSucceed)> _Statements = new();

//    public static CodeRunner StartWith(Func<Result> statement)
//    {
//        var result = new CodeRunner();
//        result._Statements.Add(statement);
//        return result;
//    }

//    public CodeRunner OnSucceed(Func<Result> statement) => this.Fluent(() => this._Statements.Add(statement, true));

//    public CodeRunner Run()
//    {

//    }
//}
