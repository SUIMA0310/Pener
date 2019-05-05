using System.Collections.Generic;

namespace Pener.Services.User
{
    public interface IUser
    {
        string Id { get; set; }
        string UserName { get; set; }
        string PasswordHash { get; set; }
        string SecurityStamp { get; set; }
        IReadOnlyList<string> Roles { get; }

        bool LockoutEnabled { get; set; }

    }
}