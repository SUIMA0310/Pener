using System;

namespace Pener.Models.Auth
{
    public class LoginRequest
    {
        public string UserName {get; set;}
        public string Password {get; set;}

        public Guid RequestId {get; set;}
    }
}