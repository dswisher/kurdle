using Kurdle.Config.Models;
using Kurdle.DepGraph.Models;

namespace Kurdle.Config
{
    public interface IScopeUpdater
    {
        void Update(ScanScope scope, ScopeConfig config);
    }
}
