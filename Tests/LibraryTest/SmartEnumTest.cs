using Library;

namespace LibraryTest;

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
    public void ByIdTest()
    {
        var male1 = GenderSmartEnum.Male;
        var male2 = GenderSmartEnum.ById(male1.Id);
        Assert.IsTrue(male1 == male2);
    }

    [TestMethod]
    public void ByNameTest1()
    {
        var male1 = GenderSmartEnum.Male;
        var fmale = GenderSmartEnum.Female;
        var male2 = GenderSmartEnum.ByName(male1.FriendlyName).Single();
        Assert.IsTrue(male1 == male2);
    }

    [TestMethod]
    public void ByNameTest2()
    {
        var male1 = GenderSmartEnum.Male;
        var fmale1 = GenderSmartEnum.Female;
        var fmale2 = GenderSmartEnum.ByName(fmale1.FriendlyName).Single();
        Assert.IsFalse(male1 == fmale2);
    }
}

internal class GenderSmartEnum : SmartEnum<GenderSmartEnum>
{
    public static readonly GenderSmartEnum None = new(0, "Unknown");
    public static readonly GenderSmartEnum Male = new(1, "Male");
    public static readonly GenderSmartEnum Female = new(2, "Female");
    public GenderSmartEnum(int id, string? friendlyName)
        : base(id, friendlyName)
    {
    }
}