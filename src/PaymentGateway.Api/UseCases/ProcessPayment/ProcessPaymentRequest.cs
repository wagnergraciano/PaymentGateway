using MediatR;

namespace PaymentGateway.Api.UseCases.ProcessPayment;

public class ProcessPaymentRequest : IRequest<ProcessPaymentResponse>
{
    public string CardNumber { get; }    
    public int ExpiryMonth { get; }
    public int ExpiryYear { get; }
    public string Currency { get; }
    public int Amount { get; }
    public string Cvv { get; }

    public ProcessPaymentRequest(string cardNumber, int expiryMonth, int expiryYear, string currency, int amount, string cvv)
    {
        CardNumber = cardNumber;
        ExpiryMonth = expiryMonth;
        ExpiryYear = expiryYear;
        Currency = currency;
        Amount = amount;
        Cvv = cvv;
    }
}