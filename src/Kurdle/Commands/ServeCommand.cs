using System;
using Yaclops;
using Yaclops.Attributes;

namespace Kurdle.Commands
{
    [Summary("Build the site and start a minimalist web server to browse the result.")]
    public class ServeCommand : AbstractBuildCommand, ISubCommand
    {
        public void Execute()
        {

            Console.WriteLine("Serve is not yet implemented.");
        }
    }
}
