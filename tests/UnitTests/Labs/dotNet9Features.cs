namespace UnitTests.Labs;

[Trait("Category", "What's new in .NET 9.0")]
public class DotNet9Features
{
    [Fact]
    public void EnhancedParams()
    {
        // Arrange
        var s = "This is Mohammad";
        var mohammad = "Mohammad";
        var ali = "ali";

        // Act
        var found = s.IndexOfAny(out var item, mohammad, ali);

        // Assert
        Assert.True(found > -1);
        Assert.Equal(item, mohammad);
    }
}

//class Person;

//public implicit extension PersonExtension for Person
//{
//    public bool IsLead
//        => this.Organization
//            .Teams
//            .Any(team => team.Lead == this);
//}

//implicit extension StringExtensions for string
//{
//    public bool IsNotNullOrEmpty() 
//        => !string.IsNullOrEmpty(this);
//}