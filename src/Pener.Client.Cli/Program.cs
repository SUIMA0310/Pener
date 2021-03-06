﻿using System;
using System.IO;
using System.Net.Http;
using McMaster.Extensions.CommandLineUtils;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using Pener.Client.Cli.Commands;
using Pener.Client.Services.Auth;
using Pener.Client.Services.Jwt;

namespace Pener.Client.Cli
{
    public class Program
    {
        public static int Main(string[] args)
        {
            var services = CreateServiceCollection();
            var config = CreateConfiguration();

            ConfigureServices(services, config);

            var app = new CommandLineApplication<RootCommand>();
            app.Conventions
                .UseDefaultConventions()
                .UseConstructorInjection(services.BuildServiceProvider());

            return app.Execute(args);

        }

        public static void ConfigureServices(
            IServiceCollection services,
            IConfiguration config)
        {
            services.AddOptions();

            services.AddJwtService(config);
            services.AddAuthService(config).AddDefaultHttpClient();
        }

        public static IServiceCollection CreateServiceCollection()
        {
            return new ServiceCollection();
        }

        public static IConfiguration CreateConfiguration()
        {
            return new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .AddCommandLine(Environment.GetCommandLineArgs())
                .Build();
        }
    }
}
