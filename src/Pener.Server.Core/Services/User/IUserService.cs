using Microsoft.AspNetCore.Identity;

namespace Pener.Services.User
{
    public interface IUserService
    {
        UserManager<User> UserManager { get; }
    }
}