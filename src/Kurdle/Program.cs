using System.Threading;
using System.Threading.Tasks;
using CommandLine;
using Kurdle.Commands;
using Kurdle.Options;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Kurdle
{
    internal static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var parsedArgs = Parser.Default.ParseArguments<
                BuildOptions,
                CreateOptions>(args);

            // Pull out the common options
            CommonOptions commonOptions = null;
            parsedArgs.WithParsed<CommonOptions>(opts =>
            {
                commonOptions = opts;
            });

            // Set up logging
            var logConfig = new LoggerConfiguration()
                .WriteTo.Console();

            if (commonOptions != null && commonOptions.Verbose)
            {
                logConfig.MinimumLevel.Debug();
            }

            Log.Logger = logConfig.CreateLogger();

            // Set up the container
            // TODO - move this out to a container class
            var services = new ServiceCollection();
            services.AddSingleton(Log.Logger);

            services.AddSingleton<BuildCommand>();
            services.AddSingleton<CreateCommand>();

            var container = services.BuildServiceProvider();

            // Run the correct command
            return await parsedArgs.MapResult(
                (BuildOptions opts) => RunAsync<BuildOptions, BuildCommand>(opts, container),
                (CreateOptions opts) => RunAsync<CreateOptions, CreateCommand>(opts, container),
                _ => Task.FromResult(1));
        }


        private static async Task<int> RunAsync<TOpt, TCmd>(TOpt options, ServiceProvider container)
        where TCmd : ICommand<TOpt>
        {
            var command = container.GetRequiredService<TCmd>();

            // TODO - create a proper cancellation token
            var token = default(CancellationToken);

            return await command.ExecuteAsync(options, token);
        }
    }
}
