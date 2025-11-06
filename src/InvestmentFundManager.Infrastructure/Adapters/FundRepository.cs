using Amazon.DynamoDBv2;
using Amazon.DynamoDBv2.Model;
using InvestmentFundManager.Domain.Entities;
using InvestmentFundManager.Domain.Ports;
using InvestmentFundManager.Infrastructure.Config;
using Microsoft.Extensions.Options;

namespace InvestmentFundManager.Infrastructure.Adapters
{
    public class FundRepository : IFundRepository
    {

        private readonly IAmazonDynamoDB _dynamoDb;
        private readonly AwsSettings _settings;

        public FundRepository(IAmazonDynamoDB dynamoDb, IOptions<AwsSettings> settings)
        {
            _dynamoDb = dynamoDb ?? throw new ArgumentNullException(nameof(dynamoDb));
            _settings = settings?.Value ?? throw new ArgumentNullException(nameof(settings));
        }

        public async Task<Fund?> GetFundByIdAsync(string fundId)
        {            
            var response = await _dynamoDb.GetItemAsync(new GetItemRequest
            {
                TableName = _settings.DynamoFundTableName,
                Key = new Dictionary<string, AttributeValue> { { nameof(Fund.Id), new AttributeValue(fundId) } }
            });

            return response.Item != null && response.Item.Count > 0 ? MapFund(response.Item) : null;
        }

        public async Task<List<Fund>> ListFundsAsync()
        {
            var response = await _dynamoDb.ScanAsync(new ScanRequest
            {
                TableName = _settings.DynamoFundTableName
            });

            return response.Items.Select(MapFund).ToList();
        }

        private static Fund MapFund(Dictionary<string, AttributeValue> item)
        {
            return new Fund(item[nameof(Fund.Id)].S,
                item[nameof(Fund.Name)].S,
                decimal.Parse(item[nameof(Fund.MinimumAmount)].N),
                item[nameof(Fund.Category)].S
            );
        }
    }
}
