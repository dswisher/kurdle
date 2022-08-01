using CommandLine;

namespace Kurdle.Options
{
    [Verb("build", HelpText = "Build the site")]
    public class BuildOptions : CommonOptions
    {
        [Value(0, HelpText = "The root directory of the site tree.")]
        public string SourceDir { get; set; }

        [Option("rebuild", HelpText = "Do a full rebuild of all files. The default is an incremental build.")]
        public bool Rebuild { get; set; }
    }
}
