using System;
using System.Collections.Generic;

namespace Pener.Client.Services.Auth
{
    public interface IAuthStatus
    {
        string UserName { get; }
        string UserId { get; }

        DateTime? NotValidBefore { get; }
        DateTime? ExpirationTime { get; }

        string Issuer { get; }
        IEnumerable<string> Audiences { get; }
    }
}