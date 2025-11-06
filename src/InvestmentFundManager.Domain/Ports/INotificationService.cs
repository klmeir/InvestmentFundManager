using InvestmentFundManager.Domain.Entities;

namespace InvestmentFundManager.Domain.Ports
{
    /// <summary>
    /// Defines an abstraction for sending user notifications.
    /// Implementations may use Amazon SNS, email services, or other messaging systems.
    /// </summary>
    public interface INotificationService
    {
        Task NotifyAsync(FundTransaction model, string message);
    }
}
