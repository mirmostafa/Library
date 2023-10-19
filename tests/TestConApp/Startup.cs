using Library.CodeGeneration;

using Microsoft.CodeAnalysis.CSharp;

using TestConApp;

internal class Program
{
    private static void Main(string[] args)
    {
        Initialize();

        var setAgeMethodBody = @"
this._year = year;
return this;
";
        var ns = RoslynHelper.CreateNamespace("Test.Dtos");
        var personType = new TypePath("PersonDto");
        var personDto = RoslynHelper.CreateType(personType)
            .AddField(new("_lastName", TypePath.New<int>()), out var lastNameField)
            .AddConstructor()
            .AddProperty<string>("Name")
            .AddPropertyWithBackingField(new("LastName", TypePath.New<string>()), lastNameField)
            .AddPropertyWithBackingField(new("Age", TypePath.New<int>(), setAccessor: (false, null)))
            .AddProperty(new PropertyInfo("StaticProperty", TypePath.New<int>(), modifiers: new[] { SyntaxKind.PublicKeyword, SyntaxKind.StaticKeyword }))
            .AddMethod(new MethodInfo(null, personType, "SetAge", new[] { (TypePath.New<int>(), "age") }, setAgeMethodBody))
            ;

        ns = ns.AddType(personDto);

        WriteLine(ns.GenerateCode());
    }
}