using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Pener.Server.Services.Jwt;
using Pener.Services.User;

namespace Pener.Server.Services.Auth
{
    public class AuthService : IAuthService
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly IJwtService _jwtService;

        public AuthService(
            SignInManager<User> signInManager,
            IJwtService jwtService)
        {
            _signInManager = signInManager;
            _userManager = signInManager.UserManager;
            _jwtService = jwtService;
        }

        public async Task<AuthResult> Login(string userName, string password)
        {
            var user = await _userManager.FindByNameAsync(userName);

            if (user == null)
            {
                return new AuthResult()
                {
                    Successed = false,
                    ErrorMessage = "認証に失敗しました"
                };
            }

            var result = await _signInManager.CheckPasswordSignInAsync(user, password, true);

            if (result.Succeeded)
            {
                return new AuthResult()
                {
                    Successed = true,
                    Token = _jwtService.CreateToken(user)
                };
            }
            else if (result.IsLockedOut)
            {
                return new AuthResult()
                {
                    Successed = false,
                    ErrorMessage = "アカウントがロックされています"
                };
            }
            else
            {
                return new AuthResult()
                {
                    Successed = false,
                    ErrorMessage = "認証に失敗しました"
                };
            }
        }
    }
}