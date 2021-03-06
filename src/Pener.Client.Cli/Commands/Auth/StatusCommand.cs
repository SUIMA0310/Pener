﻿using System;
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

                Console.WriteLine($"ステータス   : ログイン中");
                Console.WriteLine($"ユーザ名     : {status.UserName}");
                Console.WriteLine($"ユーザId     : {status.UserId}");
                Console.WriteLine($"ログイン時刻 : {status.NotValidBefore.ToString()}");
                Console.WriteLine($"有効期限     : {status.ExpirationTime.ToString()}");

            }
            else
            {
                Console.WriteLine($"ステータス   : ログアウト");
            }
        }
    }
}
