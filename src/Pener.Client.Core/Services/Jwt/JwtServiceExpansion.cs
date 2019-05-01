using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pener.Client.Services.Jwt
{
    public static class JwtServiceExpansion
    {
        private const string SECTION_KEY = "Jwt";

        public static IServiceCollection AddJwtService(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.AddJwtServiceCore(config);
            services.AddFileJwtStore(config);

            return services;
        }

            public static IServiceCollection AddJwtServiceCore(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.Configure<JwtServiceConfig>(config.GetSection(SECTION_KEY));
            services.AddTransient<IJwtService, JwtService>();

            services.AddTransient<JwtSecurityTokenHandler, MyJwtSecurityTokenHandler>();

            return services;
        }

        public static IServiceCollection AddFileJwtStore(
            this IServiceCollection services,
            IConfiguration config)
        {
            services.Configure<FileJwtStoreConfig>(config.GetSection(SECTION_KEY));
            services.AddTransient<IJwtStore, FileJwtStore>();

            return services;
        }
    }
}