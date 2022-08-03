using System.Collections.Generic;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Kurdle.DepGraph.Models;

namespace Kurdle.Config
{
    public interface IConfigScanner
    {
        Task ScanAsync(ScanScope scope, HashSet<FileInfo> files, CancellationToken cancellationToken);
    }
}
