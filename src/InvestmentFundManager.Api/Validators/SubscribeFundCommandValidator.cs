using FluentValidation;
using InvestmentFundManager.Application.Funds.Commands;

namespace InvestmentFundManager.Api.Validators
{
    /// <summary>
    /// Validates the subscription command input before it reaches the domain layer.
    /// </summary>
    public class SubscribeFundCommandValidator : AbstractValidator<SubscribeFundCommand>
    {
        public SubscribeFundCommandValidator()
        {

            RuleFor(x => x.FundId)
                .NotEmpty().WithMessage("FundId is required.");

            RuleFor(x => x.NotificationChannel)
                .IsInEnum()
                .WithMessage("Notification channel must be EMAIL or SMS.");
        }
    }
}
