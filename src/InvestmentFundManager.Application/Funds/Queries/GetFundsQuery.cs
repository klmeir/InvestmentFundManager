using InvestmentFundManager.Domain.Entities;
using MediatR;

namespace InvestmentFundManager.Application.Funds.Queries
{
    public record GetFundsQuery() : IRequest<List<Fund>>;
}
