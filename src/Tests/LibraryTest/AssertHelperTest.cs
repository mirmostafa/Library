using Library.Exceptions.Validations;

namespace Library.UnitTest;

[TestClass]

public class AssertHelperTest
{
    [TestMethod]
    public void TestMethod1() => Assert.That.Equal(1).To(1);

    [TestMethod]
    public void TestMethod2() => Assert.That.IsFamiliarException(new ValidationException());
}
