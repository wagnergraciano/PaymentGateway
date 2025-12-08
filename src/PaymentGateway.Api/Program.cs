using System.Reflection;

using Microsoft.Extensions.DependencyInjection;

using PaymentGateway.Api.Services.PaymentsProcessor;
using PaymentGateway.Api.Services.PaymentsRepository;
using PaymentGateway.Api.UseCases.GetPayment;
using PaymentGateway.Api.UseCases.ProcessPayment;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddSingleton<IPaymentsRepository, PaymentsRepository>();
builder.Services.AddHttpClient<IPaymentsProcessor, PaymentsProcessor>();
builder.Services.AddMediatR(cfg =>
{
    cfg.RegisterServicesFromAssembly(typeof(GetPaymentHandler).Assembly);
    cfg.RegisterServicesFromAssembly(typeof(ProcessPaymentHandler).Assembly);
});
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }