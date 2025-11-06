using InvestmentFundManager.Domain.Entities;

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
    }
}
