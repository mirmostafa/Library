using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

public record class TestClass(string? Name, int? Age) : ITestClass<TestClass>
{
    public static TestClass New(string? name, int? age) =>
        new(name, age);
    public void MyMethod() =>
        ITestClass<TestClass>.New();
}

public static class Unit
{
    public static void MyTest<TTestClass>()
        where TTestClass : ITestClass<TTestClass> =>
        ITestClass<TTestClass>.New("Ali");
}