using PaymentGateway.Api.Domain.ValueObjects;

namespace PaymentGateway.Api.Tests.Domain.ValueObjects;

public class CurrencyTests
{
    [Theory]
    [InlineData("USD")] // Valid currency
    [InlineData("EUR")] // Valid currency
    [InlineData("GBP")] // Valid currency
    public void Constructor_ShouldCreateCurrency_WhenValid(string validCurrency)
    {
        var currency = new Currency(validCurrency);
        Assert.Equal(validCurrency, currency.Value);
    }

    [Theory]
    [InlineData("US")] // Less than 3 characters
    [InlineData("USDE")] // More than 3 characters
    [InlineData("ABC")] // Unsupported currency
    public void Constructor_ShouldThrowArgumentException_WhenInvalid(string invalidCurrency)
    {
        Assert.Throws<ArgumentException>(() => new Currency(invalidCurrency));
    }
}