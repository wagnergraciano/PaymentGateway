using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Models.Responses;
using PaymentGateway.Api.Services.PaymentsRepository;
using PaymentGateway.Api.UseCases.GetPayment;

namespace PaymentGateway.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : Controller
{
    private readonly IPaymentsRepository _paymentsRepository;

    public PaymentsController(IPaymentsRepository paymentsRepository)
    {
        _paymentsRepository = paymentsRepository;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetPaymentResponse>> GetPaymentAsync(Guid id)
    {
        var request = new GetPaymentRequest(id);
        var handler = new GetPaymentHandler(_paymentsRepository);
        try{

            GetPaymentResponse paymentResponse = await handler.Handle(request, CancellationToken.None);
            return new OkObjectResult(paymentResponse);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

    }
}