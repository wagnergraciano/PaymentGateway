namespace PaymentGateway.Api.Domain.ValueObjects;

public class Amount
{
    public int Value { get; }

    public Amount(int value)
    {
        if (value <= 0)
        {
            throw new ArgumentException("Amount must be a positive integer representing the minor currency unit.");
        }

        Value = value;
    }
}