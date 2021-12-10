using Library;

namespace LibraryTest;

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2252:This API requires opting into preview features", Justification = "<Pending>")]
[TestClass]
public class SmartEnumTest
{

    [TestMethod]
    public void EqualTest1()
    {
        var male1 = GenderSmartEnum.Male;
        var male2 = GenderSmartEnum.Male;
        Assert.AreEqual(male1, male2);
    }

    [TestMethod]
    public void EqualTest2()
    {
        var male1 = GenderSmartEnum.Male;
        var male2 = GenderSmartEnum.Male;
        Assert.IsTrue(male1 == male2);
    }

    [TestMethod]
    public void EqualTest3()
    {
        var male1 = GenderSmartEnum.Male;
        var male2 = GenderSmartEnum.Male;
        Assert.IsFalse(male1 == male2);
    }

    [TestMethod]
    public void EqualTest4()
    {
        var male1 = GenderSmartEnum.Male;
        var male2 = GenderSmartEnum.ById(male1.Id);
        Assert.IsFalse(male1 == male2);
    }
}

[System.Diagnostics.CodeAnalysis.SuppressMessage("Usage", "CA2252:This API requires opting into preview features", Justification = "<Pending>")]
internal class GenderSmartEnum : SmartEnum<GenderSmartEnum>
{
    public static readonly GenderSmartEnum None = new(0, "Unknown");
    public static readonly GenderSmartEnum Male = new(1, "Male");
    public static readonly GenderSmartEnum Female = new(2, "Femail");
    public GenderSmartEnum(int id, string? friendlyName)
        : base(id, friendlyName)
    {
    }
}