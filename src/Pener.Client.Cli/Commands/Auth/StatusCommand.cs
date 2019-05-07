using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Pener.Client.Services.Auth;

namespace Pener.Client.Cli.Commands.Auth
{
    [Command]
    public class StatusCommand
    {
        private readonly IAuthService _authService;

        public StatusCommand(IAuthService authService)
        {
            _authService = authService;
        }

        public async Task OnExecuteAsync(CommandLineApplication app)
        {
            if (await _authService.IsAuthenticatedAsync(true))
            {

                var status = await _authService.GetAuthStatusAsync();

                Console.WriteLine($"ログイン中   : true");
                Console.WriteLine($"ログイン時刻 : {status.NotValidBefore.ToString()}");
                Console.WriteLine($"有効期限     : {status.ExpirationTime.ToString()}");
                Console.WriteLine($"ユーザId     : {status.UserId}");
                Console.WriteLine($"ユーザ名     : {status.UserName}");
            }
            else
            {
                Console.WriteLine($"ログイン中   : false");
            }
        }
    }
}
