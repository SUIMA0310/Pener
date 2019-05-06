using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Pener.Client.Services.Jwt;

namespace Pener.Client.Services.Auth
{
    public class AuthServiceConfig
    {
        private string _AuthServerAddress;

        public string ServerAddress { get; set; }

        public string AuthServerAddress
        {
            get => _AuthServerAddress ?? ServerAddress;
            set => _AuthServerAddress = value;
        }
    }


    public class AuthService : IAuthService
    {
        private readonly AuthServiceConfig _config;
        private readonly IJwtService _jwtService;
        private readonly HttpClient _httpClient;

        public AuthService(
            IOptionsSnapshot<AuthServiceConfig> options,
            IJwtService jwtService)
        {
            _config = options.Value;
            _jwtService = jwtService;
        }

        public virtual async Task<IAuthStatus> GetAuthStatusAsync()
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
        }

        public virtual async Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        protected virtual async Task<bool> HardLoginCheckAsync()
        {
            var res = await _httpClient.GetAsync("/Api/Auth/Check");

            return res.IsSuccessStatusCode;
        }
    }
}