using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;

using PaymentGateway.Api.Services.PaymentsProcessor;
using PaymentGateway.Api.Services.PaymentsRepository;
using PaymentGateway.Api.Tests.Mocks;

namespace PaymentGateway.Api.Tests.Controllers;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            var descriptorRepository = services.SingleOrDefault(
                d => d.ServiceType == typeof(IPaymentsRepository));
            if (descriptorRepository != null)
                services.Remove(descriptorRepository);
            services.AddSingleton<IPaymentsRepository, MockPaymentsRepository>();

            var descriptorProcessor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IPaymentsProcessor));
            if (descriptorProcessor != null)
                services.Remove(descriptorProcessor);
            services.AddSingleton<IPaymentsProcessor, MockPaymentsProcessor>();
        });
    }
}