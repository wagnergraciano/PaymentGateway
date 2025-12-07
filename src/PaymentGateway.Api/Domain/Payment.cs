namespace PaymentGateway.Api.Domain;

using PaymentGateway.Api.Domain.ValueObjects;
using PaymentGateway.Api.Models;

public class Payment
{
    public Guid Id { get; }
    public CardNumber CardNumber { get; }
    public ExpiryDate ExpiryDate { get; }
    public Currency Currency { get; }
    public Amount Amount { get; }
    public CVV Cvv { get; }
    public PaymentStatus Status { get; private set; }

    public Payment(long cardNumber, int expiryMonth, int expiryYear, string currency, int amount, string cvv) 
        : this(cardNumber.ToString(), expiryMonth, expiryYear, currency, amount, cvv){}
        
    public Payment(string cardNumber, int expiryMonth, int expiryYear, string currency, int amount, string cvv)
    {
        Id = Guid.NewGuid();
        CardNumber = new CardNumber(cardNumber);
        ExpiryDate = new ExpiryDate(expiryMonth, expiryYear);
        Currency = new Currency(currency);
        Amount = new Amount(amount);
        Cvv = new CVV(cvv);
        Status = PaymentStatus.Created;
    }
}