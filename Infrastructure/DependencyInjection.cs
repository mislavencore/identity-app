using Infrastructure.EmailProvider;
using Infrastructure.ExternalRequests;
using Infrastructure.WebToken;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services)
        {
            services.AddSingleton<IWebTokenService, WebTokenService>();
            services.AddSingleton<IExternalRequests, ExternalRequestsExecution>();
            services.AddSingleton<IEmailSender, EmailSender>();

            return services;
        }
    }
}