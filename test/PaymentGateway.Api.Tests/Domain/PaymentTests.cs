using PaymentGateway.Api.Domain;

namespace PaymentGateway.Api.Tests.Domain;

public class PaymentTests
{
    [Fact]
    public void Constructor_ShouldCreatePayment_WhenAllValuesAreValid()
    {
        // Arrange
        long cardNumber = 12345678901234;
        int expiryMonth = 12;
        int expiryYear = 2030;
        string currency = "USD";
        int amount = 1000;
        string cvv = "123";

        // Act
        Payment payment = new Payment(cardNumber, expiryMonth, expiryYear, currency, amount, cvv);

        // Assert
        Assert.NotNull(payment);
        Assert.Equal(cardNumber, payment.CardNumber.Value);
        Assert.Equal(expiryMonth, payment.ExpiryDate.Month);
        Assert.Equal(expiryYear, payment.ExpiryDate.Year);
        Assert.Equal(currency, payment.Currency.Value);
        Assert.Equal(amount, payment.Amount.Value);
        Assert.Equal(cvv, payment.Cvv.Value);
    }

    [Fact]
    public void Constructor_ShouldThrowArgumentException_WhenAnyValueIsInvalid()
    {
        // Arrange
        long cardNumber = 12345678901234;
        int expiryMonth = 12;
        int expiryYear = 2030;
        string currency = "USD";
        int amount = 1000;
        string invalidCVV = "12";

        // Act & Assert
        Assert.Throws<ArgumentException>(() => new Payment(cardNumber, expiryMonth, expiryYear, currency, amount, invalidCVV));
    }
}