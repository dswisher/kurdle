using Spectre.Console;
using Spectre.Console.Cli;

namespace Kurdle.Commands
{
    public sealed class CreateCommand : Command<CreateCommand.Settings>
    {
        public sealed class Settings : CommandSettings
        {
        }


        public override int Execute(CommandContext context, Settings settings)
        {
            AnsiConsole.Write(new Markup("[red]Create is not yet implemented.[/]"));
            return 0;
        }
    }
}
