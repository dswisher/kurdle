using System;
using System.IO;
using Yaclops;
using Yaclops.Attributes;

namespace Kurdle.Commands
{
    [Summary("Remove any previously generated content.")]
    public class CleanCommand : AbstractBuildCommand, ISubCommand
    {
        public void Execute()
        {
            Options options = new Options(this);

            var dest = new DirectoryInfo(options.Destination);

            if (dest.Exists)
            {
                Console.WriteLine("Removing {0}", dest.FullName);
                dest.Delete(true);
            }
        }
    }
}
