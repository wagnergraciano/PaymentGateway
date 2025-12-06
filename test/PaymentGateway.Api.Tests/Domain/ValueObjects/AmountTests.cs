using PaymentGateway.Api.Domain.ValueObjects;

namespace PaymentGateway.Api.Tests.Domain.ValueObjects;

public class AmountTests
{
    [Theory]
    [InlineData(1)] // Minimum valid amount
    [InlineData(1000)] // Arbitrary valid amount
    public void Constructor_ShouldCreateAmount_WhenValid(int validAmount)
    {
        var amount = new Amount(validAmount);
        Assert.Equal(validAmount, amount.Value);
    }

    [Theory]
    [InlineData(0)] // Zero amount
    [InlineData(-100)] // Negative amount
    public void Constructor_ShouldThrowArgumentException_WhenInvalid(int invalidAmount)
    {
        Assert.Throws<ArgumentException>(() => new Amount(invalidAmount));
    }
}