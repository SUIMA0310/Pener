using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Pener.Client.Services.Jwt;
using Microsoft.Extensions.Options;

namespace Pener.Client.Services.Auth
{
    public static class AuthServiceExpansion
    {
        public static IServiceCollection AddAuthService(
            this IServiceCollection services,
            IConfiguration config)
        {
            // add config.
            services.Configure<AuthServiceConfig>(config);

            // add auth services.
            services.AddScoped<IAuthService, AuthService>();

            return services;
        }

        public static IServiceCollection AddDefaultHttpClient(
            this IServiceCollection services)
        {
            return services.AddHttpClient(new HttpClientHandler());
        }

        public static IServiceCollection AddHttpClient(
            this IServiceCollection services,
            HttpMessageHandler innerHandler)
        {
            services.AddSingleton(x => 
            {
                var jwtService = x.GetService<IJwtService>();
                var option = x.GetService<IOptions<AuthServiceConfig>>();
                var handler = new AuthDelegatingHandler(jwtService, innerHandler);

                return new HttpClient(handler)
                {
                    BaseAddress = option.Value.ServerAddress
                };
            });

            return services;
        }
    }
}
