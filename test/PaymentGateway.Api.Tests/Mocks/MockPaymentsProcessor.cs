using PaymentGateway.Api.Domain;
using PaymentGateway.Api.Services.PaymentsProcessor;

namespace PaymentGateway.Api.Tests.Mocks
{
    public class MockPaymentsProcessor : IPaymentsProcessor
    {
        public Task<bool> ProcessPaymentAsync(Payment payment, CancellationToken cancellationToken)
        {
            return payment.Currency.Value.Equals("USD") ? Task.FromResult(true) : Task.FromResult(false);
        }
    }
}