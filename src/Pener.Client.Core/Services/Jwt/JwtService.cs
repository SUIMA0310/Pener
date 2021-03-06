using System;
using System.IdentityModel.Tokens.Jwt;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace Pener.Client.Services.Jwt
{

    public class JwtService : IJwtService
    {
        private readonly JwtServiceConfig _config;
        private readonly IJwtStore _store;
        private readonly JwtSecurityTokenHandler _handler;

        public JwtService(
            IOptionsSnapshot<JwtServiceConfig> options,
            IJwtStore store,
            JwtSecurityTokenHandler handler)
        {
            _config = options.Value;
            _store = store;
            _handler = handler;
        }

        public virtual async Task ClearTokenAsync()
        {
            if (await _store.HasTokenAsync())
            {
                await _store.DeleteTokenAsync();
            } 
        }

        public virtual async Task<JwtPayload> GetPayloadAsync()
        {
            var token = await GetTokenAsync();
            var jwt = new JwtSecurityToken(token);
            return jwt.Payload;
        }

        public virtual async Task<string> GetTokenAsync()
        {
            try
            {
                if (!await _store.HasTokenAsync())
                {
                    throw new FileNotFoundException("Tokenが存在しません");
                }

                var token = await _store.ReadTokenAsync();
                if (!_handler.CanReadToken(token))
                {
                    await ClearTokenAsync();
                    throw new InvalidDataException("保存されたTokenが不正です");
                }

                return token;
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Tokenの読み込みに失敗しました", ex);
            }
        }

        public virtual async Task SetTokenAsync(string token)
        {
            try
            {
                if (!_handler.CanReadToken(token))
                {
                    throw new InvalidDataException("Tokenの書式が不正です");
                }

                await _store.WriteTokenAsync(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Tokenの保存に失敗しました", ex);
            }
        }

        public virtual async Task<bool> HasValidTokenAsync()
        {
            // tokenが存在するか
            if (!await _store.HasTokenAsync())
            {
                return false;
            }

            try
            {
                // Tokenを取得
                var token = await GetTokenAsync();

                // Tokenの書式は正しいか
                if (!_handler.CanReadToken(token))
                {
                    return false;
                }

                // Tokenの中身を検証
                var parameter = ConfigreValidationParameters(_config);
                _handler.ValidateToken(token, parameter, out var _);

                return true;
            }
            catch(InvalidOperationException)
            {
                // Tokenの取得に失敗
                return false;
            }
            catch(SecurityTokenException)
            {
                // Tokenの検証に失敗
                return false;
            }
        }

        protected virtual TokenValidationParameters ConfigreValidationParameters(JwtServiceConfig config)
        {
            return new TokenValidationParameters()
            {
                ValidateLifetime = true,

                ValidateIssuer = config.Issuer != null,
                ValidIssuer = config.Issuer,

                ValidateAudience = config.Audiences?.Any() ?? false,
                ValidAudiences = config.Audiences
            };
        }
    }
}