using System.IO;
using CommonMark;
using RazorEngine.Templating;

namespace Kurdle.Generation
{
    public class MarkDownPageGenerator : AbstractPageGenerator
    {
        public MarkDownPageGenerator(IRazorEngineService razorEngine, IProjectInfo projectInfo, DocumentEntry entry)
            : base(razorEngine, projectInfo, entry)
        {
        }



        protected override string GetPageContent(TextReader reader)
        {
            return CommonMarkConverter.Convert(reader.ReadToEnd());
        }
    }
}
