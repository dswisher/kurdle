using System.IO;
using RazorEngine.Templating;

namespace Kurdle.Generation
{
    public abstract class AbstractPageGenerator : AbstractFileProcessor
    {
        private readonly IRazorEngineService _razorEngine;


        protected AbstractPageGenerator(IRazorEngineService razorEngine, IProjectInfo projectInfo, DocumentEntry entry)
            : base(projectInfo, entry)
        {
            _razorEngine = razorEngine;
        }


        protected abstract string GetPageContent(TextReader reader);


        public override void Process(bool dryRun)
        {
            // Get the page-specific content...
            string pageContent;
            using (var reader = _entry.Info.OpenText())
            {
                SkipHeader(reader);

                // Snag the actual, formatted content
                pageContent = GetPageContent(reader);
            }

            // Build the model.
            DocumentModel model = new DocumentModel
            {
                SiteName = _projectInfo.SiteName,
                PageContent = pageContent
            };

            // Format it up!
            var result = _razorEngine.Run(_entry.Template, typeof(DocumentModel), model);

            // And write out.
            if (!dryRun)
            {
                MakeOutputDir();

                var file = GetOutputInfo(".html");

                using (var writer = file.CreateText())
                {
                    writer.Write(result);
                }
            }
        }



        private void SkipHeader(TextReader reader)
        {
            string data;
            int count = 0;
            while (((data = reader.ReadLine()) != null) && (count < 2))
            {
                if (data == "---")
                {
                    count += 1;
                }
            }
        }
    }
}
