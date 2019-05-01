using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

#if DEBUG
[assembly: System.Runtime.CompilerServices.InternalsVisibleTo("Tests.Pener.Client.Cli")]
#endif

namespace Pener.Client.Services.Jwt
{
    internal class MyJwtSecurityTokenHandler : JwtSecurityTokenHandler
    {
        public override ClaimsPrincipal ValidateToken(string token, TokenValidationParameters validationParameters, out SecurityToken validatedToken)
        {
            // Cancel signature verification.
            validatedToken = new JwtSecurityToken(token);
            return ValidateTokenPayload(validatedToken as JwtSecurityToken, validationParameters);
        }
    }
}