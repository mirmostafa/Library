using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Library.Helpers.Models;

public class EqualityAssertion
{
    private readonly object _actual;

    public EqualityAssertion(object actual) 
        => this._actual = actual;

    public void To(object expected)
        => Assert.AreEqual(expected, this._actual);
}