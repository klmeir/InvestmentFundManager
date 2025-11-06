using FluentValidation;
using FluentValidation.Results;
using InvestmentFundManager.Application.Funds.Commands;
using InvestmentFundManager.Application.Funds.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InvestmentFundManager.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FundsController : ControllerBase
    {
        private IValidator<SubscribeFundCommand> _subscribeValidator;
        private IValidator<CancelFundCommand> _cancelValidator;
        private readonly IMediator _mediator;

        public FundsController(IValidator<SubscribeFundCommand> subscribeValidator, IValidator<CancelFundCommand> cancelValidator, IMediator mediator)
        {
            _subscribeValidator = subscribeValidator;
            _cancelValidator = cancelValidator;
            _mediator = mediator;
        }

        [HttpGet()]
        public async Task<IActionResult> ListFunds()
        {
            var funds = await _mediator.Send(new GetFundsQuery());
            return Ok(funds);
        }

        [HttpPost("subscribe")]
        public async Task<IActionResult> Subscribe([FromBody] SubscribeFundCommand model)
        {
            ValidationResult result = await _subscribeValidator.ValidateAsync(model);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var id = await _mediator.Send(model);
            return Ok(new { message = $"User subscribed successfully to fund.", id });
        }

        [HttpPost("cancel")]
        public async Task<IActionResult> Cancel([FromBody] CancelFundCommand model)
        {
            ValidationResult result = await _cancelValidator.ValidateAsync(model);

            if (!result.IsValid)
            {
                return BadRequest(result);
            }

            var id = await _mediator.Send(model);
            return Ok(new { message = "Cancellation registered successfully", id });
        }

        [HttpGet("transactions")]
        public async Task<IActionResult> ListTransactions()
        {
            var transactions = await _mediator.Send(new GetTransactionsQuery());
            return Ok(transactions);
        }
    }
}
