using Library.CodeGeneration;
using Library.CodeGeneration.v2;
using Library.CodeGeneration.v2.Back;
using Library.Helpers.ConsoleHelper;

Initialize();

INamespace.New("GSTech.Dtos")
    .AddType(IClass.New("PersonDto")
        .AddField("_age", typeof(int), AccessModifier.Private)
        .AddProperty("Name", TypePath.New(typeof(string).FullName!, isNullable: true))
        .AddProperty("Age", typeof(int), "_age")
        .AddMethod("CalcBirthYear", "return DateTime.Now.Year - this.Age;", null, typeof(int))
        )
    .GenerateCode<RoslynCodeGenerator>()
    .WriteLine();

List<int> list = new List<int>().AsFluent().Add(5).Add(2).Add(3);
DumpListLine(list);