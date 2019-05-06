using McMaster.Extensions.CommandLineUtils;

namespace Pener.Client.Cli.Commands.Auth
{
    [Command]
    [Subcommand(typeof(LoginCommand))]
    [Subcommand(typeof(LogoutCommand))]
    public class AuthCommand
    {
    }
}
