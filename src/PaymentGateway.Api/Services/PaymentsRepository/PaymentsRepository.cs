using PaymentGateway.Api.Domain;

namespace PaymentGateway.Api.Services.PaymentsRepository;

public class PaymentsRepository : IPaymentsRepository
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

        public Task UpdatePaymentAsync(Payment payment, CancellationToken cancellationToken)
        {
            var existingPayment = Payments.FirstOrDefault(p => p.Id == payment.Id);
            if (existingPayment != null)
            {
                Payments.Remove(existingPayment);
                Payments.Add(payment);
            }
            return Task.CompletedTask;
        }
}