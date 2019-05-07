namespace Pener.Client.Services.Auth
{
    public class AuthServiceConfig
    {
        private string _AuthServerAddress;

        public string ServerAddress { get; set; }

        public string AuthServerAddress
        {
            get => _AuthServerAddress ?? ServerAddress;
            set => _AuthServerAddress = value;
        }
    }
}