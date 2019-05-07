using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Pener.Client.Services.Jwt;

namespace Pener.Client.Services.Auth
{
    public class AuthDelegatingHandler : DelegatingHandler
    {
        private readonly IJwtService _jwtService;

        public AuthDelegatingHandler(
            IJwtService jwtService)
            : this(jwtService, new HttpClientHandler())
        { }

        public AuthDelegatingHandler(
            IJwtService jwtService,
            HttpMessageHandler innerHandler)
            : base(innerHandler)
        {
            _jwtService = jwtService;
        }

        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken)
        {
            // check cancel token.
            cancellationToken.ThrowIfCancellationRequested();

            if (await _jwtService.HasValidTokenAsync())
            {
                request.Headers.Authorization
                    = new AuthenticationHeaderValue("Bearer", await _jwtService.GetTokenAsync());
            }

            return await base.SendAsync(request, cancellationToken);
        }
    }
}
