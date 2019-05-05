using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pener.Server.Services.Auth
{
    public static class AuthServiceExpansion
    {
        public static IServiceCollection AddAuthService(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }
    }
}