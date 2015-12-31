using System;
using System.ComponentModel;
using Yaclops;
using Yaclops.Attributes;

namespace Kurdle.Commands
{
    [Summary("Generate a website based on the current directory tree.")]
    public class BuildCommand : AbstractBuildCommand, ISubCommand
    {
        [NamedParameter, ShortName("n")]
        [Description("Don’t actually generate anything, just parse and emit any errors.")]
        public bool DryRun { get; set; }

        [NamedParameter, ShortName("v")]
        [Description("Print extra info about the build.")]
        public bool Verbose { get; set; }


        public void Execute()
        {

            Console.WriteLine("Build is not yet implemented.");
        }
    }
}
