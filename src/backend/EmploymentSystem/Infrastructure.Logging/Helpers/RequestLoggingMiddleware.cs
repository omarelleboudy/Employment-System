using Infrastructure.Logging.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Infrastructure.Logging.Helpers
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<RequestLoggingMiddleware> _logger;
        private const string _correlationIdHeader = "CorrelationId";


        public RequestLoggingMiddleware(RequestDelegate next, ILogger<RequestLoggingMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task Invoke(HttpContext context, ICorrelationIdGenerator correlationIdGenerator)
        {
            var correlationId = GetCorrelationIdFromHeader(context, correlationIdGenerator);

            AddCorrelationIdToResponse(context, correlationId);
            // scope on everything in the pipeline using correlationId
            using (_logger.BeginScope(GetScopeInformation(correlationId)))
            {
                try
                {
                    _logger.LogInformation("Request with CorrelationId: " + correlationId + "  Started At: {0}", DateTime.UtcNow);
                   
                    await _next(context);
                   
                    _logger.LogInformation("Request with CorrelationId: " + correlationId + " Finished At: {0}", DateTime.UtcNow);

                }
                catch (Exception ex)
                {
                    context.Response.StatusCode = StatusCodes.Status500InternalServerError;
                    _logger.LogError(ex.ToString());
                }
            }
        }

        private static void AddCorrelationIdToResponse(HttpContext context, string correlationId)
        {
            context.Response.OnStarting(() =>
            {
                context.Response.Headers.Add(_correlationIdHeader, new[] { correlationId });
                return Task.CompletedTask;
            });
        }

        private static string GetCorrelationIdFromHeader(HttpContext context, ICorrelationIdGenerator correlationIdGenerator)
        {
            if (context.Request.Headers.TryGetValue(_correlationIdHeader, out var correlationId))
            {
                correlationIdGenerator.SetCorrelationId(correlationId);
                return correlationId;
            }
            context.Request.Headers.Add(_correlationIdHeader, correlationId);
            return correlationIdGenerator.GetCorrelationId();
        }
        private Dictionary<string, object> GetScopeInformation(string correlationId)
        {
            var scopeInfo = new Dictionary<string, object>();
            scopeInfo.Add("CorrelationId", correlationId);
            scopeInfo.Add("MachineName", Environment.MachineName);
            scopeInfo.Add("Environment", "Development");
            scopeInfo.Add("AppName", "Employment System");
            return scopeInfo;
        }
    }
}
