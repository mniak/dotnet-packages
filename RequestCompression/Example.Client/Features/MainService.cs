using Microsoft.Extensions.Hosting;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Example.Client.Features
{
    public class MainService : BackgroundService
    {
        private readonly IMainWorker worker;

        public MainService(IMainWorker worker)
        {
            this.worker = worker ?? throw new ArgumentNullException(nameof(worker));
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            return worker.Work(stoppingToken);
        }
    }
}
