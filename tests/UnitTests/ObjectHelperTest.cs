using System;
using System.Collections;

using Xunit;

namespace UnitTests;

[Trait("Category", "Helpers")]
public sealed class ObjectHelperTest
{
    public static TheoryData<(object?, object, Delegate), object> CheckDbNullData =>
        new()
        {
            { (DBNull.Value, 5, new Func<object, object>(x => Convert.ToInt32(x))), 5 },
            { (null, 5, new Func<object, object>(x => Convert.ToInt32(x))), 5 },
            { ("John", "Smith", new Func<object, object?>(x => x)), "John" },
            { ("John", 7, new Func<object, object?>(Convert.ToString)), "John" },
        };

    public static TheoryData<(object?, Type), bool> IsInheritedOrImplementedData =>
        new()
        {
            {(5, typeof(int)), true},
            {(5, typeof(string)), false },
            {("John", typeof(string)), true },
            {("John", typeof(int)), false },
            {("", typeof(int)), false },
            {("", typeof(string)), true },
            {(null, typeof(string)), false },
            {(null, typeof(int)), false },
            {(new Student(), typeof(Person)), true},
            {(new Person(), typeof(Student)), false },
            {(new Person(), typeof(IStudent)), false },
            {(new Student(), typeof(IStudent)), true },
        };
    public static TheoryData<int, bool> IsNullFamilyData2 =>
        new()
        {
            {5, false },
            {0, true },
            {default, true},
        };
    public static TheoryData<Guid, bool> IsNullFamilyData3 =>
        new()
        {
            {Guid.NewGuid(), false },
            {Guid.Empty, true },
            {default, true},
        };

    [Theory]
    [MemberData(nameof(CheckDbNullData))]
    public void CheckDbNullTest((object? o, object defaultValue, Delegate conveter) args, object expected)
    {
        // Assign

        // Act
        var actual = ObjectHelper.CheckDbNull(args.o, args.defaultValue, x => args.conveter.DynamicInvoke(new[] { x }));

        // Assert
        Assert.Equal(expected, actual);
    }

    [Fact]
    public void ExtraPropertyTest()
    {
        var testString = "Ali";
        testString.props().IsReadOnly = true;
        var actual = testString.props().IsReadOnly;
        Assert.True(actual);
    }

    [Theory]
    [MemberData(nameof(IsInheritedOrImplementedData))]
    public void IsInheritedOrImplementedTest((object? o, Type type) args, bool expected)
    {
        // Assign

        // Act
        var actual = ObjectHelper.IsInheritedOrImplemented(args.o, args.type);

        // Assert
        Assert.Equal(expected, actual);
    }

    [Theory]
    [InlineData(5, false)]
    [InlineData(0, false)]
    [InlineData("John", false)]
    [InlineData("", false)]
    [InlineData(null, true)]
    public void IsNullFamilyTest1(object arg, bool expected)
    {
        var actual = ObjectHelper.IsNull(arg);
        Assert.Equal(expected, actual);
    }
    [Theory]
    [InlineData(5, false)]
    [InlineData(0, false)]
    [InlineData(default, true)]
    public void IsNullFamilyTest2(int? arg, bool expected)
    {
        var actual = ObjectHelper.IsNull(arg);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(IsNullFamilyData3))]
    public void IsNullFamilyTest3(Guid arg, bool expected)
    {
        var actual = ObjectHelper.IsNullOrEmpty(arg);
        Assert.Equal(expected, actual);
    }

    [Theory]
    [MemberData(nameof(IsNullFamilyData3))]
    public void IsNullFamilyTest4(Guid? arg, bool expected)
    {
        var actual = ObjectHelper.IsNullOrEmpty(arg);
        Assert.Equal(expected, actual);
    }
    [Theory]
    [InlineData("John", 5)]
    [InlineData(3000, 2999)]
    public void RepeatTest(object item, int count)
    {
        var actual = ObjectHelper.Repeat(item, count).ToArray();
        for (var i = 0; i < count; i++)
        {
            Assert.Equal(item, actual[i]);
        }
    }
}

#region Models
file interface IA { }

file interface IB : IA { }

file interface IC { }

file interface ID { }

file interface IE { }

file interface IStudent { }
file record Person;
file record Student : Person, IStudent;

file class CA : IB, ID { }
file class CB : IE, ID { }

#endregion
