using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Pener.Server.Services.Auth;
using Pener.Models.Auth;

namespace Pener.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(
            IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginRequest request)
        {
            if (!ModelState.IsValid ||
                string.IsNullOrEmpty(request?.UserName) ||
                string.IsNullOrEmpty(request?.Password))
            {
                return BadRequest();
            }

            var result = await _authService.Login(request.UserName, request.Password);

            if (result.Successed)
            {
                return Ok(new LoginResponse()
                {
                    Token = result.Token
                });
            }
            else
            {
                return BadRequest(result.ErrorMessage);
            }
        }

        [HttpGet("check")]
        [Authorize]
        public async Task<IActionResult> Check()
        {
            await Task.CompletedTask;
            return Ok();
        }
    }
}