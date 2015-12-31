using System;
using System.ComponentModel;
using Kurdle.Model;
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


        public void Execute()
        {
            var options = new Options(this);
            var site = new Site(options);

            if (!DryRun)
            {
                site.Process();
            }
        }
    }
}
