using System.Threading;
using System.Threading.Tasks;
using Kurdle.Options;
using Serilog;

namespace Kurdle.Commands
{
    public class BuildCommand : ICommand<BuildOptions>
    {
        private readonly ILogger logger;

        public BuildCommand(ILogger logger)
        {
            this.logger = logger;
        }


        public Task<int> ExecuteAsync(BuildOptions options, CancellationToken cancellationToken)
        {
            // TODO
            logger.Warning("Build is not yet implemented.");
            return Task.FromResult(1);
        }
    }
}
