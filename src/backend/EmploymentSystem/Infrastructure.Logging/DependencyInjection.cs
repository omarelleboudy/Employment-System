using Infrastructure.Logging.Configuration;
using Infrastructure.Logging.Core;
using Infrastructure.Logging.Helpers;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure.Logging
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddCorrelationIdManager(this IServiceCollection services)
        {
            services.AddScoped<ICorrelationIdGenerator, CorrelationIdGenerator>();
            return services;
        }
        public static void UseLoggingMiddleware(this IApplicationBuilder builder)
        {
            builder.UseMiddleware<RequestLoggingMiddleware>();
        }

    }
}

