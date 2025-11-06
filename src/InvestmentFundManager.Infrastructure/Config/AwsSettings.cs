namespace InvestmentFundManager.Infrastructure.Config
{
    /// <summary>
    /// Represents AWS configuration values for DynamoDB, SNS, and other services.
    /// Populated via configuration binding (e.g., appsettings.json or environment variables).
    /// </summary>
    public class AwsSettings
    {
        /// <summary>
        /// Gets or sets the base AWS service endpoint.
        /// Used for LocalStack or custom AWS endpoints.
        /// Example: http://localstack:4566
        /// </summary>
        public string ServiceURL { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the DynamoDB table name for fund transactions.
        /// Example: FundsTransactions
        /// </summary>
        public string DynamoFundsTransactionsTableName { get; set; } = "FundsTransactions";
        public string DynamoUserTableName { get; set; } = "Users";
        public string DynamoFundTableName { get; set; } = "Funds"; 

        /// <summary>
        /// Gets or sets the SNS topic ARN for email notifications.
        /// Example: arn:aws:sns:us-east-1:123456789012:email-topic
        /// </summary>
        public string SnsEmailTopicArn { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the SNS topic ARN for SMS notifications.
        /// Example: arn:aws:sns:us-east-1:123456789012:sms-topic
        /// </summary>
        public string SnsSmsTopicArn { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the default AWS region.
        /// Example: us-east-1
        /// </summary>
        public string Region { get; set; } = "us-east-1";

        /// <summary>
        /// Gets or sets the AWS access key ID (used mainly for local or CI/CD environments).
        /// </summary>
        public string AccessKey { get; set; } = "test";

        /// <summary>
        /// Gets or sets the AWS secret access key (used mainly for local or CI/CD environments).
        /// </summary>
        public string SecretKey { get; set; } = "test";
    }
}
