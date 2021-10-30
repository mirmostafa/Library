using Library.Exceptions;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Library.Helpers;

public static class UnitTestHelper
{
    public static void IsFamiliarException(this Assert assert, Exception exception)
        => Assert.IsInstanceOfType(exception, typeof(ExceptionBase));
    
    public static void ItsOk(this Assert assert) { }

    public static EqualityAssertion Equal(this Assert assert, object actual) 
        => new(actual);

    //internal static void HandleFail(string assertionName, string message)
    //    => throw new AssertFailedException($"Assertion {assertionName} failed with message: {message}");
}

public class EqualityAssertion
{
    private readonly object actual;

    public EqualityAssertion(object actual) => this.actual = actual;
    public void To(object expected)
        => Assert.AreEqual(expected, this.actual);
}