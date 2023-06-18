using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Library.Exceptions.Validations;
using Library.Globalization;

using Xunit;

namespace UnitTests;
public sealed class CommonHelperTest
{
    [Fact]
    public void Parse_ValidString_ReturnsParsedDateTime()
    {
        // Arrange
        string input = "1399/01/01";
        PersianDateTime expectedDateTime = new PersianDateTime(1399, 1, 1);

        // Act
        PersianDateTime result = input.Parse<PersianDateTime>();

        // Assert
        Assert.Equal(expectedDateTime, result);
    }

    [Fact]
    public void Parse_InvalidString_ThrowsFormatException()
    {
        // Arrange
        string input = "invalid_date_string";

        // Act & Assert
        Assert.Throws<ValidationException>(() => input.Parse<PersianDateTime>());
    }
}
