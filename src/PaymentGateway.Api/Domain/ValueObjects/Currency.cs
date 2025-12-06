namespace PaymentGateway.Api.Domain.ValueObjects;

public class Currency
{
    public string Value { get; }

    private static readonly string[] SupportedCurrencies = { "USD", "EUR", "GBP" };

    public Currency(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length != 3 || !SupportedCurrencies.Contains(value))
        {
            throw new ArgumentException("Currency must be a valid 3-character ISO code (USD, EUR, GBP).");
        }

        Value = value;
    }
}