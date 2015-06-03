using System;
using System.ComponentModel;
using Kurdle.Generation;
using Kurdle.Server;
using Yaclops;
using Yaclops.Attributes;

namespace Kurdle.Commands
{
    [Summary("Start a small, minimalist web server and watch for changes to the source.")]
    public class ServeCommand : ISubCommand
    {
        private readonly IProjectInfo _projectInfo;
        private readonly ISimpleServer _server;
        private readonly IAutoGenerator _autoGenerator;
        private readonly Func<IProjectInfo, ISiteGenerator> _siteGenerator;


        public ServeCommand(IProjectInfo projectInfo,
                            ISimpleServer server,
                            IAutoGenerator autoGenerator,
                            Func<IProjectInfo, ISiteGenerator> siteGenerator)
        {
            _projectInfo = projectInfo;
            _server = server;
            _autoGenerator = autoGenerator;
            _siteGenerator = siteGenerator;
            Port = 8765;
        }


        public void Execute()
        {
            // Parse the project info...
            _projectInfo.Init(false);

            // Make sure the site is up to date...
            _siteGenerator(_projectInfo).Generate();

            // Fire up the web server
            _server.Start(_projectInfo.OutputDirectory, Port);

            // Watch for changes in the source directory and regen
            _autoGenerator.Watch(_projectInfo, Beep);

            // If they asked for it, open a web browser...
            if (OpenBrowser)
            {
                System.Diagnostics.Process.Start(_server.Url);
            }

            // Wait for key press from the user...
            Console.WriteLine("Press [enter] to exit");
            Console.ReadLine();

            // Shut things down...
            _autoGenerator.Stop();
            _server.Stop();
        }


        [NamedParameter(ShortName = "p")]
        [Description("The port on which to listen. The default is 8765.")]
        public int Port { get; set; }


        
        [NamedParameter(ShortName = "b")]
        [Description("Make a short been when generation completes.")]
        public bool Beep { get; set; }


        [NamedParameter(ShortName = "w")]
        [Description("Open a web browser.")]
        public bool OpenBrowser { get; set; }
    }
}
