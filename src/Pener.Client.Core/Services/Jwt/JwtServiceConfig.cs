using System.Collections.Generic;

namespace Pener.Client.Services.Jwt
{
    public class JwtServiceConfig
    {
        public string Issuer { get; set; }

        public IEnumerable<string> Audiences { get; set; }
    }
}