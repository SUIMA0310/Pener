using System;
using System.Collections.Generic;
using Pener.Server.Services.Jwt;

namespace Pener.Services.User
{
    public class User : Raven.Identity.IdentityUser, IUser
    {
        public User():base()
        { }

        public User(string userName, string pwHash):this()
        {
            UserName = userName;
            PasswordHash = pwHash;
        }

        public override bool PhoneNumberConfirmed => false;
        public override string PhoneNumber => string.Empty;

        public override bool EmailConfirmed => false;
        public override string Email => $"{UserName}@localhost";

        public override DateTimeOffset? LockoutEnd => null;

        public override bool TwoFactorEnabled => false;
        public override List<string> TwoFactorRecoveryCodes => null;
    }
}