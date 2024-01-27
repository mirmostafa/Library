using Library.CodeGeneration.v2;
using Library.CodeGeneration.v2.Back;

Initialize();

INamespace.New("GSTech.Dtos")
    .AddType(IClass.New("PersonDto")
        .AddMember(IField.New("_age", typeof(int), AccessModifier.Private))
        .AddMember(IProperty.New("Name", typeof(string)))
        .AddMember(IProperty.New("Age", typeof(int), "_age"))
        .AddMember(IMethod.New("CalcBirthYear", "return DateTime.Now.Year - this.Age;", null, typeof(int)))
        )
    .GenerateCode<RoslynCodeGenerator>()
    .WriteLine();