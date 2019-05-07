using McMaster.Extensions.CommandLineUtils;

namespace Pener.Client.Cli.Commands.Auth
{
    [Command]
    [Subcommand(typeof(LoginCommand))]
    [Subcommand(typeof(LogoutCommand))]
    [Subcommand(typeof(StatusCommand))]
    public class AuthCommand
    {
        public void OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
        }
    }
}
