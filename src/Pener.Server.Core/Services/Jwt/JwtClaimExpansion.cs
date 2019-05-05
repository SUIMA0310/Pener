using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Pener.Server.Services.Jwt
{
    internal static class JwtClaimExpansion
    {
        internal static IList<Claim> AddNameId(
            this IList<Claim> claims,
            string nameId)
        {
            new Claim(JwtRegisteredClaimNames.NameId, nameId);

            return claims;
        }

        internal static IList<Claim> AddUniqueName(
            this IList<Claim> claims,
            string uniqueName)
        {
            new Claim(JwtRegisteredClaimNames.NameId, uniqueName);

            return claims;
        }

        internal static IList<Claim> AddJti(
                this IList<Claim> claims)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Jti, GetJti()));

            return claims;
        }

        internal static IList<Claim> AddAudiences(
                this IList<Claim> claims,
                IEnumerable<string> audiences)
        {
            foreach (var audience in audiences)
            {
                claims.Add(new Claim(JwtRegisteredClaimNames.Aud, audience));
            }

            return claims;
        }

        internal static IList<Claim> AddIssuer(
            this IList<Claim> claims,
            string issuer)
        {
            claims.Add(new Claim(JwtRegisteredClaimNames.Iss, issuer));

            return claims;
        }

        internal static IList<Claim> AddExpire(
            this IList<Claim> claims,
            int expireTime)
        {
            var exp = DateTime.Now.AddMinutes(expireTime);
            claims.Add(new Claim(JwtRegisteredClaimNames.Exp, GetUnixTime(exp)));

            return claims;
        }

        internal static IList<Claim> AddNotbefore(
            this IList<Claim> claims)
        {
            var nbf = DateTime.Now;
            claims.Add(new Claim(JwtRegisteredClaimNames.Nbf, GetUnixTime(nbf)));

            return claims;
        }

        private static string GetJti()
        {
            var random = new Random();
            var bytes = new byte[128];

            // get random bytes.
            random.NextBytes(bytes);

            // convert to string.
            return Convert.ToBase64String(bytes);
        }

        private static string GetUnixTime(DateTime time)
        {
            var baseTime = new DateTime(1970, 1, 1);
            return (time - baseTime).Seconds.ToString();
        }
    }
}