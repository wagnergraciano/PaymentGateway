using PaymentGateway.Api.UseCases.ProcessPayment;

namespace PaymentGateway.Api.Controllers.DTOs;

public class ProcessPaymentResponseDto
{
    public Guid Id { get; set; }
    public string Status { get; set; }
    public int CardNumberLastFour { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string Currency { get; set; }
    public int Amount { get; set; }

    public static ProcessPaymentResponseDto FromDomain(ProcessPaymentResponse response)
    {
        return new ProcessPaymentResponseDto
        {
            Id = response.Id,
            Status = response.Status.ToString(),
            CardNumberLastFour = response.CardNumberLastFour,
            ExpiryMonth = response.ExpiryMonth,
            ExpiryYear = response.ExpiryYear,
            Currency = response.Currency,
            Amount = response.Amount
        };
    }
}