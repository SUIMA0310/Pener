using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using McMaster.Extensions.CommandLineUtils;
using Microsoft.Extensions.Configuration;

namespace Pener.Client.Cli.Commands.File
{
    [Command("Download", "DL")]
    [HelpOption]
    public class DownloadCommand
    {
        private readonly RootCommand _rootCommand;
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        [Argument(0)]
        public Uri SourceUrl { get; }

        [Argument(1)]
        public string SavePath { get; }

        public DownloadCommand(
            RootCommand rootCommand,
            IConfiguration configuration,
            HttpClient httpClient)
        {
            _rootCommand = rootCommand;
            _configuration = configuration;
            _httpClient = httpClient;
        }

        public async Task OnExecute(CommandLineApplication app)
        {

            using (var ns = await _httpClient.GetStreamAsync(SourceUrl))
            using (var fs = new FileStream(SavePath, FileMode.CreateNew))
            {
                await ns.CopyToAsync(fs);
            }
        }
    }
}
