﻿using Library.Exceptions.Validations;
using Library.Validations;

namespace LibraryTest;

[TestClass]
public class CheckValidationTest
{
    [TestMethod]
    public void MinMaxTest()
    {
        var (min, arg) = (0, 5);
        Check.IfArgumentBiggerThan(arg, min);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public void MinMaxTest1()
    {
        var (arg, min) = (0, 5);
        Check.IfArgumentBiggerThan(arg, min);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentNullException))]
    public void ArgumentNullTest()
    {
        string? lname = null;
        Check.IfArgumentNotNull(lname);
    }

    [TestMethod]
    public void IfIs1()
    {
        var lname = "Mirmostafa";
        Check.IfIs<string>(lname);
    }

    [TestMethod]
    [ExpectedException(typeof(TypeMismatchValidationException))]
    public void IfIs2()
    {
        var lname = "Mirmostafa";
        Check.IfIs<int>(lname);
    }

    [TestMethod]
    public void NotValidTest1()
    {
        var lname = "Mirmostafa";
        Check.NotValid(lname, x => x is null, () => new NullValueValidationException());
    }

    [TestMethod]
    [ExpectedException(typeof(NullValueValidationException))]
    public void NotValidTest2()
    {
        string? lname = null;
        Check.NotValid(lname, x => x is null, () => new NullValueValidationException());
    }

    [TestMethod]
    public void NotNullTest1()
    {
        var lname = "Mirmostafa";
        Check.NotNull(lname, () => new NullValueValidationException());
    }

    [TestMethod]
    [ExpectedException(typeof(NullValueValidationException))]
    public void NotNullTest2()
    {
        string? lname = null;
        Check.NotNull(lname, () => new NullValueValidationException());
    }
}