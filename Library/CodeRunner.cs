namespace Library
{
    public class CodeRunner
    {
        private readonly List<Action> _Statements = new List<Action>();

        public static CodeRunner StartWith(Action statement)
        {
            var result = new CodeRunner();
            result._Statements.Add(statement);
            return result;
        }

        public CodeRunner Then(Action statement) => this.Fluent(() => this._Statements.Add(statement));

        public CodeRunner Run() => ForEach(this, _Statements, statement => statement?.Invoke());
    }
}
