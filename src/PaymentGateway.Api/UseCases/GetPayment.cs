using MediatR;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.UseCases;

public class GetPaymentQuery : IRequest<GetPaymentResponse>
{
    public Guid PaymentId { get; }

    public GetPaymentQuery(Guid paymentId)
    {
        PaymentId = paymentId;
    }
}

public class GetPaymentHandler : IRequestHandler<GetPaymentQuery, GetPaymentResponse>
{
    private readonly IPaymentsRepository _repository;

    public GetPaymentHandler(IPaymentsRepository repository)
    {
        _repository = repository;
    }

    public async Task<GetPaymentResponse> Handle(GetPaymentQuery request, CancellationToken cancellationToken)
    {
        var payment = await _repository.GetPaymentByIdAsync(request.PaymentId);
        if (payment == null)
        {
            throw new KeyNotFoundException("Payment not found.");
        }

        return new GetPaymentResponse
        {
            Id = payment.Id,
            Amount = payment.Amount,
            Currency = payment.Currency,
            Status = payment.Status
        };
    }
}