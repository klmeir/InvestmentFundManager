using InvestmentFundManager.Domain.Entities;
using InvestmentFundManager.Domain.Enum;

namespace InvestmentFundManager.Domain.Ports
{
    /// <summary>
    /// Defines persistence operations for fund transactions.
    /// Implementations may use DynamoDB, SQL Server, or any other storage engine.
    /// </summary>
    public interface ITransactionRepository
    {

        /// <summary>
        /// Registers a new fund transaction (subscription or cancellation).
        /// </summary>
        /// <param name="transaction">The transaction to be stored.</param>
        Task RegisterTransactionAsync(FundTransaction transaction);

        /// <summary>
        /// Retrieves all fund transactions across all users.
        /// </summary>
        /// <returns>A list of all transactions.</returns>
        Task<List<FundTransaction>> ListTransactionsAsync();

        /// <summary>
        /// Updates the status of an existing transaction by its Id.
        /// This is useful for cancellation or status changes without modifying other fields.
        /// </summary>
        /// <param name="transactionId">The Id of the transaction to update.</param>
        /// <param name="newStatus">The new status to set.</param>
        Task UpdateTransactionStatusAsync(string transactionId, TransactionStatus newStatus);

        /// <summary>
        /// Retrieves the latest active subscription of a user for a specific fund.
        /// Returns null if there is no active subscription.
        /// </summary>
        Task<FundTransaction> GetActiveSubscriptionAsync(string userId, string fundId);
    }
}
