using InvestmentFundManager.Domain.Exceptions;
using System.Net;

namespace InvestmentFundManager.Api.Middleware
{
    public class AppExceptionHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<AppExceptionHandlerMiddleware> _logger;

        public AppExceptionHandlerMiddleware(RequestDelegate next, ILogger<AppExceptionHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next.Invoke(context);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);

                var result = System.Text.Json.JsonSerializer.Serialize(new
                {
                    ErrorMessage = ex.Message
                });

                context.Response.ContentType = "application/json";
                context.Response.StatusCode =
                    (ex is CoreBusinessException) ? (int)HttpStatusCode.BadRequest : (int)HttpStatusCode.InternalServerError;
                await context.Response.WriteAsync(result);
            }
        }
    }
}
