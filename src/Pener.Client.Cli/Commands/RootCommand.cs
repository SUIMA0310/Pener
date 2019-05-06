using McMaster.Extensions.CommandLineUtils;
using Pener.Client.Cli.Commands.Auth;
using Pener.Client.Cli.Commands.File;

namespace Pener.Client.Cli.Commands
{
    [Command]
    [Subcommand(typeof(FileCommand))]
    [Subcommand(typeof(AuthCommand), typeof(LoginCommand), typeof(LogoutCommand))]
    [HelpOption]
    //[VersionOptionFromMember]
    public class RootCommand
    {
        public void OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
        }
    }
}
