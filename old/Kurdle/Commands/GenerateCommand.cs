﻿using System;
using System.ComponentModel;
using Kurdle.Generation;
using Yaclops;
using Yaclops.Attributes;

namespace Kurdle.Commands
{
    [Summary("Generate a website based on the current directory tree.")]
    public class GenerateCommand : ISubCommand
    {
        private readonly IProjectInfo _projectInfo;
        private readonly Func<IProjectInfo, ISiteGenerator> _siteGenerator;


        public GenerateCommand(IProjectInfo projectInfo, Func<IProjectInfo, ISiteGenerator> siteGenerator)
        {
            _projectInfo = projectInfo;
            _siteGenerator = siteGenerator;
        }


        public void Execute()
        {
            _projectInfo.Init(Verbose);
            _siteGenerator(_projectInfo).Generate(DryRun);
        }


        [NamedParameter(ShortName = "n")]
        [Description("Don’t actually generate anything, just parse and emit any errors.")]
        public bool DryRun { get; set; }

        [NamedParameter(ShortName = "v")]
        [Description("Print extra info about the generation.")]
        public bool Verbose { get; set; }
    }
}
