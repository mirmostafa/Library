using Library.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Library.Helpers;

public static class UnitTestHelper
{
    public static void IsFamiliarException(this Assert assert, Exception exception)
        => Assert.IsInstanceOfType(exception, typeof(ExceptionBase));

    public static void ItsOk(this Assert assert)
    { }

    public static EqualityAssertion Equal(this Assert assert, object actual)
        => new(actual);

    //internal static void HandleFail(string assertionName, string message)
    //    => throw new AssertFailedException($"Assertion {assertionName} failed with message: {message}");

    public static void AreEqual<T>(this Assert assert, IEnumerable<T> items1, IEnumerable<T> items2)
    {
        if (items1 == null && items2 == null)
        {
            Assert.IsTrue(true);
            return;
        }

        if (items1 == null || items2 == null)
        {
            Assert.IsTrue(false);
            return;
        }

        if (items1.Count() != items2.Count())
        {
            Assert.IsTrue(false);
            return;
        }

        var ok = !items1.Except(items2).Any();
        Assert.IsTrue(ok);
    }
}

public class EqualityAssertion
{
    private readonly object actual;

    public EqualityAssertion(object actual) => this.actual = actual;

    public void To(object expected)
        => Assert.AreEqual(expected, this.actual);
}