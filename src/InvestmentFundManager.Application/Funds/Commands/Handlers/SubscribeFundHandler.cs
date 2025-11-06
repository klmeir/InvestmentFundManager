using InvestmentFundManager.Domain.Entities;
using InvestmentFundManager.Domain.Services;
using MediatR;

namespace InvestmentFundManager.Application.Funds.Commands.Handlers
{
    public class SubscribeFundHandler : IRequestHandler<SubscribeFundCommand, string>
    {
        private readonly FundsService _fundsService;

        public SubscribeFundHandler(FundsService fundsService)
        {
            _fundsService = fundsService ?? throw new ArgumentNullException(nameof(fundsService));
        }

        public async Task<string> Handle(SubscribeFundCommand command, CancellationToken cancellationToken)
        {            
            var transaction = new FundTransaction
            {
                Id = Guid.NewGuid().ToString(),
                Fund = command.FundId,
                NotificationChannel = command.NotificationChannel
            };
            
            return await _fundsService.SubscribeAsync(transaction);
        }
    }
}
