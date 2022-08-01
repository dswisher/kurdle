using CommandLine;

namespace Kurdle.Options
{
    public abstract class CommonOptions
    {
        [Option("verbose", HelpText = "Enable verbose logging.")]
        public bool Verbose { get; set; }
    }
}
