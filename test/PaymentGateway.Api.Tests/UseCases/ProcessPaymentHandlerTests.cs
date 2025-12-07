using PaymentGateway.Api.Models;
using PaymentGateway.Api.Tests.Mocks;
using PaymentGateway.Api.UseCases.ProcessPayment;

namespace PaymentGateway.Api.Tests.UseCases.ProcessPayment;

public class ProcessPaymentHandlerTests
{
    [Fact]
    public async Task Handle_ShouldProcessPaymentSuccessfully()
    {
        // Arrange
        var paymentRequest = new ProcessPaymentRequest(
            "12345678901234", 12, 2030, "USD", 1000, "123"
        );

        var mockRepository = new MockPaymentsRepository();
        var mockProcessor = new MockPaymentsProcessor();

        var handler = new ProcessPaymentHandler(mockRepository, mockProcessor);

        // Act
        ProcessPaymentResponse response = await handler.Handle(paymentRequest, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(PaymentStatus.Authorized, response.Status);
        
        Assert.Single(mockRepository.Payments);
        var savedPayment = mockRepository.Payments.First();
        Assert.Equal(paymentRequest.CardNumber, savedPayment.CardNumber.Value.ToString());
        Assert.Equal(paymentRequest.ExpiryMonth, savedPayment.ExpiryDate.Month);
        Assert.Equal(paymentRequest.ExpiryYear, savedPayment.ExpiryDate.Year);
        Assert.Equal(paymentRequest.Currency, savedPayment.Currency.Value);
        Assert.Equal(paymentRequest.Amount, savedPayment.Amount.Value);
    }

    [Fact]
    public async Task Handle_ShouldReturnFailedStatus_WhenProcessorFails()
    {
        // Arrange
        var paymentRequest = new ProcessPaymentRequest(
            "12345678901234", 12, 2030, "GBP", 1000, "123"
        );

        var mockRepository = new MockPaymentsRepository();
        var mockProcessor = new MockPaymentsProcessor();

        var handler = new ProcessPaymentHandler(mockRepository, mockProcessor);

        // Act
        ProcessPaymentResponse response = await handler.Handle(paymentRequest, CancellationToken.None);

        // Assert
        Assert.NotNull(response);
        Assert.Equal(PaymentStatus.Declined, response.Status);

        Assert.Single(mockRepository.Payments);
        var savedPayment = mockRepository.Payments.First();
        Assert.Equal(paymentRequest.CardNumber, savedPayment.CardNumber.Value.ToString());
        Assert.Equal(paymentRequest.ExpiryMonth, savedPayment.ExpiryDate.Month);
        Assert.Equal(paymentRequest.ExpiryYear, savedPayment.ExpiryDate.Year);
        Assert.Equal(paymentRequest.Currency, savedPayment.Currency.Value);
        Assert.Equal(paymentRequest.Amount, savedPayment.Amount.Value);
    }

    [Fact]
    public async Task Handle_ShouldThrow_WhenProcessorResquestAsInvalidParams()
    {
        // Arrange
        var paymentRequest = new ProcessPaymentRequest(
            "123456", 12, 2030, "GBP", 1000, "123"
        );

        var mockRepository = new MockPaymentsRepository();
        var mockProcessor = new MockPaymentsProcessor();

        var handler = new ProcessPaymentHandler(mockRepository, mockProcessor);

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(async () =>
            await handler.Handle(paymentRequest, CancellationToken.None));
    }
}