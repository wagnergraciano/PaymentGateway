using MediatR;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Models.Responses;

namespace PaymentGateway.Api.UseCases;

public class ProcessPaymentCommand : IRequest<PostPaymentResponse>
{
    public PostPaymentRequest PaymentRequest { get; }

    public ProcessPaymentCommand(PostPaymentRequest paymentRequest)
    {
        PaymentRequest = paymentRequest;
    }
}

public class ProcessPaymentHandler : IRequestHandler<ProcessPaymentCommand, PostPaymentResponse>
{
    private readonly IPaymentsRepository _repository;
    private readonly IExternalPaymentProcessor _paymentProcessor;

    public ProcessPaymentHandler(IPaymentsRepository repository, IExternalPaymentProcessor paymentProcessor)
    {
        _repository = repository;
        _paymentProcessor = paymentProcessor;
    }

    public async Task<PostPaymentResponse> Handle(ProcessPaymentCommand request, CancellationToken cancellationToken)
    {
        // Begin transaction
        var payment = new Payment(
            request.PaymentRequest.CardNumber,
            request.PaymentRequest.ExpiryMonth,
            request.PaymentRequest.ExpiryYear,
            request.PaymentRequest.Currency,
            request.PaymentRequest.Amount,
            request.PaymentRequest.Cvv
        );

        // Call external payment processor
        var processorResponse = await _paymentProcessor.ProcessPaymentAsync(payment);
        payment.UpdateStatus(processorResponse.Status);

        // Save payment in database
        await _repository.SavePaymentAsync(payment);

        // Return response
        return new PostPaymentResponse
        {
            Id = payment.Id,
            Status = payment.Status
        };
    }
}