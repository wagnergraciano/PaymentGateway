using PaymentGateway.Api.UseCases.GetPayment;

namespace PaymentGateway.Api.Controllers.DTOs;

public class GetPaymentResponseDto
{
    public Guid Id { get; set; }
    public string Status { get; set; }
    public string CardNumberLastFour { get; set; }
    public int ExpiryMonth { get; set; }
    public int ExpiryYear { get; set; }
    public string Currency { get; set; }
    public int Amount { get; set; }

    public static GetPaymentResponseDto FromDomain(GetPaymentResponse response)
    {
        return new GetPaymentResponseDto
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