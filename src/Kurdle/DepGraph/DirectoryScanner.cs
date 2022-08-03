using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Kurdle.Config;
using Kurdle.Config.Models;
using Kurdle.DepGraph.Models;
using Serilog;

namespace Kurdle.DepGraph
{
    public class DirectoryScanner : IDirectoryScanner
    {
        private readonly DirectoryInfo rootDirectory;       // TODO - rename to inputRoot
        private readonly DirectoryInfo outputDirectory;     // TODO - rename to outputRoot
        // TODO - need cacheRoot
        private readonly IConfigScanner configScanner;
        private readonly IScopeUpdater scopeUpdater;
        private readonly ILogger logger;

        public DirectoryScanner(
            string rootDirectory,
            string outputDirectory,
            IConfigScanner configScanner,
            IScopeUpdater scopeUpdater,
            ILogger logger)
        {
            this.rootDirectory = new DirectoryInfo(rootDirectory);
            this.outputDirectory = new DirectoryInfo(outputDirectory);

            this.configScanner = configScanner;
            this.scopeUpdater = scopeUpdater;
            this.logger = logger;
        }


        public async Task<DependencyGraph> ScanAsync(CancellationToken cancellationToken)
        {
            // Create the empty graph
            var graph = new DependencyGraph();

            // Set up the initial (root) scope
            var scope = new ScanScope(rootDirectory);

            var config = new ScopeConfig
            {
                ProcessingMode = ProcessingMode.Index
            };

            scopeUpdater.Update(scope, config);

            // Start scanning at the root directory, and recurse down from there
            await DoScanAsync(graph, scope, cancellationToken);

            // Return what has been built
            return graph;
        }


        private async Task DoScanAsync(DependencyGraph graph, ScanScope scope, CancellationToken cancellationToken)
        {
            // Get the files, as a hash set
            var files = scope.Directory.GetFiles().ToHashSet();

            // Process files, starting with files that update the scope settings, which are known as config files.
            await configScanner.ScanAsync(scope, files, cancellationToken);

            // Process anything that remains
            foreach (var file in files.OrderBy(x => x.Extension).ThenBy(x => x.Name))
            {
                scope.ProcessingMode.AddFileToGraph(graph, scope, file);
            }

            // Process any subdirectories
            foreach (var subdir in scope.Directory.GetDirectories().OrderBy(x => x.Name))
            {
                // TODO - scope needs to have inputDirectory, outputDirectory, and cacheDirectory
                var childScope = scope.Clone(subdir);

                await DoScanAsync(graph, childScope, cancellationToken);
            }
        }
    }
}
