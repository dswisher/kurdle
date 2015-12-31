using System.ComponentModel;
using Yaclops.Attributes;

namespace Kurdle.Commands
{
    public abstract class AbstractBuildCommand
    {
        [NamedParameter, ShortName("s"), LongName("source")]
        [Description("The site source.")]
        public string SourceDir { get; set; }

        [NamedParameter, ShortName("d"), LongName("destination")]
        [Description("The site destination.")]
        public string DestinationDir { get; set; }
    }
}
