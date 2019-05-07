using System;
using System.Collections.Generic;

namespace Pener.Client.Services.Auth
{
    public class AuthStatus : IAuthStatus
    {
        public string UserName { get; set; }
        public string UserId { get; set; }

        public DateTime? NotValidBefore { get; set; }
        public DateTime? ExpirationTime { get; set; }

        public string Issuer { get; set; }
        public IEnumerable<string> Audiences { get; set; }
    }
}