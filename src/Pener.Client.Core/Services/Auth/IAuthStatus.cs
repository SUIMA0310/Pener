using System;

namespace Pener.Client.Services.Auth
{
    public interface IAuthStatus
    {
        string UserName { get; }
        Guid UserId { get; }

        DateTime NotValidBefore { get; }
        DateTime ExpirationTime { get; }

        string Issuer { get; }
        string Audience { get; }
    }
}