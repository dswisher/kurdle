using System;
using Yaclops;
using Yaclops.Attributes;

namespace Kurdle.Commands
{
    [Summary("Remove any previously generated content.")]
    public class CleanCommand : AbstractBuildCommand, ISubCommand
    {
        public void Execute()
        {
            Console.WriteLine("Clean is not yet implemented.");
        }
    }
}
