namespace Pener.Server.Services.Jwt
{
    public class JwtServiceConfig
    {
        public string SigningKey { get; set; }

        public string Issuer { get; set; }

        public string Audience { get; set; }

        public int ExpireTime { get; set; }
    }
}