//using Library.Coding;

//namespace LibraryTest;

//[TestClass]
//public class IfStatementTest
//{
//    [TestMethod]
//    public void IfStatementTestMethod1() => true.If().Then(Methods.Empty).Else(Methods.Empty).Build();

//    [TestMethod]
//    public void IfStatementTestMethod2()
//    {
//        var actual = true.If<int>().Then<int>(() => 1).Else<int>(() => 2).Build();
//        Assert.AreEqual(1, actual);
//    }

//    [TestMethod]
//    public void IfStatementTestMethod3()
//    {
//        var actual = false.If<int>().Then<int>(() => 1).Else<int>(() => 2).Build();
//        Assert.AreEqual(2, actual);
//    }
//}
