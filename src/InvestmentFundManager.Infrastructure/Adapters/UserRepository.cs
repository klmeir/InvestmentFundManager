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
    /// DynamoDB implementation of IUserRepository.
    /// Manages user balance and contact preferences.
    /// </summary>
    public class UserRepository : IUserRepository
    {
        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly AwsSettings _settings;

        public UserRepository(IAmazonDynamoDB dynamoDb, IOptions<AwsSettings> settings)
        {
            _dynamoDb = dynamoDb;
            _settings = settings.Value;
        }

        public async Task<User?> GetUserAsync()
        {
            var response = await _dynamoDb.ScanAsync(new ScanRequest
            {
                TableName = _settings.DynamoUserTableName,
                Limit = 1
            });

            return response.Items.Count > 0 ? MapToEntity(response.Items.First()) : null;
        }

        public async Task UpdateUserAsync(User user)
        {
            var request = new PutItemRequest
            {
                TableName = _settings.DynamoUserTableName,
                Item = MapToItem(user)
            };

            await _dynamoDb.PutItemAsync(request);
        }

        private static User MapToEntity(Dictionary<string, AttributeValue> item)
        {
            return new User
            {
                Id = item[nameof(User.Id)].S,
                Name = item[nameof(User.Name)].S,
                Email = item[nameof(User.Email)].S,
                PhoneNumber = item[nameof(User.PhoneNumber)].S,
                PreferredChannel = Enum.Parse<NotificationChannel>(item[nameof(User.PreferredChannel)].S),
                Balance = decimal.Parse(item[nameof(User.Balance)].N),
                LastUpdated = DateTime.Parse(item[nameof(User.LastUpdated)].S)
            };
        }

        private static Dictionary<string, AttributeValue> MapToItem(User user) =>
            new()
            {
                [nameof(User.Id)] = new AttributeValue(user.Id),
                [nameof(User.Name)] = new AttributeValue(user.Name),
                [nameof(User.Email)] = new AttributeValue(user.Email),
                [nameof(User.PhoneNumber)] = new AttributeValue(user.PhoneNumber),
                [nameof(User.PreferredChannel)] = new AttributeValue(user.PreferredChannel.ToString()),
                [nameof(User.Balance)] = new AttributeValue { N = user.Balance.ToString() },
                [nameof(User.LastUpdated)] = new AttributeValue { S = user.LastUpdated.ToString("o") }
            };
    }
}
