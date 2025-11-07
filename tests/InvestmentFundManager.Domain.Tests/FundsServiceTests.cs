using InvestmentFundManager.Domain.Entities;
using InvestmentFundManager.Domain.Enum;
using InvestmentFundManager.Domain.Exceptions;
using InvestmentFundManager.Domain.Ports;
using InvestmentFundManager.Domain.Services;
using Moq;

namespace InvestmentFundManager.Domain.Tests
{
    public class FundsServiceTests
    {
        private readonly Mock<ITransactionRepository> _transactionRepoMock;
        private readonly Mock<IFundRepository> _fundRepoMock;
        private readonly Mock<IUserRepository> _userRepoMock;
        private readonly Mock<INotificationService> _notificationMock;
        private readonly FundsService _service;

        public FundsServiceTests()
        {
            _transactionRepoMock = new Mock<ITransactionRepository>();
            _fundRepoMock = new Mock<IFundRepository>();
            _userRepoMock = new Mock<IUserRepository>();
            _notificationMock = new Mock<INotificationService>();

            _userRepoMock.Setup(u => u.GetUserAsync())
                .ReturnsAsync(new User { Id = "default-user", Email = "user@email.com", PhoneNumber = "+573001234567", Balance = 500000m });

            _fundRepoMock.Setup(f => f.GetFundByIdAsync(It.IsAny<string>()))
                .ReturnsAsync((string id) => new Fund("1", "FPV_BTG_PACTUAL_RECAUDADORA", 75000m, "FPV"));

            _service = new FundsService(
                _transactionRepoMock.Object,
                _fundRepoMock.Object,
                _userRepoMock.Object,
                _notificationMock.Object
            );
        }

        [Fact(DisplayName = "SubscribeAsync should register a transaction and send email when funds are sufficient")]
        public async Task SubscribeAsync_Should_Register_Transaction_And_Notify_Email()
        {
            // Arrange
            var transaction = new FundTransaction
            {
                Id = Guid.NewGuid().ToString(),
                User = "default-user",
                Fund = "1",
                Recipient = "user@email.com",
                NotificationChannel = NotificationChannel.EMAIL
            };
            _transactionRepoMock.Setup(t => t.GetActiveSubscriptionAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((FundTransaction)null);

            // Act
            var result = await _service.SubscribeAsync(transaction);

            // Assert
            Assert.Equal(transaction.Id, result);

            _transactionRepoMock.Verify(r => r.RegisterTransactionAsync(It.IsAny<FundTransaction>()), Times.Once);
            _notificationMock.Verify(n => n.NotifyAsync(It.IsAny<FundTransaction>(), It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName = "SubscribeAsync should throw when user has insufficient funds")]
        public async Task SubscribeAsync_Should_Throw_When_Insufficient_Balance()
        {
            // Arrange
            _userRepoMock.Setup(u => u.GetUserAsync())
                .ReturnsAsync(new User { Balance = 20000m }); // Low balance

            var transaction = new FundTransaction
            {
                Id = Guid.NewGuid().ToString(),
                User = "low-user",
                Fund = "1",
                Recipient = "low@email.com",
                NotificationChannel = NotificationChannel.EMAIL
            };

            // Act & Assert
            await Assert.ThrowsAsync<CoreBusinessException>(() => _service.SubscribeAsync(transaction));

            _transactionRepoMock.Verify(r => r.RegisterTransactionAsync(It.IsAny<FundTransaction>()), Times.Never);
        }

        [Fact(DisplayName = "CancelAsync should register cancellation and send SMS notification")]
        public async Task CancelAsync_Should_Register_Cancellation_And_Notify_SMS()
        {
            // Arrange
            var transaction = new FundTransaction
            {
                Id = Guid.NewGuid().ToString(),
                User = "default-user",
                Fund = "1",
                Recipient = "+573001234567",
                NotificationChannel = NotificationChannel.SMS
            };
            _transactionRepoMock.Setup(t => t.GetActiveSubscriptionAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(transaction);

            // Act
            var result = await _service.CancelAsync(transaction);

            // Assert
            Assert.Equal(transaction.Id, result);

            _transactionRepoMock.Verify(r => r.RegisterTransactionAsync(It.IsAny<FundTransaction>()), Times.Once);
            _notificationMock.Verify(n => n.NotifyAsync(It.IsAny<FundTransaction>(), It.IsAny<string>()), Times.Once);
        }

        [Fact(DisplayName = "SubscribeAsync should call NotifyAsync with EMAIL channel")]
        public async Task SubscribeAsync_Should_Call_NotifyAsync_When_Channel_Is_Email()
        {
            // Arrange
            var transaction = new FundTransaction
            {
                Id = Guid.NewGuid().ToString(),
                User = "default-user",
                Fund = "1",
                Recipient = "user@email.com",
                NotificationChannel = NotificationChannel.EMAIL
            };
            _transactionRepoMock.Setup(t => t.GetActiveSubscriptionAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((FundTransaction)null);

            // Act
            await _service.SubscribeAsync(transaction);

            // Assert
            _notificationMock.Verify(n =>
                n.NotifyAsync(
                    It.Is<FundTransaction>(t => t.NotificationChannel == NotificationChannel.EMAIL),
                    It.Is<string>(msg => msg.Contains("subscribed"))),
                Times.Once);
        }

        [Fact(DisplayName = "SubscribeAsync should call NotifyAsync with SMS channel")]
        public async Task SubscribeAsync_Should_Call_NotifyAsync_When_Channel_Is_Sms()
        {
            // Arrange
            var transaction = new FundTransaction
            {
                Id = Guid.NewGuid().ToString(),
                User = "default-user",
                Fund = "1",
                Recipient = "+573001234567",
                NotificationChannel = NotificationChannel.SMS
            };
            _transactionRepoMock.Setup(t => t.GetActiveSubscriptionAsync(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((FundTransaction)null);

            // Act
            await _service.SubscribeAsync(transaction);

            // Assert
            _notificationMock.Verify(n =>
                n.NotifyAsync(
                    It.Is<FundTransaction>(t => t.NotificationChannel == NotificationChannel.SMS),
                    It.Is<string>(msg => msg.Contains("subscribed"))),
                Times.Once);
        }

    }
}
