using MediatR;

using Microsoft.AspNetCore.Mvc;

using PaymentGateway.Api.Controllers.DTOs;
using PaymentGateway.Api.Models;
using PaymentGateway.Api.Models.Requests;
using PaymentGateway.Api.UseCases.GetPayment;
using PaymentGateway.Api.UseCases.ProcessPayment;

namespace PaymentGateway.Api.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PaymentsController : Controller
{
    private readonly IMediator _mediator;

    public PaymentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("{id:guid}")]
    public async Task<ActionResult<GetPaymentResponseDto>> GetPaymentAsync(Guid id)
    {
        var request = new GetPaymentRequest(id);
        try{

            GetPaymentResponse paymentResponse = await _mediator.Send(request);
            var responseDto = GetPaymentResponseDto.FromDomain(paymentResponse);
            return new OkObjectResult(responseDto);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<ActionResult<ProcessPaymentResponseDto>> ProcessPaymentAsync(
        [FromBody] PostPaymentRequest request)
    {
        ProcessPaymentResponseDto responseDto;
        var processPaymentRequest = new ProcessPaymentRequest(
            request.CardNumber,
            request.ExpiryMonth,
            request.ExpiryYear,
            request.Currency,
            request.Amount,
            request.Cvv);
        try
        {
            ProcessPaymentResponse response = await _mediator.Send(processPaymentRequest);
            responseDto = ProcessPaymentResponseDto.FromDomain(response);
        }
        catch
        {
            ProcessPaymentResponse response = new ProcessPaymentResponse
            {
                Id = Guid.Empty,
                Status = PaymentStatus.Rejected
            };
            responseDto = ProcessPaymentResponseDto.FromDomain(response);
        }
        return new OkObjectResult(responseDto);
    }
}