using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.Extensions.Options;
using Pener.Client.Services.Jwt;
using Pener.Utils;
using Pener.Models.Auth;

namespace Pener.Client.Services.Auth
{
    public class AuthService : IAuthService
    {
        private const string AUTH_LOGIN_URL = @"/api/auth/login";
        private const string AUTH_CHECK_URL = @"/api/auth/check";

        private readonly AuthServiceConfig _config;
        private readonly IJwtService _jwtService;
        private readonly HttpClient _httpClient;

        public AuthService(
            IOptionsSnapshot<AuthServiceConfig> options,
            IJwtService jwtService,
            HttpClient httpClient)
        {
            _config = options.Value;
            _jwtService = jwtService;
            _httpClient = httpClient;
        }

        public virtual async Task<IAuthStatus> GetAuthStatusAsync()
        {


            var payload = await _jwtService.GetPayloadAsync();

            var status = new AuthStatus()
            {
                UserId = payload[JwtRegisteredClaimNames.NameId] as string,
                UserName = payload[JwtRegisteredClaimNames.UniqueName] as string,

                Issuer = payload.Iss,
                Audiences = payload.Aud,

                ExpirationTime = payload.Exp?.FromUnixTime(),
                NotValidBefore = payload.Nbf?.FromUnixTime()
            };

            return status;
        }

        public virtual async Task<bool> IsAuthenticatedAsync(bool hardCheck = false)
        {
            if (!await _jwtService.HasValidTokenAsync())
            {
                return false;
            }

            if (hardCheck && !await HardLoginCheckAsync())
            {
                return false;
            }

            return true;
        }

        public virtual async Task LoginAsync(string userName, string password)
        {
            // make request info.
            var userRequest = new LoginRequest()
            {
                UserName = userName,
                Password = password,
                RequestId = Guid.NewGuid()
            };
            var sendUrl = new Uri(_config.AuthServerAddress, AUTH_LOGIN_URL);

            // auth request.
            var res = await _httpClient
                .PostAsJsonAsync(sendUrl, userRequest)
                .ConfigureAwait(false);

            if (!res.IsSuccessStatusCode)
            {
                throw new HttpRequestException(res.ReasonPhrase);
            }

            // get responce.
            var login = await res.Content
                .ReadAsAsync<LoginResponse>()
                .ConfigureAwait(false);

            // save json web token.
            await _jwtService.SetTokenAsync(login.Token);
        }

        public virtual async Task LogoutAsync()
        {
            await _jwtService.ClearTokenAsync();
        }

        protected virtual async Task<bool> HardLoginCheckAsync()
        {
            var sendUrl = new Uri(_config.AuthServerAddress, AUTH_CHECK_URL);
            var res = await _httpClient.GetAsync(sendUrl);
            return res.IsSuccessStatusCode;
        }
    }
}