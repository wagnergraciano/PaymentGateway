using PaymentGateway.Api.Domain;
using PaymentGateway.Api.Services.PaymentsRepository;

namespace PaymentGateway.Api.Tests.Mocks
{
    public class MockPaymentsRepository : IPaymentsRepository
    {
        public IList<Payment> Payments { get; private set; } = new List<Payment>();

        public Task AddPaymentAsync(Payment payment, CancellationToken cancellationToken)
        {
            Payments.Add(payment);
            return Task.CompletedTask;
        }

        public Task<Payment> GetPaymentByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            var payment = Payments.FirstOrDefault(p => p.Id == id);
            return Task.FromResult(payment);
        }
    }
}