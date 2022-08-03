using System.Threading;
using System.Threading.Tasks;
using Kurdle.DepGraph.Models;

namespace Kurdle.DepGraph
{
    public interface IDirectoryScanner
    {
        Task<DependencyGraph> ScanAsync(CancellationToken cancellationToken);
    }
}
