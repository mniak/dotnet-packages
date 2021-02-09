using System.Threading;
using System.Threading.Tasks;

namespace Example.Client.Features
{
    public interface IMainWorker
    {
        Task Work(CancellationToken stoppingToken);
    }
}
