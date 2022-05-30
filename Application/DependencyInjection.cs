using Application.Authentication.Commands;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddMediatR(typeof(LoginUserCommand.Handler).Assembly);
            services.AddAutoMapper(typeof(LoginUserCommand.Handler));
            services.AddHttpClient();

            return services;
        }
    }
}