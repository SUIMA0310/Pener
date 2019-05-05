using System.Threading.Tasks;

namespace Pener.Server.Services.Auth
{
    public interface IAuthService
    {
        Task<AuthResult> Login(string userName, string password);
    }
}