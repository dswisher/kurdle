using System.IO;
using Kurdle.DepGraph.Models;
using Serilog;

namespace Kurdle.Processors.Index
{
    public class IndexMode : IIndexMode
    {
        private readonly ILogger logger;

        public IndexMode(ILogger logger)
        {
            this.logger = logger;
        }


        public void AddFileToGraph(DependencyGraph graph, ScanScope scope, FileInfo file)
        {
            // TODO - xyzzy - implement me!
            logger.Warning("IndexMode - AddFileToGraph is not yet implemented.");
        }
    }
}
