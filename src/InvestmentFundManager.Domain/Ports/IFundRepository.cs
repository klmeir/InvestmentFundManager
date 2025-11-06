using InvestmentFundManager.Domain.Entities;

namespace InvestmentFundManager.Domain.Ports
{    
    public interface IFundRepository
    {
        Task<Fund?> GetFundByIdAsync(string fundId);
        Task<List<Fund>> ListFundsAsync();
    }
}
