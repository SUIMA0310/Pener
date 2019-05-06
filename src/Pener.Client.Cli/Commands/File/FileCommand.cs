using McMaster.Extensions.CommandLineUtils;

namespace Pener.Client.Cli.Commands.File
{
    [Command("File")]
    [Subcommand(typeof(DownloadCommand))]
    [HelpOption]
    public class FileCommand
    {
        public void OnExecute(CommandLineApplication app)
        {
            app.ShowHelp();
        }
    }
}
