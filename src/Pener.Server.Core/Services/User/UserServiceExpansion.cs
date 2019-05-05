using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pener.Services.User
{
    public static class UserServiceExpansion
    {
        public static IServiceCollection AddUserService(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddScoped<IUserService, UserService>(); 

            return services;
        }
    }
}