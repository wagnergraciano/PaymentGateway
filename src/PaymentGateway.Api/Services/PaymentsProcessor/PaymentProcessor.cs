using System.Net;
using System.Net.Http.Json;
using Microsoft.Extensions.Logging;
using PaymentGateway.Api.Domain;

namespace PaymentGateway.Api.Services.PaymentsProcessor;

public class PaymentsProcessor : IPaymentsProcessor
{
    private const string BankSimulatorUrl = "http://localhost:8080/payments";

    private readonly HttpClient _httpClient;
    private readonly ILogger<PaymentsProcessor> _logger;

    public PaymentsProcessor(HttpClient httpClient, ILogger<PaymentsProcessor> logger)
    {
        _httpClient = httpClient;
        _logger = logger;
    }

    public async Task<bool> ProcessPaymentAsync(Payment payment, CancellationToken cancellationToken)
    {
        var requestBody = new
        {
            card_number = payment.CardNumber.Value.ToString(),
            expiry_date = payment.GetFormattedExpiryDate(),
            currency = payment.Currency,
            amount = payment.Amount,
            cvv = payment.Cvv.Value
        };

        _logger.LogInformation(
            "Sending payment request to bank simulator. Card: ****{Last4}, Amount: {Amount} {Currency}",
            payment.GetMaskedCardNumber(),
            payment.Amount,
            payment.Currency
        );

        HttpResponseMessage response;

        try
        {
            response = await _httpClient.PostAsJsonAsync(
                BankSimulatorUrl,
                requestBody,
                cancellationToken
            );
        }
        catch (HttpRequestException ex)
        {
            _logger.LogError(ex, "Failed to reach bank simulator at {Url}", BankSimulatorUrl);
            return false;
        }

        _logger.LogInformation(
            "Bank simulator responded with status code {StatusCode}",
            response.StatusCode
        );

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var result = await response.Content.ReadFromJsonAsync<BankResponse>(cancellationToken: cancellationToken);

            if (result is null)
            {
                _logger.LogWarning("Bank simulator returned empty or invalid response.");
                return false;
            }

            _logger.LogInformation(
                "Bank response: Authorized={Authorized}, AuthCode={AuthCode}",
                result.authorized,
                result.authorization_code
            );

            return result.authorized;
        }

        _logger.LogWarning(
            "Bank simulator returned a non-success status code: {StatusCode}",
            response.StatusCode
        );

        return false;
    }

    private class BankResponse
    {
        public bool authorized { get; set; }
        public string authorization_code { get; set; } = string.Empty;
    }
}