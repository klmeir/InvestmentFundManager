using InvestmentFundManager.Domain.Enum;
using MediatR;

namespace InvestmentFundManager.Application.Funds.Commands
{
    /// <summary>
    /// Command to subscribe a user to a fund.
    /// </summary>
    /// <param name="Fund">Fund name to subscribe to.</param>
    /// <param name="NotificationChannel">Notification channel selected by the user (EMAIL or SMS).</param>
    /// <param name="Recipient">Email or phone number where the user will receive notifications.</param>
    public record SubscribeFundCommand(        
        string FundId,
        NotificationChannel NotificationChannel
    ) : IRequest<string>;
}
