using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace WorkerService
{
    public class Worker : BackgroundService
    {
        private readonly ILogger<Worker> _logger;
        private readonly IHttpClientFactory httpClientFactory;
        private List<String> Urls = new List<string> { "https://www.google.com/" };

        public Worker(ILogger<Worker> logger, IHttpClientFactory httpClientFactory)
        {
            this._logger = logger;
            this.httpClientFactory = httpClientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    await PollUrls();
                }
                catch (Exception ex)
                {

                    _logger.LogError(ex, "An error occured while polling URLs.");
                }
                finally
                {
                    await Task.Delay(1000, stoppingToken);
                }

                // _logger.LogInformation("Worker running at: {time}", DateTimeOffset.Now);

                // Delay time in seconds
                await Task.Delay(5000, stoppingToken);
            }
        }

        private async Task PollUrls()
        {
            var tasks = new List<Task>();

            foreach (var url in Urls)
            {
                tasks.Add(PollUrl(url));
            }

            await Task.WhenAll(tasks);
        }

        private async Task PollUrl(string url)
        {
            try
            {
                var client = httpClientFactory.CreateClient();
                var response = await client.GetAsync(url);

                if (response.IsSuccessStatusCode)
                    _logger.LogInformation("{Url} is online.", url);
                else
                    _logger.LogInformation("{Url} is offline.", url);
            }
            catch (Exception ex)
            {

                _logger.LogWarning(ex, "{Url} is offline.", url);
            }
        }
    }
}
