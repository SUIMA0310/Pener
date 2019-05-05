using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Pener.Server.Services.Jwt
{
    public static class JwtServiceExpansion
    {
        public static IServiceCollection AddJwtService(
            this IServiceCollection services,
            IConfiguration config)
        {
            // add config.
            services.Configure<JwtServiceConfig>(config.GetSection(JwtService.CONFIG_SECTION_NAME));

            // add modules.
            services.AddScoped<JwtSecurityTokenHandler>();

            // add service.
            services.AddScoped<IJwtService, JwtService>();

            return services;
        }

    }
}
