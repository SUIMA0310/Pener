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
                    throw new FileNotFoundException("Token�����݂��܂���");
                }

                var token = await _store.ReadTokenAsync();
                if (!_handler.CanReadToken(token))
                {
                    await ClearTokenAsync();
                    throw new InvalidDataException("�ۑ����ꂽToken���s���ł�");
                }

                return token;
            }
            catch(Exception ex)
            {
                throw new InvalidOperationException("Token�̓ǂݍ��݂Ɏ��s���܂���", ex);
            }
        }

        public virtual async Task SetTokenAsync(string token)
        {
            try
            {
                if (!_handler.CanReadToken(token))
                {
                    throw new InvalidDataException("Token�̏������s���ł�");
                }

                await _store.WriteTokenAsync(token);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Token�̕ۑ��Ɏ��s���܂���", ex);
            }
        }

        public virtual async Task<bool> HasValidTokenAsync()
        {
            // token�����݂��邩
            if (!await _store.HasTokenAsync())
            {
                return false;
            }

            try
            {
                // Token���擾
                var token = await GetTokenAsync();

                // Token�̏����͐�������
                if (!_handler.CanReadToken(token))
                {
                    return false;
                }

                // Token�̒��g������
                var parameter = ConfigreValidationParameters(_config);
                _handler.ValidateToken(token, parameter, out var _);

                return true;
            }
            catch(InvalidOperationException)
            {
                // Token�̎擾�Ɏ��s
                return false;
            }
            catch(SecurityTokenException)
            {
                // Token�̌��؂Ɏ��s
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