namespace ConAppTest;

internal class Person
{
    public int Age { get; set; }

    public string? FirstName { get; set; }

    public required string LastName { get; set; }
}

internal class Student : Person
{
}