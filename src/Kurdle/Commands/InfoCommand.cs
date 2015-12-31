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

            Console.WriteLine("Source:      {0}", options.Source ?? "n/a");
            Console.WriteLine("Destination: {0}", options.Destination ?? "n/a");
        }
    }
}
