using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Pener.Services.User;

namespace Pener.Server.Services.Jwt
{

    public class JwtService : IJwtService
    {
        public const string CONFIG_SECTION_NAME = "Jwt";

        private readonly JwtSecurityTokenHandler _handler;
        private readonly JwtServiceConfig _config;

        public virtual string CreateToken(IUser user)
        {
            var claims = new List<Claim>();

            claims.AddNameId(user.Id);
            claims.AddUniqueName(user.UserName);

            claims.AddAudiences(_config.Audiences);
            claims.AddIssuer(_config.Issuer);

            claims.AddJti();

            claims.AddExpire(_config.ExpireTime);
            claims.AddNotbefore();

            return CreateToken(claims);
        }

        protected virtual string CreateToken(IEnumerable<Claim> claims)
        {
            var token = new JwtSecurityToken(
                header: new JwtHeader(GetCredentials()),
                payload: new JwtPayload(claims));

            return _handler.WriteToken(token);
        }

        /// <summary>
        /// SecurityKeyÇê∂ê¨Ç∑ÇÈ
        /// </summary>
        /// <returns>SecurityKey</returns>
        protected virtual SecurityKey GetSecurityKey()
        {
            var bytes = Encoding.UTF8.GetBytes(_config.SigningKey);
            return new SymmetricSecurityKey(bytes);
        }

        /// <summary>
        /// SecurityKeyÇ©ÇÁSigningCredentialsÇê∂ê¨Ç∑ÇÈ
        /// </summary>
        /// <returns>SigningCredentials</returns>
        protected virtual SigningCredentials GetCredentials()
        {
            var key = GetSecurityKey();
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        }

        public static TokenValidationParameters GetValidationParameters(IConfiguration configuration)
        {
            var config = configuration.GetValue<JwtServiceConfig>(CONFIG_SECTION_NAME);

            return new TokenValidationParameters()
            {
                ValidateLifetime = true,

                ValidateIssuer = config?.Issuer != null,
                ValidIssuer = config?.Issuer,

                ValidateAudience = config?.Audiences?.Any() ?? false,
                ValidAudiences = config?.Audiences
            };
        }
    }
}