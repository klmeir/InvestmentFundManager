
using InvestmentFundManager.Domain.Entities;
using InvestmentFundManager.Domain.Enum;
using InvestmentFundManager.Domain.Exceptions;
using InvestmentFundManager.Domain.Ports;

namespace InvestmentFundManager.Domain.Services
{
    /// <summary>
    /// Implements fund operations such as subscriptions, cancellations, and transaction retrieval.
    /// Updates user balance and triggers notifications after each operation.
    /// </summary>
    [DomainService]
    public class FundsService
    {
        private readonly ITransactionRepository _transactionRepository;
        private readonly IFundRepository _fundRepository;
        private readonly IUserRepository _userRepository;
        private readonly INotificationService _notificationService;

        public FundsService(
            ITransactionRepository transactionRepository,
            IFundRepository fundRepository,
            IUserRepository userRepository,
            INotificationService notificationService)
        {
            _transactionRepository = transactionRepository;
            _fundRepository = fundRepository;
            _userRepository = userRepository;
            _notificationService = notificationService;
        }

        /// <summary>
        /// Handles user subscription to a fund and updates balance.
        /// </summary>
        public async Task<string> SubscribeAsync(FundTransaction model)
        {
            var user = await GetUserAsync();

            if (user == null)
                throw new CoreBusinessException($"Default User not found.");


            var fund = await _fundRepository.GetFundByIdAsync(model.FundId);

            if (fund == null)
                throw new CoreBusinessException($"Fund '{model.FundId}' not found.");

            var subscriptionTransaction = await _transactionRepository.GetActiveSubscriptionAsync(user.Id, fund.Id);

            if (subscriptionTransaction != null)
                throw new CoreBusinessException($"You already have an active subscription for this fund. '{fund.Name}'.");

            if (user.Balance < fund.MinimumAmount)
                throw new CoreBusinessException($"No tiene saldo disponible para vincularse al fondo {fund.Name}");

            user.Balance -= fund.MinimumAmount;
            user.LastUpdated = DateTime.UtcNow;

            model.Fund = fund.Name;
            model.Amount = fund.MinimumAmount;
            model.Type = TransactionType.SUBSCRIPTION;
            model.Status = TransactionStatus.COMPLETED;
            model.Date = DateTime.UtcNow;
            model.BalanceAfterTransaction = user.Balance;
            model.Recipient = model.NotificationChannel == NotificationChannel.EMAIL ? user.Email : user.PhoneNumber;
            model.User = user.Id;

            await _transactionRepository.RegisterTransactionAsync(model);
            await _userRepository.UpdateUserAsync(user);

            await _notificationService.NotifyAsync(
                model,
                $"User {user.Name} subscribed to fund {fund.Name} successfully."
            );

            return model.Id;
        }

        /// <summary>
        /// Handles fund cancellation and restores user's balance.
        /// </summary>
        public async Task<string> CancelAsync(FundTransaction model)
        {
            var user = await GetUserAsync();

            if (user == null)
                throw new CoreBusinessException($"Default User not found.");

            var fund = await _fundRepository.GetFundByIdAsync(model.FundId);

            if (fund == null)
                throw new CoreBusinessException($"Fund '{model.FundId}' not found.");

            var activeSubscriptionTransaction = await _transactionRepository.GetActiveSubscriptionAsync(user.Id, fund.Id);

            if (activeSubscriptionTransaction == null)
                throw new CoreBusinessException($"No active subscription found to cancel for fund '{fund.Name}'.");

            user.Balance += fund.MinimumAmount;
            user.LastUpdated = DateTime.UtcNow;

            model.Fund = fund.Name;
            model.Amount = fund.MinimumAmount;
            model.Type = TransactionType.CANCELLATION;
            model.Status = TransactionStatus.COMPLETED;
            model.Date = DateTime.UtcNow;
            model.BalanceAfterTransaction = user.Balance;

            model.NotificationChannel = activeSubscriptionTransaction.NotificationChannel;
            model.Recipient = activeSubscriptionTransaction.Recipient;
            model.User = user.Id;            

            await _transactionRepository.RegisterTransactionAsync(model);
            await _userRepository.UpdateUserAsync(user);
            
            await _transactionRepository.UpdateTransactionStatusAsync(activeSubscriptionTransaction.Id, TransactionStatus.CANCELED);

            await _notificationService.NotifyAsync(
                model,
                $"User {user.Name} canceled subscription to fund {fund.Name}. The amount was refunded."
            );

            return model.Id;
        }

        /// <summary>
        /// Returns all recorded funds.
        /// </summary>
        public async Task<List<Fund>> ListFundsAsync()
        {
            return await _fundRepository.ListFundsAsync();
        }

        /// <summary>
        /// Returns all recorded fund transactions.
        /// </summary>
        public async Task<List<FundTransaction>> ListTransactionsAsync()
        {
            return await _transactionRepository.ListTransactionsAsync();
        }

        /// <summary>
        /// Retrieves a default user if not found.
        /// </summary>
        private async Task<User> GetUserAsync()
        {
            return await _userRepository.GetUserAsync();
        }

    }
}

