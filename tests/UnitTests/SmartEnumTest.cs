﻿using Library.Types;

namespace UnitTests;


[Trait("Category", "Code Helpers")]
public sealed class SmartEnumTest
{

    [Fact]
    public void EqualTest1()
    {
        var male1 = Gender.Male;
        var male2 = Gender.Male;
        Assert.Equal(male1, male2);
    }

    [Fact]
    public void EqualTest2()
    {
        var male1 = Gender.Male;
        var male2 = Gender.Male;
        Assert.True(male1 == male2);
    }

    [Fact]
    public void ByIdTest()
    {
        var male1 = Gender.Male;
        var male2 = Gender.ById(male1.Id);
        Assert.True(male1 == male2);
    }

    [Fact]
    public void ByNameTest1()
    {
        var male1 = Gender.Male;
        var fmale = Gender.Female;
        var male2 = Gender.ByName(male1.FriendlyName).Single();
        Assert.True(male1 == male2);
    }

    [Fact]
    public void ByNameTest2()
    {
        var male1 = Gender.Male;
        var fmale1 = Gender.Female;
        var fmale2 = Gender.ByName(fmale1.FriendlyName).Single();
        Assert.False(male1 == fmale2);
    }
}

internal class Gender : SmartEnum<Gender>
{
    public static readonly Gender None = new(0, "Unknown");
    public static readonly Gender Male = new(1, "Male");
    public static readonly Gender Female = new(2, "Female");
    private Gender(int id, string? friendlyName)
        : base(id, friendlyName)
    {
    }
}