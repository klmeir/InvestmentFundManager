using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using InvestmentFundManager.Domain.Entities;
using InvestmentFundManager.Domain.Enum;
using InvestmentFundManager.Domain.Exceptions;
using InvestmentFundManager.Domain.Ports;
using InvestmentFundManager.Infrastructure.Config;
using Microsoft.Extensions.Logging;

namespace InvestmentFundManager.Infrastructure.Adapters
{
    /// <summary>
    /// Defines an abstraction for sending user notifications.
    /// Implementations may use Amazon SNS, email services, or other messaging systems.
    /// </summary>
    public class NotificationService : INotificationService
    {
        private readonly IAmazonSimpleNotificationService _snsClient;
        private readonly ILogger<NotificationService> _logger;
        private readonly AwsSettings _settings;

        public NotificationService(
            IAmazonSimpleNotificationService snsClient,
            AwsSettings settings,
            ILogger<NotificationService> logger)
        {
            _snsClient = snsClient ?? throw new ArgumentNullException(nameof(snsClient));
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task NotifyAsync(FundTransaction model, string message)
        {
            string formattedMessage = $"{message}\nFund: {model.Fund}\nDate: {model.Date:dd/MM/yyyy HH:mm:ss}";

            switch (model.NotificationChannel)
            {
                case NotificationChannel.EMAIL:
                    await SendEmailAsync(model.Recipient, "Fund Notification", formattedMessage);
                    break;

                case NotificationChannel.SMS:
                    await SendSmsAsync(model.Recipient, formattedMessage);
                    break;

                default:
                    throw new CoreBusinessException("Unsupported notification channel.");
            }
        }

        /// <summary>
        /// Sends an email notification to a recipient.
        /// </summary>
        /// <param name="to">Recipient email address.</param>
        /// <param name="subject">Email subject line.</param>
        /// <param name="body">Email body content.</param>
        private async Task SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                if (string.IsNullOrEmpty(_settings.SnsEmailTopicArn))
                    throw new InvalidOperationException("SNS Email Topic ARN no configurado.");

                var message = $"Subject: {subject}\n\n{body}";

                await _snsClient.PublishAsync(new PublishRequest
                {
                    TopicArn = _settings.SnsEmailTopicArn,
                    Message = message,
                    Subject = subject
                });

                _logger.LogInformation("📧 Email sent successfully to {Recipient}", to);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to send email to {Recipient}", to);
                throw;
            }
        }

        /// <summary>
        /// Sends an SMS notification to a recipient.
        /// </summary>
        /// <param name="phoneNumber">Recipient phone number (E.164 format).</param>
        /// <param name="message">Message body.</param>
        private async Task SendSmsAsync(string phoneNumber, string message)
        {
            try
            {
                await _snsClient.PublishAsync(new PublishRequest
                {
                    Message = message,
                    PhoneNumber = phoneNumber
                });

                _logger.LogInformation("📱 SMS sent successfully to {PhoneNumber}", phoneNumber);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Failed to send SMS to {PhoneNumber}", phoneNumber);
                throw;
            }
        }
    }
}
