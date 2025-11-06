using System.Text.Json.Serialization;

namespace InvestmentFundManager.Domain.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum TransactionType
    {
        SUBSCRIPTION,
        CANCELLATION
    }
}
