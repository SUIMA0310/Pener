using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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

        public JwtService(
            JwtSecurityTokenHandler handler,
            IOptions<JwtServiceConfig> options)
        {
            _handler = handler;
            _config = options.Value;
        }

        public virtual string CreateToken(User user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.NameId, user.Id),
                new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName),
                new Claim(JwtRegisteredClaimNames.Jti, GetJti())
            };

            return CreateToken(
                issuer: _config.Issuer,
                audience: _config.Audience,
                claims: claims,
                notBefore: DateTime.Now,
                expires: DateTime.Now.AddMinutes(_config.ExpireTime)
            );
        }

        protected virtual string CreateToken(
            string issuer, 
            string audience, 
            IEnumerable<Claim> claims, 
            DateTime? notBefore, 
            DateTime? expires)
        {
            var token = new JwtSecurityToken(
                header: new JwtHeader(GetCredentials()),
                payload: new JwtPayload(issuer, audience, claims, notBefore, expires));

            return _handler.WriteToken(token);
        }

        /// <summary>
        /// SecurityKeyÇê∂ê¨Ç∑ÇÈ
        /// </summary>
        /// <returns>SecurityKey</returns>
        protected static SecurityKey GetSecurityKey(string baseKey)
        {
            if (string.IsNullOrEmpty(baseKey))
            {
                return null;
            }

            var bytes = Encoding.UTF8.GetBytes(baseKey);
            return new SymmetricSecurityKey(bytes);
        }

        /// <summary>
        /// SecurityKeyÇ©ÇÁSigningCredentialsÇê∂ê¨Ç∑ÇÈ
        /// </summary>
        /// <returns>SigningCredentials</returns>
        protected virtual SigningCredentials GetCredentials()
        {
            var key = GetSecurityKey(_config.SigningKey)??throw new Exception("KeyÇÃçÏê¨Ç…é∏îs");
            return new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        }

        public static TokenValidationParameters GetValidationParameters(IConfiguration config)
        {
            var issuer = config[$"{CONFIG_SECTION_NAME}:{nameof(JwtServiceConfig.Issuer)}"];
            var audience = config[$"{CONFIG_SECTION_NAME}:{nameof(JwtServiceConfig.Audience)}"];
            var signingKey = config[$"{CONFIG_SECTION_NAME}:{nameof(JwtServiceConfig.SigningKey)}"];

            return new TokenValidationParameters()
            {
                ValidateLifetime = true,

                ValidateIssuer = issuer != null,
                ValidIssuer = issuer,

                ValidateAudience = audience != null,
                ValidAudience = audience,

                ValidateIssuerSigningKey = signingKey != null,
                IssuerSigningKey = GetSecurityKey(signingKey)
            };
        }

        private string GetJti()
        {
            return ToHexString(GetRandomBytes());
        }

        private byte[] GetRandomBytes()
        {
            var random = new RNGCryptoServiceProvider();
            var bytes = new byte[16];

            // get random bytes.
            random.GetBytes(bytes);

            return bytes;
        }

        private string ToHexString(IEnumerable<byte> bytes)
        {
            var capacity = bytes.Count() * 2;
            var bilder = new StringBuilder(capacity);

            foreach (var b in bytes)
            {
                bilder.AppendFormat("{0:x2}", b);
            }

            return bilder.ToString();
        }
    }
}