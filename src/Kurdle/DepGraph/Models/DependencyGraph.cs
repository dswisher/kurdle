using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Kurdle.DepGraph.Models
{
    public class DependencyGraph
    {
        private readonly List<AbstractProcessorNode> processors = new();

        public DependencyGraph()
        {
        }


        public void AddProcessor(AbstractProcessorNode node)
        {
            processors.Add(node);
        }


        public async Task ExecuteAsync(bool rebuild, CancellationToken cancellationToken)
        {
            // TODO - this is overly simplistic! It needs to take dependencies into account!
            foreach (var node in processors)
            {
                await node.ExecuteAsync(rebuild, cancellationToken);
            }
        }
    }
}
