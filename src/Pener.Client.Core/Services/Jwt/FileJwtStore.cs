using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;

namespace Pener.Client.Services.Jwt
{
    public class FileJwtStore : IJwtStore
    {
        private readonly FileJwtStoreConfig _config;
        private readonly SemaphoreSlim _semaphore;

        public FileJwtStore(
            IOptionsSnapshot<FileJwtStoreConfig> options)
        {
            _config = options.Value;
            _semaphore = new SemaphoreSlim(1,1);
        }

        public async Task DeleteTokenAsync()
        {
            try
            {
                await _semaphore.WaitAsync().ConfigureAwait(false);

                if (File.Exists(_config.SavePath))
                {
                    File.Delete(_config.SavePath);
                }

                return;
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<bool> HasTokenAsync()
        {
            try
            {
                await _semaphore.WaitAsync().ConfigureAwait(false);

                return File.Exists(_config.SavePath);
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task<string> ReadTokenAsync()
        {
            try
            {
                await _semaphore.WaitAsync().ConfigureAwait(false);

                using (var sr = new StreamReader(_config.SavePath))
                {
                    return await sr.ReadToEndAsync().ConfigureAwait(false);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }

        public async Task WriteTokenAsync(string token)
        {
            try
            {
                await _semaphore.WaitAsync().ConfigureAwait(false);

                using (var sw = new StreamWriter(_config.SavePath))
                {
                    await sw.WriteAsync(token).ConfigureAwait(false);
                }
            }
            finally
            {
                _semaphore.Release();
            }
        }
    }
}