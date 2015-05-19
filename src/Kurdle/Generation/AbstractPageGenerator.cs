using System.IO;
using RazorEngine.Templating;

namespace Kurdle.Generation
{
    public abstract class AbstractPageGenerator
    {
        private readonly IRazorEngineService _razorEngine;
        private readonly IProjectInfo _projectInfo;
        protected readonly DocumentEntry _entry;

        protected AbstractPageGenerator(IRazorEngineService razorEngine, IProjectInfo projectInfo, DocumentEntry entry)
        {
            _razorEngine = razorEngine;
            _projectInfo = projectInfo;
            _entry = entry;
        }


        protected abstract string GetPageContent(TextReader reader);


        public void Generate(bool dryRun)
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
                var dir = GetOutputDirectory();

                MakeDir(dir);

                var file = GetOutputPath(dir);

                using (var writer = file.CreateText())
                {
                    writer.Write(result);
                }
            }
        }



        private void MakeDir(DirectoryInfo dir)
        {
            if (!dir.Exists)
            {
                MakeDir(dir.Parent);

                dir.Create();
            }
        }



        private DirectoryInfo GetOutputDirectory()
        {
            var subdir = Path.Combine(_projectInfo.OutputDirectory.FullName, _entry.SubDirectory);

            return new DirectoryInfo(subdir);
        }



        private FileInfo GetOutputPath(DirectoryInfo outputDirectory)
        {
            var outputName = Path.ChangeExtension(_entry.Info.Name, ".html");
            var path = Path.Combine(outputDirectory.FullName, outputName);
            var file = new FileInfo(Path.GetFullPath(path));

            return file;
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
