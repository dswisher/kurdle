using System.IO;
using Kurdle.DepGraph.Models;

namespace Kurdle.Processors
{
    public interface IProcessingMode
    {
        void AddFileToGraph(DependencyGraph graph, ScanScope scope, FileInfo file);
    }
}
