using System;
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
            var services = new ServiceCollection();

            services.RegisterServices();

            using (var container = services.BuildServiceProvider())
            {
                // Run the correct command
                return await parsedArgs.MapResult(
                    (BuildOptions opts) => RunAsync<BuildOptions, BuildCommand>(opts, container),
                    (CreateOptions opts) => RunAsync<CreateOptions, CreateCommand>(opts, container),
                    _ => Task.FromResult(1));
            }
        }


        private static async Task<int> RunAsync<TOpt, TCmd>(TOpt options, ServiceProvider container)
        where TCmd : ICommand<TOpt>
        {
            var command = container.GetRequiredService<TCmd>();

            using (var tokenSource = new CancellationTokenSource())
            {
                // Shut down semi-gracefully on ctrl+c...
                Console.CancelKeyPress += (sender, eventArgs) =>
                {
                    Log.Warning("*** Cancel event triggered ***");
                    tokenSource.Cancel();
                    eventArgs.Cancel = true;
                };

                return await command.ExecuteAsync(options, tokenSource.Token);
            }
        }
    }
}
