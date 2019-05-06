using System;

namespace Pener.Client.Services.Auth
{
    public class AuthStatus : IAuthStatus
    {
        public string UserName { get; set; }
        public Guid UserId { get; set; }

        public DateTime NotValidBefore { get; set; }
        public DateTime ExpirationTime { get; set; }

        public string Issuer { get; set; }
        public string Audience { get; set; }
    }
}