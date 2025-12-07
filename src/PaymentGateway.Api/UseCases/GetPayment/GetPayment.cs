using MediatR;
using PaymentGateway.Api.Domain;
using PaymentGateway.Api.Services.PaymentsRepository;

namespace PaymentGateway.Api.UseCases.GetPayment;

public class GetPaymentHandler : IRequestHandler<GetPaymentRequest, GetPaymentResponse>
{
    private readonly IPaymentsRepository _repository;

    public GetPaymentHandler(IPaymentsRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetPaymentResponse> Handle(GetPaymentRequest request, CancellationToken cancellationToken)
    {       
        Payment payment = await _repository.GetPaymentByIdAsync(request.PaymentId, cancellationToken);
        if(payment == null)
        {
            throw new KeyNotFoundException($"Payment with ID {request.PaymentId} not found.");
        }

        return new GetPaymentResponse
        {
            Id = payment.Id,
            Status = payment.Status,
            CardNumberLastFour = payment.GetMaskedCardNumber(),
            ExpiryMonth = payment.ExpiryDate.Month,
            ExpiryYear = payment.ExpiryDate.Year,
            Currency = payment.Currency.Value,
            Amount = payment.Amount.Value
        };
    }
}