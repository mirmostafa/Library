using Library.Validations;

namespace UnitTest;

[TestClass]
public class ValidationTest
{
    [TestMethod]
    public void BeNotNullTest()
    {
        var arg = "Test";

        //arg = arg.Should().BeNotNull();
    }
}