using System;
using Yaclops;
using Yaclops.Attributes;

namespace Kurdle.Commands
{
    [Summary("Read the site and print out some info about it.")]
    public class InfoCommand : AbstractBuildCommand, ISubCommand
    {
        public void Execute()
        {
            Options options = new Options(this);

            Console.WriteLine("{0}: {1}", Options.SourceName, options.Source ?? "n/a");
            Console.WriteLine("{0}: {1}", Options.DestinationName, options.Destination ?? "n/a");
            Console.WriteLine("name: {0}", options.Get("name") ?? "n/a");
        }
    }
}
