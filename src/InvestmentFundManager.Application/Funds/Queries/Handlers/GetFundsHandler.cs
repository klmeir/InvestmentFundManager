using InvestmentFundManager.Domain.Entities;
using InvestmentFundManager.Domain.Services;
using MediatR;

namespace InvestmentFundManager.Application.Funds.Queries.Handlers
{
    public class GetFundsHandler : IRequestHandler<GetFundsQuery, List<Fund>>
    {
        private readonly FundsService _fundsService;

        public GetFundsHandler(FundsService fundsService)
        {
            _fundsService = fundsService ?? throw new ArgumentNullException(nameof(fundsService));
        }

        public async Task<List<Fund>> Handle(GetFundsQuery request, CancellationToken cancellationToken)
        {
            return await _fundsService.ListFundsAsync();
        }
    }
}
