using System.Net;
using System.Net.Http.Json;

using Microsoft.Extensions.DependencyInjection;

using PaymentGateway.Api.Controllers.DTOs;
using PaymentGateway.Api.Domain;
using PaymentGateway.Api.Models;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Services.PaymentsRepository;
using PaymentGateway.Api.Tests.Mocks;
using PaymentGateway.Api.UseCases.GetPayment;
using PaymentGateway.Api.UseCases.ProcessPayment;

namespace PaymentGateway.Api.Tests.Controllers;

public class PaymentsControllerTests
{   
    [Fact]
    public async Task GetPaymentAsync_ShouldReturnOk_WhenPaymentExists()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory();

        var mockRepository = factory.Services.GetRequiredService<IPaymentsRepository>() 
                             as MockPaymentsRepository;

        var client = factory.CreateClient();

        var payment = new Payment("12345678901234", 12, 2030, "USD", 1000, "123");
        await mockRepository.AddPaymentAsync(payment, CancellationToken.None);

        // Act
        var response = await client.GetAsync($"/api/Payments/{payment.Id}");
        var paymentResponse = await response.Content.ReadFromJsonAsync<GetPaymentResponseDto>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(paymentResponse);
    }

    [Fact]
    public async Task Returns404IfPaymentNotFound()
    {
        var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        var response = await client.GetAsync($"/api/Payments/{Guid.NewGuid()}");

        Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
    }

    [Fact]
    public async Task ProcessPaymentAsync_ShouldReturnAuthorized_WhenProcessorReturnsTrue()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        var requestBody = new PostPaymentRequest
        {
            CardNumber = "1234567890123456",
            ExpiryMonth = 12,
            ExpiryYear = 2030,
            Currency = "USD",
            Amount = 100,
            Cvv = "123"
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/payments", requestBody);
        var result = await response.Content.ReadFromJsonAsync<ProcessPaymentResponseDto>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);
        Assert.NotEqual(Guid.Empty, result.Id);
        Assert.Equal(PaymentStatus.Authorized.ToString(), result.Status);

        var mockRepository = factory.Services.GetRequiredService<IPaymentsRepository>() 
                             as MockPaymentsRepository;
        var savedPayment = mockRepository.Payments.First();
        Assert.Equal(requestBody.CardNumber, savedPayment.CardNumber.Value.ToString());
        Assert.Equal(requestBody.ExpiryMonth, savedPayment.ExpiryDate.Month);
        Assert.Equal(requestBody.ExpiryYear, savedPayment.ExpiryDate.Year);
        Assert.Equal(requestBody.Currency, savedPayment.Currency.Value);
        Assert.Equal(requestBody.Amount, savedPayment.Amount.Value);
    }

    [Fact]
    public async Task ProcessPaymentAsync_ShouldReturnRejected_WhenProcessorReturnsFalse()
    {
        // Arrange
        var factory = new CustomWebApplicationFactory();
        var client = factory.CreateClient();

        var requestBody = new PostPaymentRequest
        {
            CardNumber = "9876543210987654",
            ExpiryMonth = 11,
            ExpiryYear = 2030,
            Currency = "GBP",
            Amount = 200,
            Cvv = "321"
        };

        // Act
        var response = await client.PostAsJsonAsync("/api/payments", requestBody);
        var result = await response.Content.ReadFromJsonAsync<ProcessPaymentResponseDto>();

        // Assert
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        Assert.NotNull(result);        
        Assert.Equal(PaymentStatus.Declined.ToString(), result.Status);
    }
}