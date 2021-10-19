using Library.Exceptions.Validations;
namespace LibraryTest;

[TestClass]

public class AssertHelperTest
{
    [TestMethod]
    public void TestMethod1() => Assert.That.Equal(1).With(1);

    [TestMethod]
    public void TestMethod2() => Assert.That.IsFamiliarException(new ValidationException());
}
