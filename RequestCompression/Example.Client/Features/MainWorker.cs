using Bogus;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Client.Features
{
    public class MainWorker : IMainWorker
    {
        private readonly ILogger logger;
        private readonly HttpClient httpClient;

        public MainWorker(ILogger<MainWorker> logger, HttpClient httpClient)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.httpClient = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
        }

        public async Task Work(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                logger.LogInformation("Press <Enter> to start");
                Console.ReadLine();
                Console.Clear();

                var largePayload = ProduceLargePayload();

                await httpClient.PostAsJsonAsync("http://localhost:5000/data", largePayload);
            }
        }

        private static IEnumerable<string> ProduceLargePayload()
        {
            var faker = new Faker();
            for (int i = 0; i < 10; i++)
            {
                yield return faker.Lorem.Sentence(5);
            }
        }
    }
}
