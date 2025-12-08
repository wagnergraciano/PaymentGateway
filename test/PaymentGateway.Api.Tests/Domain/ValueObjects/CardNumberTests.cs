using PaymentGateway.Api.Domain.ValueObjects;
namespace PaymentGateway.Api.Tests.Domain.ValueObjects;

public class CardNumberTests
{
    [Theory]
    [InlineData(12345678901234)] // 14 digits
    [InlineData(1234567890123456789)] // 19 digits
    public void Constructor_ShouldCreateCardNumber_WhenValidLong(long validCardNumber)
    {
        var cardNumber = new CardNumber(validCardNumber);
        Assert.Equal(validCardNumber, cardNumber.Value);
    }

    [Theory]
    [InlineData("123")] // Less than 14 digits
    [InlineData("12345678901234567890")] // More than 19 digits
    public void Constructor_ShouldThrowArgumentException_WhenInvalidLong(string invalidCardNumber)
    {
        Assert.Throws<ArgumentException>(() => new CardNumber(invalidCardNumber));
    }

    [Theory]
    [InlineData("12345678901234")] // 14 digits
    [InlineData("1234567890123456789")] // 19 digits
    public void Constructor_ShouldCreateCardNumber_WhenValidString(string validCardNumber)
    {
        var cardNumber = new CardNumber(validCardNumber);
        Assert.Equal(long.Parse(validCardNumber), cardNumber.Value);
    }

    [Theory]
    [InlineData("123")] // Less than 14 digits
    [InlineData("12345678901234567890")] // More than 19 digits
    [InlineData("1234abcd901234")] // Contains non-numeric characters
    public void Constructor_ShouldThrowArgumentException_WhenInvalidString(string invalidCardNumber)
    {
        Assert.Throws<ArgumentException>(() => new CardNumber(invalidCardNumber));
    }

    [Theory]
    [InlineData("12345678901234", "1234")] // 14 digits
    [InlineData("1234567890123456789", "6789")] // 19 digits
    public void GetMaskedNumber_ShouldReturnLastFourDigits(string cardNumber, string expectedMasked)
    {
        // Arrange
        var card = new CardNumber(cardNumber);

        // Act
        var maskedNumber = card.GetMaskedNumber();

        // Assert
        Assert.Equal(expectedMasked, maskedNumber);
    }
}