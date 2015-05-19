using System.IO;
using RazorEngine.Templating;

namespace Kurdle.Generation
{
    public class AsciiDocPageGenerator : AbstractPageGenerator
    {
        public AsciiDocPageGenerator(IRazorEngineService razorEngine, IProjectInfo projectInfo, DocumentEntry entry)
            : base(razorEngine, projectInfo, entry)
        {
        }



        protected override string GetPageContent(TextReader reader)
        {
            // TODO - implement AsciiDoc
            return string.Empty;
        }
    }
}
