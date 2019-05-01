using System.IdentityModel.Tokens.Jwt;
using System.Threading.Tasks;

namespace Pener.Client.Services.Jwt
{
    public interface IJwtService
    {
        Task<bool> HasValidTokenAsync();

        Task<JwtPayload> GetPayloadAsync();

        Task<string> GetTokenAsync();
        Task SetTokenAsync(string token);
        Task ClearTokenAsync();
    }
}