using System.ComponentModel;
using Spectre.Console;
using Spectre.Console.Cli;

namespace Kurdle.Commands
{
    public sealed class GenerateCommand : Command<GenerateCommand.Settings>
    {
        public sealed class Settings : CommandSettings
        {
            [CommandArgument(0, "[sourceDir]")]
            [DefaultValue(".")]
            public string SourceDir { get; set; }

            [CommandOption("--output|-o")]
            [DefaultValue("build")]
            public string OutputDir { get; set; }
        }

        public override int Execute(CommandContext context, Settings settings)
        {
            AnsiConsole.Write(new Markup("[red]Generate is not yet implemented.[/]\n"));

            // Create a table
            var table = new Table();

            table.HideHeaders();

            // Add some columns
            table.AddColumn("Name");
            table.AddColumn("Value");

            // Add some rows
            table.AddRow("Source Dir", settings.SourceDir);
            table.AddRow("Output Dir", settings.OutputDir);

            // Render the table to the console
            AnsiConsole.Write(table);

            return 0;
        }
    }
}
