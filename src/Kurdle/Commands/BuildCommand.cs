using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Kurdle.DepGraph;
using Kurdle.Options;
using Serilog;

namespace Kurdle.Commands
{
    public class BuildCommand : ICommand<BuildOptions>
    {
        private readonly Func<string, string, IDirectoryScanner> scannerFactory;
        private readonly ILogger logger;

        public BuildCommand(
            Func<string, string, IDirectoryScanner> scannerFactory,
            ILogger logger)
        {
            this.scannerFactory = scannerFactory;
            this.logger = logger;
        }


        public async Task<int> ExecuteAsync(BuildOptions options, CancellationToken cancellationToken)
        {
            // Apply defaults to options, if needed
            var rootDir = options.SourceDir ?? Environment.CurrentDirectory;
            var outDir = options.OutputDir;
            if (string.IsNullOrEmpty(outDir))
            {
                var root = new DirectoryInfo(rootDir);
                if (root.Parent == null)
                {
                    outDir = Path.Join(Environment.CurrentDirectory, "site");
                }
                else
                {
                    outDir = Path.Join(root.Parent.FullName, "site");
                }
            }

            // Scan the directories and build a dependency graph
            var scanner = scannerFactory(rootDir, outDir);

            var graph = await scanner.ScanAsync(cancellationToken);

            // Execute the dependency graph
            await graph.ExecuteAsync(options.Rebuild, cancellationToken);

            // No errors!
            return 0;
        }
    }
}
