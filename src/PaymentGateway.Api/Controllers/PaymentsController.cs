using MediatR;

using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Models;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.Services.PaymentsProcessor;
using PaymentGateway.Api.Services.PaymentsRepository;
using PaymentGateway.Api.UseCases.GetPayment;
using PaymentGateway.Api.UseCases.ProcessPayment;

namespace PaymentGateway.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : Controller
{
    private readonly IPaymentsRepository _paymentsRepository;
    private readonly IPaymentsProcessor _paymentsProcessor;
    private readonly IMediator _mediator;

    public PaymentsController(IPaymentsRepository paymentsRepository, IPaymentsProcessor paymentsProcessor, IMediator mediator)
    {
        _paymentsRepository = paymentsRepository;
        _paymentsProcessor = paymentsProcessor;
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetPaymentResponse>> GetPaymentAsync(Guid id)
    {
        var request = new GetPaymentRequest(id);
        try{

            GetPaymentResponse paymentResponse = await _mediator.Send(request);
            return new OkObjectResult(paymentResponse);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<ProcessPaymentResponse>> ProcessPaymentAsync(
        [FromBody] PostPaymentRequest request)
    {
        var processPaymentRequest = new ProcessPaymentRequest(
            request.CardNumber,
            request.ExpiryMonth,
            request.ExpiryYear,
            request.Currency,
            request.Amount,
            request.Cvv);
        var handler = new ProcessPaymentHandler(_paymentsRepository, _paymentsProcessor);
        ProcessPaymentResponse response;
        try
        {
            response = await _mediator.Send(processPaymentRequest);
            return new OkObjectResult(response);
        }
        catch
        {
            response = new ProcessPaymentResponse
            {
                Id = Guid.Empty,
                Status = PaymentStatus.Rejected
            };
        }
        return new OkObjectResult(response);
    }
}