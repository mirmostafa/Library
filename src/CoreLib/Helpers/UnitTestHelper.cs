using Library.Exceptions;
using Library.Helpers.Models;

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