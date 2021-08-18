using Library.Helpers;

namespace LibraryTest
{
    [TestClass]
    public class StringHelperTest
    {
        [TestMethod]
        public void IsNullOrEmptyTest()
        {
            var emptyStr = string.Empty;
            var sampleStr = "Sample String";

            var emptyStrIsEmpty = emptyStr.IsNullOrEmpty();
            var sampleStrIsEmpty = sampleStr.IsNullOrEmpty();

            Assert.IsTrue(emptyStrIsEmpty);
            Assert.IsFalse(sampleStrIsEmpty);
        }

        [TestMethod]
        public void IsNumberTest()
        {
            var numStr = "123456789";
            var numStrIsNumbber = numStr.IsNumber();

            var invNumStr = "12345678..9";
            var invNumStrIsNumber = invNumStr.IsNumber();

            Assert.IsTrue(numStrIsNumbber);
            Assert.IsFalse(invNumStrIsNumber);
        }

        [TestMethod]
        public void SliceTest()
        {
            var sliceStr = "Mohammad Mirmostafa is a C# Developer.";

            var mohammad = sliceStr.Slice(0, 8);
            Assert.AreEqual(mohammad, "Mohammad");

            var cShartDeveloper = sliceStr.Slice(25);
            Assert.AreEqual(cShartDeveloper, "C# Developer.");
        }
    }
}
