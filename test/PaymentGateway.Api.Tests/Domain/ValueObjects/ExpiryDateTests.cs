using PaymentGateway.Api.Domain.ValueObjects;

namespace PaymentGateway.Api.Tests.Domain.ValueObjects;

public class ExpiryDateTests
{
    [Theory]
    [InlineData(12, 2030)] // Valid future date
    [InlineData(1, 2026)] // Valid future date
    public void Constructor_ShouldCreateExpiryDate_WhenValid(int month, int year)
    {
        var expiryDate = new ExpiryDate(month, year);
        Assert.Equal(month, expiryDate.Month);
        Assert.Equal(year, expiryDate.Year);
    }

    [Theory]
    [InlineData(0, 2030)] // Invalid month (less than 1)
    [InlineData(13, 2030)] // Invalid month (greater than 12)
    [InlineData(12, 2020)] // Expired year
    [InlineData(1, 2025)] // Expired month and year
    public void Constructor_ShouldThrowArgumentException_WhenInvalid(int month, int year)
    {
        Assert.Throws<ArgumentException>(() => new ExpiryDate(month, year));
    }
}