using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using InvestmentFundManager.Domain.Entities;
using InvestmentFundManager.Domain.Enum;
using InvestmentFundManager.Domain.Ports;
using InvestmentFundManager.Infrastructure.Config;
using Microsoft.Extensions.Options;

namespace InvestmentFundManager.Infrastructure.Adapters
{
    /// <summary>
    /// DynamoDB implementation of ITransactionRepository.
    /// Responsible for persisting fund transactions and retrieving user transaction data.
    /// </summary>
    public class TransactionRepository : ITransactionRepository
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly AwsSettings _settings;

        public TransactionRepository(IAmazonDynamoDB dynamoDb, IOptions<AwsSettings> settings)
        {
            _dynamoDb = dynamoDb ?? throw new ArgumentNullException(nameof(dynamoDb));
            _settings = settings.Value;
        }

        /// <inheritdoc />
        public async Task RegisterTransactionAsync(FundTransaction transaction)
        {
            var item = new Dictionary<string, AttributeValue>
            {
                ["Id"] = new AttributeValue(transaction.Id),
                ["User"] = new AttributeValue(transaction.User),
                ["Fund"] = new AttributeValue(transaction.Fund),
                ["Amount"] = new AttributeValue { N = transaction.Amount.ToString() },
                ["Date"] = new AttributeValue { S = transaction.Date.ToString("o") },
                ["Type"] = new AttributeValue(transaction.Type.ToString()),
                ["Status"] = new AttributeValue(transaction.Status.ToString()),
                ["NotificationChannel"] = new AttributeValue(transaction.NotificationChannel.ToString()),
                ["Recipient"] = new AttributeValue(transaction.Recipient),
                ["BalanceAfterTransaction"] = new AttributeValue { N = transaction.BalanceAfterTransaction.ToString() }
            };

            var request = new PutItemRequest
            {
                TableName = _settings.DynamoFundsTransactionsTableName,
                Item = item
            };

            await _dynamoDb.PutItemAsync(request);
        }

        /// <inheritdoc />
        public async Task<List<FundTransaction>> ListTransactionsAsync()
        {
            var request = new ScanRequest
            {
                TableName = _settings.DynamoFundsTransactionsTableName
            };

            var response = await _dynamoDb.ScanAsync(request);
            return response.Items.Select(MapToEntity).ToList();
        }

        #region 🔹 Helper Methods

        private static FundTransaction MapToEntity(Dictionary<string, AttributeValue> item)
        {
            return new FundTransaction
            {
                Id = item["Id"].S,
                User = item["User"].S,
                Fund = item["Fund"].S,
                Amount = decimal.Parse(item["Amount"].N),
                Date = DateTime.Parse(item["Date"].S),
                Type = Enum.Parse<TransactionType>(item["Type"].S),
                Status = Enum.Parse<TransactionStatus>(item["Status"].S),
                NotificationChannel = Enum.Parse<NotificationChannel>(item["NotificationChannel"].S),
                Recipient = item["Recipient"].S,
                BalanceAfterTransaction = decimal.Parse(item["BalanceAfterTransaction"].N)
            };
        }

        #endregion
    }
}
