using System.Threading;
using System.Threading.Tasks;
using Kurdle.Options;
using Serilog;

namespace Kurdle.Commands
{
    public sealed class CreateCommand : ICommand<CreateOptions>
    {
        private readonly ILogger logger;

        public CreateCommand(ILogger logger)
        {
            this.logger = logger;
        }


        public Task<int> ExecuteAsync(CreateOptions options, CancellationToken cancellationToken)
        {
            // TODO
            logger.Warning("Create is not yet implemented.");
            return Task.FromResult(1);
        }
    }
}
