using InvestmentFundManager.Domain.Entities;
using InvestmentFundManager.Domain.Services;
using MediatR;

namespace InvestmentFundManager.Application.Funds.Queries.Handlers
{
    public class GetTransactionsHandler : IRequestHandler<GetTransactionsQuery, List<FundTransaction>>
    {
        private readonly FundsService _fundsService;

        public GetTransactionsHandler(FundsService fundsService)
        {
            _fundsService = fundsService ?? throw new ArgumentNullException(nameof(fundsService));
        }

        public async Task<List<FundTransaction>> Handle(GetTransactionsQuery request, CancellationToken cancellationToken)
        {
            return await _fundsService.ListTransactionsAsync();
        }
    }
}
