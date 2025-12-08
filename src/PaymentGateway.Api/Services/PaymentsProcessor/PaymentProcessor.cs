using System.Net;

using PaymentGateway.Api.Domain;

namespace PaymentGateway.Api.Services.PaymentsProcessor;

public class PaymentsProcessor : IPaymentsProcessor
{
    const string BankSimulatorUrl = "http://localhost:8080/payments";
    private readonly HttpClient _httpClient;

    public PaymentsProcessor(HttpClient httpClient)
    {
        _httpClient = httpClient;
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

        HttpResponseMessage response;

        try
        {
            response = await _httpClient.PostAsJsonAsync(
                BankSimulatorUrl,
                requestBody,
                cancellationToken
            );
        }
        catch (HttpRequestException)
        {
            // Bank unavailable
            return false;
        }

        if (response.StatusCode == HttpStatusCode.OK)
        {
            var result = await response.Content.ReadFromJsonAsync<BankResponse>(cancellationToken: cancellationToken);

            return result?.authorized ?? false;
        }

        return false;
    }

    private class BankResponse
    {
        public bool authorized { get; set; }
        public string authorization_code { get; set; } = string.Empty;
    }
}