using FluentValidation;
using InvestmentFundManager.Application.Funds.Commands;

namespace InvestmentFundManager.Api.Validators
{
    /// <summary>
    /// Validates the cancellation command input.
    /// </summary>
    public class CancelFundCommandValidator : AbstractValidator<CancelFundCommand>
    {
        public CancelFundCommandValidator()
        {
            RuleFor(x => x.FundId)
                .NotEmpty().WithMessage("Fund Id is required.");
        }
    }
}
