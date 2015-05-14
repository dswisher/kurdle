using System;
using System.ComponentModel;
using Kurdle.Services;
using Yaclops;
using Yaclops.Attributes;

namespace Kurdle.Commands
{
    [Summary("Generate a website based on the current directory tree.")]
    public class GenerateCommand : ISubCommand
    {
        private readonly IProjectInfo _projectInfo;

        public GenerateCommand(IProjectInfo projectInfo)
        {
            _projectInfo = projectInfo;
        }


        public void Execute()
        {
            _projectInfo.Init();

            Console.WriteLine("Generate is not yet implemented!");
        }


        [NamedParameter(ShortName = "n")]
        [Description("Don’t actually generate anything, just parse and emit any errors.")]
        public bool DryRun { get; set; }
    }
}
