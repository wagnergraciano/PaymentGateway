namespace PaymentGateway.Api.Domain.ValueObjects;

public class CardNumber
{
    public long Value { get; }

    public CardNumber(long value) : this(value.ToString()) { }

    public CardNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value) || value.Length < 14 || value.Length > 19 || !value.All(char.IsDigit))
        {
            throw new ArgumentException("Card number must be between 14-19 numeric characters.");
        }

        Value = long.Parse(value);
    }
}