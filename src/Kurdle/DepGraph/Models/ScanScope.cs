using System.IO;
using Kurdle.Config.Models;
using Kurdle.Processors;

namespace Kurdle.DepGraph.Models
{
    public class ScanScope
    {
        private readonly ScanScope parentScope;

        public ScanScope(DirectoryInfo inputDirectory, DirectoryInfo outputDirectory, ScanScope parentScope = null)
        {
            InputDirectory = inputDirectory;
            OutputDirectory = outputDirectory;

            this.parentScope = parentScope;

            if (parentScope == null)
            {
                Depth = 1;
                Layout = "main";
            }
            else
            {
                Depth = parentScope.Depth + 1;
            }
        }

        public DirectoryInfo InputDirectory { get; }
        public DirectoryInfo OutputDirectory { get; }

        public int Depth { get; }

        public IProcessingMode ProcessingMode { get; set; }
        public string Layout { get; set; }

        public ScanScope Clone(DirectoryInfo subdir)
        {
            // Determine the output directory
            // TODO - this method of determining the output directory seems overly simplistic, although
            // I cannot think of a case where it will fail...
            var outputDir = new DirectoryInfo(Path.Join(OutputDirectory.FullName, subdir.Name));

            // Create the new scope
            var scope = new ScanScope(subdir, outputDir, this);

            // Copy over some additional bits
            scope.ProcessingMode = ProcessingMode;
            scope.Layout = Layout;

            // Return the result
            return scope;
        }
    }
}
