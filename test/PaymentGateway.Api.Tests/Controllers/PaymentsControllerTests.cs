using System.Net;
using System.Net.Http.Json;

using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using PaymentGateway.Api.Controllers;
using PaymentGateway.Api.Domain;
using PaymentGateway.Api.Models.Responses;
using PaymentGateway.Api.Services.PaymentsRepository;
using PaymentGateway.Api.Tests.Mocks;
using PaymentGateway.Api.UseCases.GetPayment;

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
        var paymentResponse = await response.Content.ReadFromJsonAsync<GetPaymentResponse>();

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
}