using System.Diagnostics.CodeAnalysis;

internal record PersonRecord(string Name, int Age);

internal class PersonClass
{
    public PersonClass()
    {
    }

    [SetsRequiredMembers]
    public PersonClass(string name) 
        => this.Name = name;

    public required string? Name { get; set; }
}