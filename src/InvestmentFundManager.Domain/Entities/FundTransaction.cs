using InvestmentFundManager.Domain.Enum;

namespace InvestmentFundManager.Domain.Entities
{
    /// <summary>
    /// Represents a fund transaction (subscription or cancellation).
    /// Stored in DynamoDB through the repository layer.
    /// </summary>
    public class FundTransaction
    {
        /// <summary>
        /// Unique identifier of the transaction.
        /// </summary>
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// User performing the transaction.
        /// </summary>
        public string User { get; set; } = string.Empty;

        /// <summary>
        /// Fund name involved in the transaction.
        /// </summary>
        public string FundId { get; set; } = string.Empty;

        /// <summary>
        /// Fund name involved in the transaction.
        /// </summary>
        public string Fund { get; set; } = string.Empty;

        /// <summary>
        /// Transaction amount in COP.
        /// </summary>
        public decimal Amount { get; set; }

        /// <summary>
        /// Type of the transaction: SUBSCRIPTION or CANCELLATION.
        /// </summary>
        public TransactionType Type { get; set; }

        /// <summary>
        /// Current status of the transaction: COMPLETED, CANCELED, etc.
        /// </summary>
        public TransactionStatus Status { get; set; }

        /// <summary>
        /// Date and time when the transaction was created (UTC).
        /// </summary>
        public DateTime Date { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// Notification channel chosen by the user (EMAIL or SMS).
        /// </summary>
        public NotificationChannel NotificationChannel { get; set; }

        /// <summary>
        /// Recipient to notify (email address or phone number).
        /// </summary>
        public string Recipient { get; set; } = string.Empty;

        /// <summary>
        /// Optional: Fund category (e.g., FPV or FIC).
        /// </summary>
        public string? Category { get; set; }

        /// <summary>
        /// Optional: Minimum required amount for fund linkage.
        /// </summary>
        public decimal? MinimumAmount { get; set; }

        /// <summary>
        /// Optional: Available balance after the transaction.
        /// </summary>
        public decimal? BalanceAfterTransaction { get; set; }

        public FundTransaction()
        {
        }

        /// <summary>
        /// Creates a readable summary of the transaction.
        /// </summary>
        public override string ToString()
        {
            return $"{Type} | Fund: {Fund} | User: {User} | Amount: {Amount:C} | Status: {Status}";
        }
    }
}
