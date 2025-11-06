using InvestmentFundManager.Domain.Entities;
using MediatR;

namespace InvestmentFundManager.Application.Funds.Queries
{
    public record GetTransactionsQuery() : IRequest<List<FundTransaction>>;
}
