using InvestmentFundManager.Domain.Enum;

namespace InvestmentFundManager.Domain.Entities
{
    /// <summary>
    /// Represents a single user of the investment platform.
    /// Stores initial and current balance, contact information, and notification preferences.
    /// </summary>
    public class User
    {
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Full name or identifier of the user.
        /// </summary>
        public string Name { get; set; } = "Default User";

        /// <summary>
        /// User email address for email notifications.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// User phone number for SMS notifications.
        /// </summary>
        public string PhoneNumber { get; set; } = string.Empty;

        /// <summary>
        /// Preferred notification channel (EMAIL or SMS).
        /// </summary>
        public NotificationChannel PreferredChannel { get; set; } = NotificationChannel.EMAIL;

        /// <summary>
        /// The user's initial balance when the account was created.
        /// </summary>
        public decimal InitialBalance { get; set; } = 500_000M;

        /// <summary>
        /// The user's current available balance, updated after each transaction.
        /// </summary>
        public decimal Balance { get; set; } = 500_000M;

        /// <summary>
        /// Last update timestamp.
        /// </summary>
        public DateTime LastUpdated { get; set; } = DateTime.UtcNow;
    }
}
