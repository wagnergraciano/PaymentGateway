using PaymentGateway.Api.Domain;
using PaymentGateway.Api.Models;
using PaymentGateway.Api.Tests.Mocks;
using PaymentGateway.Api.UseCases.GetPayment;

namespace PaymentGateway.Api.Tests.UseCases.GetPayment;

public class GetPaymentHandlerTests
{
    [Fact]
    public async Task Handle_ShouldReturnPaymentDetails_WhenPaymentExists()
    {
        // Arrange
        var mockRepository = new MockPaymentsRepository();

        var payment = new Payment(
            "12345678901234", 12, 2030, "USD", 1000, "123"
        );
        await mockRepository.AddPaymentAsync(payment, CancellationToken.None);

        var handler = new GetPaymentHandler(mockRepository);
        var request = new GetPaymentRequest(payment.Id);

        // Act
        GetPaymentResponse response = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(payment.Id, response.Id);
        Assert.Equal(payment.Status, response.Status);
        Assert.Equal(payment.CardNumber.GetMaskedNumber(), response.CardNumberLastFour);
        Assert.Equal(payment.ExpiryDate.Month, response.ExpiryMonth);
        Assert.Equal(payment.ExpiryDate.Year, response.ExpiryYear);
        Assert.Equal(payment.Currency.Value, response.Currency);
        Assert.Equal(payment.Amount.Value, response.Amount);
    }

    [Fact]
    public async Task Handle_ShouldThrowException_WhenPaymentDoesNotExist()
    {
        // Arrange
        var mockRepository = new MockPaymentsRepository();
        var handler = new GetPaymentHandler(mockRepository);
        var request = new GetPaymentRequest(Guid.NewGuid());

        // Act & Assert
        await Assert.ThrowsAsync<KeyNotFoundException>(async () =>
            await handler.Handle(request, CancellationToken.None));
    }
}