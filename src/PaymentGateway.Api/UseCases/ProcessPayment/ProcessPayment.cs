using MediatR;
using PaymentGateway.Api.Domain;

namespace PaymentGateway.Api.UseCases.ProcessPayment;

public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentRequest, ProcessPaymentResponse>
{
    private readonly IPaymentsRepository _repository;
    private readonly IExternalPaymentProcessor _paymentProcessor;

    public ProcessPaymentHandler(IPaymentsRepository repository, IExternalPaymentProcessor paymentProcessor)
    {
        _repository = repository;
        _paymentProcessor = paymentProcessor;
    }

    public async Task<ProcessPaymentResponse> Handle(ProcessPaymentRequest request, CancellationToken cancellationToken)
    {
        var payment = new Payment(
            request.CardNumber,
            request.ExpiryMonth,
            request.ExpiryYear,
            request.Currency,
            request.Amount,
            request.Cvv
        );

        // Call external payment processor
        var processorResponse = await _paymentProcessor.ProcessPaymentAsync(payment);
        payment.UpdateStatus(processorResponse.Status);

        // Save payment in database
        await _repository.SavePaymentAsync(payment);

        // Return response
        return new ProcessPaymentResponse
        {
            Id = payment.Id,
            Status = payment.Status
        };
    }
}