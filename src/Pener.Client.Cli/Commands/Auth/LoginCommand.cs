using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Pener.Client.Services.Auth;

namespace Pener.Client.Cli.Commands.Auth
{
    [Command]
    public class LoginCommand
    {
        private readonly IAuthService _authService;

        [Argument(0)]
        public string UserName { get; private set; }

        [Option("-p | --password")]
        public string Password { get; private set; }

        public LoginCommand(
            IAuthService loginService)
        {
            _authService = loginService;
        }

        public async Task OnExecute(CommandLineApplication app)
        {
            if (UserName == null)
            {
                UserName = Prompt.GetString("UserName : ");
            }

            if (Password == null)
            {
                Password = Prompt.GetPassword("Password : ");
            }

            try
            {
                await _authService.LoginAsync(UserName, Password);

                Console.WriteLine(Consts.Messages.Success);
            }
            catch
            {
                Console.WriteLine(Consts.Messages.Failure);
            }
        }
    }
}
