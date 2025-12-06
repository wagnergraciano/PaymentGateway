namespace PaymentGateway.Api.Domain;

using PaymentGateway.Api.Domain.ValueObjects;

public class Payment
{
    public Guid Id { get; private set; }
    public CardNumber CardNumber { get; private set; }
    public ExpiryDate ExpiryDate { get; private set; }
    public Currency Currency { get; private set; }
    public Amount Amount { get; private set; }
    public CVV Cvv { get; private set; }

    public Payment(long cardNumber, int expiryMonth, int expiryYear, string currency, int amount, string cvv)
    {
        Id = Guid.NewGuid();
        CardNumber = new CardNumber(cardNumber);
        ExpiryDate = new ExpiryDate(expiryMonth, expiryYear);
        Currency = new Currency(currency);
        Amount = new Amount(amount);
        Cvv = new CVV(cvv);
    }
}