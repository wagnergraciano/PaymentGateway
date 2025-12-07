using MediatR;
using PaymentGateway.Api.Domain;
using PaymentGateway.Api.Services.PaymentsProcessor;
using PaymentGateway.Api.Services.PaymentsRepository;

namespace PaymentGateway.Api.UseCases.ProcessPayment;

public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentRequest, ProcessPaymentResponse>
{
    private readonly IPaymentsRepository _repository;
    private readonly IPaymentsProcessor _paymentProcessor;

    public ProcessPaymentHandler(IPaymentsRepository repository, IPaymentsProcessor paymentProcessor)
    {
        _repository = repository;
        _paymentProcessor = paymentProcessor;
    }

    public async Task<ProcessPaymentResponse> Handle(ProcessPaymentRequest request, CancellationToken cancellationToken)
    {          
        Payment payment = new Payment(
            request.CardNumber,
            request.ExpiryMonth,
            request.ExpiryYear,
            request.Currency,
            request.Amount,
            request.Cvv
        );
        
        bool isPaymentProcessed = await _paymentProcessor.ProcessPaymentAsync(payment, cancellationToken);
        payment.UpdateStatus(isPaymentProcessed);        
        await _repository.AddPaymentAsync(payment, cancellationToken);

        return new ProcessPaymentResponse
        {
            Id = payment.Id,
            Status = payment.Status
        };
    }
}