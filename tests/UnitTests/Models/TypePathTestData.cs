namespace UnitTests.Models;

public class TypePathTestData
{
    public static IEnumerable<object[]> ParseData
    {
        get
        {
            //// Straight forward
            //yield return new object[]
            //{
            //    "Person", new TypeData("Person",null, [], false)
            //};
            //// Simple namespace
            //yield return new object[]
            //{
            //    typeof(int).FullName!, new TypeData(nameof(Int32)!, typeof(int).Namespace!, [], false)
            //};
            //// Complex namespace
            //yield return new object[]
            //{
            //    "Sample.Data.Entities.Person", new TypeData("Person","Sample.Data.Entities", [], false)
            //};
            //// Straight forward nullable
            //yield return new object[]
            //{
            //    "Person?", new TypeData("Person",null, [], true)
            //};
            //// Simple namespace nullable
            //yield return new object[]
            //{
            //    typeof(int?).FullName!, new TypeData(nameof(Int32)!, typeof(int).Namespace!, [], true)
            //};
            //// Complex namespace nullable
            //yield return new object[]
            //{
            //    "Sample.Data.Entities.Person?", new TypeData("Person","Sample.Data.Entities", [], true)
            //};

            //// Simple generic
            //yield return new object[]
            //{
            //    "ValueType<Person>", new TypeData("ValueType", null, [new("Person", null, [], false)], false)
            //};
            //// Simple generic nullable 1
            //yield return new object[]
            //{
            //    "ValueType<Person>?", new TypeData("ValueType", null, [new("Person", null, [], false)], true)
            //};
            //// Simple generic nullable 2
            //yield return new object[]
            //{
            //    "ValueType<Person?>", new TypeData("ValueType", null, [new("Person", null, [], true)], false)
            //};

            //// Complex generic - real world
            //yield return new object[]
            //{
            //    typeof(Task<int>).FullName!, new TypeData(nameof(Task), typeof(Task<int>).Namespace, [new(nameof(Int32), typeof(int).Namespace, [], false)], false)
            //};

            //// Complex generic nullable - real world
            //yield return new object[]
            //{
            //    typeof(Task<int?>).FullName!, new TypeData(nameof(Task), typeof(Task<int>).Namespace, [new(nameof(Int32), typeof(int).Namespace, [], true)], false)
            //};

            //// More complex generic - real world
            //yield return new object[]
            //{
            //    typeof(Task<IEnumerable<int>>).FullName!, new TypeData(nameof(Task), typeof(Task<IEnumerable<int>>).Namespace, [new(nameof(IEnumerable<int>), typeof(IEnumerable<int>).Namespace, [new(nameof(Int32), typeof(int).Namespace, [], false)], false)], false)
            //};

            // More complex null generic - real world
            yield return new object[]
            {
                typeof(Task<IEnumerable<int?>>).FullName!, new TypeData(nameof(Task),typeof(Task).Namespace,[new(nameof(IEnumerable<int>), typeof(IEnumerable<>).Namespace, [new(nameof(Int64), typeof(int).Namespace, [],true)],false)], false)
            };
        }
    }
}