namespace PaymentGateway.Api.Domain.ValueObjects;

public class ExpiryDate
{
    public int Month { get; }
    public int Year { get; }

    public ExpiryDate(int month, int year)
    {
        if (!this.IsValidMonth(month))
        {
            throw new ArgumentException("Expiry month must be between 1 and 12.");
        }

        if (this.IsExpired(month, year))
        {
            throw new ArgumentException("Expiry year and month must be in the future.");
        }

        Month = month;
        Year = year;
    }

    private bool IsValidMonth(int month)
    {
        return month >= 1 && month <= 12;
    }

    private bool IsExpired(int month, int year)
    {
        var currentDate = DateTime.UtcNow;
        return year < currentDate.Year || (year == currentDate.Year && month < currentDate.Month);
    }
}