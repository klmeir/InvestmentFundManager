using MediatR;

namespace InvestmentFundManager.Application.Funds.Commands
{
    /// <summary>
    /// Command to cancel a user's fund subscription.
    /// Recipient is not required because it will reuse the channel and recipient from the subscription.
    /// </summary>
    /// <param name="Fund">Fund name to cancel subscription from.</param>
    public record CancelFundCommand(        
        string FundId
    ) : IRequest<string>;
}
