namespace PaymentGateway.Api.Domain.ValueObjects;

public class CVV
{
    public string Value { get; }

    public CVV(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || (value.Length != 3 && value.Length != 4) || !value.All(char.IsDigit))
        {
            throw new ArgumentException("CVV must be 3-4 numeric characters.");
        }

        Value = value;
    }
}