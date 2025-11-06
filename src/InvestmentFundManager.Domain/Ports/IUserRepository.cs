using InvestmentFundManager.Domain.Entities;

namespace InvestmentFundManager.Domain.Ports
{
    public interface IUserRepository
    {
        Task<User?> GetUserAsync();
        Task UpdateUserAsync(User user);
    }
}
