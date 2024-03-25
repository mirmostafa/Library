using System.Diagnostics.CodeAnalysis;

namespace TestConApp;

public interface ITestClass<out TTestClass>
    where TTestClass : ITestClass<TTestClass>
{
    static abstract TTestClass New(string? name, int? age);

    static TTestClass New(string? name) =>
        TTestClass.New(name, default);

    static TTestClass New(int? age) =>
        TTestClass.New(default, age);

    static TTestClass New() =>
        TTestClass.New(default, default);
}

internal record class TestClass(string? Name, int? Age) : ITestClass<TestClass>
{
    public static TestClass New(string? name, int? age) =>
        new(name, age);
    public void MyMethod() =>
        ITestClass<TestClass>.New();
}

public static class Factory
{
    [return: NotNull]
    public static TTestClass Create<TTestClass>()
        where TTestClass : ITestClass<TTestClass> =>
        ITestClass<TTestClass>.New("Ali");

    public static void Test()
    {
        var a = Create<TestClass>();
        WriteLine(a);
    }
}