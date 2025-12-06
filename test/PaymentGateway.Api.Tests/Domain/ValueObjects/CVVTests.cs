using PaymentGateway.Api.Domain.ValueObjects;

namespace PaymentGateway.Api.Tests.Domain.ValueObjects;

public class CVVTests
{
    [Theory]
    [InlineData("123")] // Valid 3 digits
    [InlineData("1234")] // Valid 4 digits
    public void Constructor_ShouldCreateCVV_WhenValid(string validCVV)
    {
        var cvv = new CVV(validCVV);
        Assert.Equal(validCVV, cvv.Value);
    }

    [Theory]
    [InlineData("12")] // Less than 3 digits
    [InlineData("12345")] // More than 4 digits
    [InlineData("12a4")] // Contains non-numeric characters
    public void Constructor_ShouldThrowArgumentException_WhenInvalid(string invalidCVV)
    {
        Assert.Throws<ArgumentException>(() => new CVV(invalidCVV));
    }
}