using Kurdle.Commands;
using Microsoft.Extensions.DependencyInjection;
using Spectre.Console.Cli;
using Kurdle.Plumbing;

namespace Kurdle
{
    internal class Program
    {
        static int Main(string[] args)
        {
            var services = new ServiceCollection();
            // TODO - register some stuff

            var registrar = new TypeRegistrar(services);

            var app = new CommandApp(registrar);
            app.Configure(config =>
            {
                config.AddCommand<GenerateCommand>("generate");

                config.AddCommand<CreateCommand>("create");
            });

            return app.Run(args);
        }
    }
}
