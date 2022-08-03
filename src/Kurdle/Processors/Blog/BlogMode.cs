using System.IO;
using Kurdle.DepGraph.Models;
using Serilog;

namespace Kurdle.Processors.Blog
{
    public class BlogMode : IBlogMode
    {
        private readonly ILogger logger;

        public BlogMode(ILogger logger)
        {
            this.logger = logger;
        }


        public void AddFileToGraph(DependencyGraph graph, ScanScope scope, FileInfo file)
        {
            // TODO - xyxxy - implement me!
            logger.Warning("BlogMode - AddFileToGraph is not yet implemented.");
        }
    }
}
