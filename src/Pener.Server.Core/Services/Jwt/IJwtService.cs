using Pener.Services.User;

namespace Pener.Server.Services.Jwt
{
    public interface IJwtService
    {
        string CreateToken(IUser user);
    }
}