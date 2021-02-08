using System.Threading;
using System.Threading.Tasks;

namespace Mniak.RequestCompression.Example.Client.Features
{
    public interface IMainWorker
    {
        Task Work(CancellationToken stoppingToken);
    }
}
