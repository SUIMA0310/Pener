using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Pener.Client.Services.Auth;
using Pener.Client.Services.Jwt;

namespace Pener.Client.Cli.Commands.Auth
{
    [Command]
    public class LogoutCommand
    {
        private readonly IAuthService _authService;

        public LogoutCommand(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task OnExecute(CommandLineApplication app)
        {
            try
            {
                await _authService.LogoutAsync();

                Console.WriteLine(Consts.Messages.Success);
            }
            catch
            {
                Console.WriteLine(Consts.Messages.Failure);
            }
        }
    }
}
