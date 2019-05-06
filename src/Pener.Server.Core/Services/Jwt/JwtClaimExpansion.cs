using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

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
            return GetRandomBytes().ToHexString();
        }

        private static string GetUnixTime(DateTime time)
        {
            var baseTime = new DateTime(1970, 1, 1);
            return (time - baseTime).Seconds.ToString();
        }

        private static byte[] GetRandomBytes()
        {
            var random = new RNGCryptoServiceProvider();
            var bytes = new byte[32];

            // get random bytes.
            random.GetBytes(bytes);

            return bytes;
        }

        private static string ToHexString(this IEnumerable<byte> bytes)
        {
            var capacity = bytes.Count() * 2;
            var bilder = new StringBuilder(capacity);

            foreach (var b in bytes)
            {
                bilder.AppendFormat("{0:x2}", b);
            }

            return bilder.ToString();
        }
    }
}