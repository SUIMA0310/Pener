using System;

namespace Pener.Client.Services.Auth
{
    public class AuthServiceConfig
    {
        private Uri _AuthServerAddress;

        public Uri ServerAddress { get; set; }

        public Uri AuthServerAddress
        {
            get => _AuthServerAddress ?? ServerAddress;
            set => _AuthServerAddress = value;
        }
    }
}