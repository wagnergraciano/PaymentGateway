using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.TestHost;

using PaymentGateway.Api.Services.PaymentsRepository;
using PaymentGateway.Api.Tests.Mocks;

namespace PaymentGateway.Api.Tests.Controllers;
public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            // Encontra e remove o serviço real
            var descriptor = services.SingleOrDefault(
                d => d.ServiceType == typeof(IPaymentsRepository));

            if (descriptor != null)
                services.Remove(descriptor); // REMOVE o PaymentsRepository verdadeiro

            // Adiciona o Mock
            services.AddSingleton<IPaymentsRepository, MockPaymentsRepository>();
        });
    }
}