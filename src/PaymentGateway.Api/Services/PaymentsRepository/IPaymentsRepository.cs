using PaymentGateway.Api.Domain;

namespace PaymentGateway.Api.Services.PaymentsRepository;

public interface IPaymentsRepository
{
    IList<Payment> Payments { get; }
    Task AddPaymentAsync(Payment payment, CancellationToken cancellationToken);
    Task UpdatePaymentAsync(Payment payment, CancellationToken cancellationToken);
    Task<Payment> GetPaymentByIdAsync(Guid id, CancellationToken cancellationToken);
}