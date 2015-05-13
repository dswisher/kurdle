using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yaclops;
using Yaclops.Attributes;

namespace Kurdle.Commands
{
    [Summary("Generate a website based on the current directory tree.")]
    public class GenerateCommand : ISubCommand
    {
        public void Execute()
        {
            Console.WriteLine("Generate is not yet implemented!");
        }


        [NamedParameter(ShortName = "n")]
        [Description("Don’t actually generate anything, just parse and emit any errors.")]
        public bool DryRun { get; set; }
    }
}
