using System.IO;
using Kurdle.Config.Models;
using Kurdle.Processors;

namespace Kurdle.DepGraph.Models
{
    public class ScanScope
    {
        private readonly ScanScope parentScope;

        public ScanScope(DirectoryInfo directory, ScanScope parentScope = null)
        {
            Directory = directory;

            this.parentScope = parentScope;
        }

        public DirectoryInfo Directory { get; }

        public IProcessingMode ProcessingMode { get; set; }

        public ScanScope Clone(DirectoryInfo subdir)
        {
            var scope = new ScanScope(subdir, this);

            scope.ProcessingMode = ProcessingMode;

            return scope;
        }
    }
}
