using System.Threading.Tasks;

namespace Pener.Client.Services.Jwt
{
    public interface IJwtStore
    {
        Task<string> ReadTokenAsync();
        Task WriteTokenAsync(string token);
        Task DeleteTokenAsync();

        Task<bool> HasTokenAsync();
    }
}