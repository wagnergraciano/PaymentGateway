using MediatR;

namespace PaymentGateway.Api.UseCases.GetPayment;

public class GetPaymentRequest : IRequest<GetPaymentResponse>
{
    public Guid PaymentId { get; }
    public GetPaymentRequest(Guid paymentId)
    {
        PaymentId = paymentId;
    }
}