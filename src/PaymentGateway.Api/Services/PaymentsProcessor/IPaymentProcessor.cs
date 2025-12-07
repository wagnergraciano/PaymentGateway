using PaymentGateway.Api.Domain;

namespace PaymentGateway.Api.Services.PaymentsProcessor;

public interface IPaymentsProcessor
{
    Task ProcessPaymentAsync(Payment payment, CancellationToken cancellationToken);
}