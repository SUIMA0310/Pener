using System.Threading.Tasks;

namespace Pener.Client.Services.Auth
{
    public interface IAuthService
    {
        Task<bool> IsAuthenticatedAsync(bool hardCheck = false);

        Task LoginAsync(string userName, string password);
        Task LogoutAsync();

        Task<IAuthStatus> GetAuthStatusAsync();
    }
}