namespace UnitTests.Models;

public class TypePathTestData
{
    public static IEnumerable<object[]> ParseData
    {
        get
        {
            // Straight forward
            yield return new object[]
            {
                "Person", new TypeData("Person",null, [], false)
            };
            // Simple namespace
            yield return new object[]
            {
                typeof(int).FullName!, new TypeData(nameof(Int32)!, typeof(int).Namespace!, [], false)
            };
            // Complex namespace
            yield return new object[]
            {
                "Sample.Data.Entities.Person", new TypeData("Person","Sample.Data.Entities", [], false)
            };
            // Straight forward nullable
            yield return new object[]
            {
                "Person?", new TypeData("Person",null, [], true)
            };
            // Simple namespace nullable
            yield return new object[]
            {
                typeof(int?).FullName!, new TypeData(nameof(Int32)!, typeof(int).Namespace!, [], true)
            };
            // Complex namespace nullable
            yield return new object[]
            {
                "Sample.Data.Entities.Person?", new TypeData("Person","Sample.Data.Entities", [], true)
            };
        }
    }
}