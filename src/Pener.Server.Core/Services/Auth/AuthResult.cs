namespace Pener.Server.Services.Auth
{
    public class AuthResult
    {
        public bool Successed { get; internal set; }

        public string Token { get; internal set; }

        public string ErrorMessage { get; internal set; }
    }
}