
using Amazon.DynamoDBv2;
using Amazon.SimpleNotificationService;
using FluentValidation;
using InvestmentFundManager.Api.Middleware;
using InvestmentFundManager.Api.Validators;
using InvestmentFundManager.Application;
using InvestmentFundManager.Application.Funds.Commands;
using InvestmentFundManager.Domain.Entities;
using InvestmentFundManager.Domain.Ports;
using InvestmentFundManager.Domain.Services;
using InvestmentFundManager.Infrastructure.Adapters;
using InvestmentFundManager.Infrastructure.Config;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using System;
using System.Text.Json.Serialization;

namespace InvestmentFundManager.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // -----------------------------
            // Load configuration
            // -----------------------------
            builder.Configuration
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddEnvironmentVariables();

            // -----------------------------
            // AWS Settings
            // -----------------------------
            //builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AwsSettings"));
            //var awsSettings = builder.Configuration.GetSection("AwsSettings").Get<AwsSettings>();
            // ? Bind AwsSettings from configuration
            builder.Services.Configure<AwsSettings>(builder.Configuration.GetSection("AwsSettings"));
            builder.Services.AddSingleton(sp =>
                sp.GetRequiredService<IOptions<AwsSettings>>().Value);

            // -----------------------------
            // AWS Clients Configuration
            // -----------------------------
            builder.Services.AddSingleton<IAmazonDynamoDB>(sp =>
            {
                var awsSettings = sp.GetRequiredService<AwsSettings>();
                var config = new AmazonDynamoDBConfig { ServiceURL = awsSettings.ServiceURL };
                return new AmazonDynamoDBClient(
                    awsSettings.AccessKey,
                    awsSettings.SecretKey,
                    config
                );
            });

            builder.Services.AddSingleton<IAmazonSimpleNotificationService>(sp =>
            {
                var awsSettings = sp.GetRequiredService<AwsSettings>();
                var config = new AmazonSimpleNotificationServiceConfig { ServiceURL = awsSettings.ServiceURL };
                return new AmazonSimpleNotificationServiceClient(
                    awsSettings.AccessKey,
                    awsSettings.SecretKey,
                    config
                );
            });

            // -----------------------------
            // Dependency Injection
            // -----------------------------
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IFundRepository, FundRepository>();
            builder.Services.AddScoped<ITransactionRepository, TransactionRepository>();
            builder.Services.AddScoped<INotificationService, NotificationService>();
            builder.Services.AddScoped<FundsService>();


            // -----------------------------
            // MediatR (CQRS Handlers)
            // -----------------------------
            builder.Services.AddMediatR(cfg =>
            {
                cfg.RegisterServicesFromAssembly(typeof(ApplicationAssemblyMarker).Assembly);
            });

            // -----------------------------
            // Controllers + Swagger
            // -----------------------------
            builder.Services.AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
                });


            builder.Services.AddScoped<IValidator<SubscribeFundCommand>, SubscribeFundCommandValidator>();
            builder.Services.AddScoped<IValidator<CancelFundCommand>, CancelFundCommandValidator>();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(opt =>
            {
                opt.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Investment Fund Manager API",
                    Version = "v1",
                    Description = "API for managing fund subscriptions, cancellations and transaction history.",
                    Contact = new OpenApiContact
                    {
                        Name = "Carlos Meriño",
                        Email = "caalmeir@gmail.com",
                        Url = new Uri("https://github.com/klmeir")
                    }
                });
            });

            var app = builder.Build();

            // -----------------------------
            // Middleware
            // -----------------------------
            if (!app.Environment.IsDevelopment())
            {
                app.UseHttpsRedirection();
            }

            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "Investment Fund Manager API v1");
                c.RoutePrefix = string.Empty;
            });

            app.MapControllers();

            app.UseMiddleware<AppExceptionHandlerMiddleware>();

            // -----------------------------
            // Run the app
            // -----------------------------
            app.Run();
        }
    }
}
