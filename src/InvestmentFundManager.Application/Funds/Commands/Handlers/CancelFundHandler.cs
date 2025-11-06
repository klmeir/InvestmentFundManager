using InvestmentFundManager.Domain.Entities;
using InvestmentFundManager.Domain.Services;
using MediatR;

namespace InvestmentFundManager.Application.Funds.Commands.Handlers
{
    public class CancelFundHandler : IRequestHandler<CancelFundCommand, string>
    {
        private readonly FundsService _fundsService;

        public CancelFundHandler(FundsService fundsService)
        {
            _fundsService = fundsService ?? throw new ArgumentNullException(nameof(fundsService));
        }

        public async Task<string> Handle(CancelFundCommand command, CancellationToken cancellationToken)
        {
            // Map command data to domain entity
            var transaction = new FundTransaction
            {
                Id = Guid.NewGuid().ToString(),
                Fund = command.FundId
            };

            // Delegate to domain service
            return await _fundsService.CancelAsync(transaction);
        }
    }
}
