using System.Text.Json.Serialization;

namespace InvestmentFundManager.Domain.Enum
{
    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum FundCategory
    {
        FPV,
        FIC
    }
}
