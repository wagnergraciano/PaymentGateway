namespace PaymentGateway.Api.Services.PaymentRepository;

public interface IPaymentsRepository
{
    IList<Payment> Payments;    
    async void AddPaymentAsync(Payment payment, CancellationToken cancellationToken);
    async Payment GetPaymentByIdAsync(Guid id, CancellationToken cancellationToken);
}