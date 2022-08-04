using System.Threading;
using System.Threading.Tasks;

namespace Kurdle.DepGraph.Models
{
    public abstract class AbstractProcessorNode
    {
        public abstract Task ExecuteAsync(bool rebuild, CancellationToken cancellationToken);
    }
}
