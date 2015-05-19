using System;
using System.IO;
using Kurdle.Misc;
using Kurdle.Models;
using Kurdle.Services;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Kurdle.Generation
{
    public interface ISiteGenerator
    {
        void Generate(IProjectInfo projectInfo, bool dryRun);
    }



    public class SiteGenerator : ISiteGenerator
    {
        private readonly IPageGeneratorFactory _pageGeneratorFactory;
        private readonly IRazorEngineService _razorEngine;


        public SiteGenerator(IPageGeneratorFactory pageGeneratorFactory)
        {
            _pageGeneratorFactory = pageGeneratorFactory;
            var config = new TemplateServiceConfiguration
            {
                CachingProvider = new DefaultCachingProvider(t => {}),
                DisableTempFileLocking = true,
                TemplateManager = new EmbeddedTemplateManager("Kurdle.Templates")
            };

            _razorEngine = RazorEngineService.Create(config);

            Console.WriteLine("Compiling templates...");
            _razorEngine.Compile("Index", typeof(IndexModel));
        }


        public void Generate(IProjectInfo projectInfo, bool dryRun)
        {
            Console.WriteLine("Generating site...");

            // Make sure the output directory exists
            if (!dryRun)
            {
                if (!projectInfo.OutputDirectory.Exists)
                {
                    projectInfo.OutputDirectory.Create();
                }
            }

            // TODO - implement this for real!
            IndexModel model = new IndexModel { SiteName = projectInfo.SiteName };

            // Generate the home page
            //if (!dryRun)
            //{
            //    var file = GetFileInfo(projectInfo, "index.html");

            //    using (var writer = file.CreateText())
            //    {
            //        var result = _razorEngine.Run("Index", typeof(IndexModel), model);

            //        writer.Write(result);
            //    }
            //}

            // Generate all the documents...
            foreach (var doc in projectInfo.Documents)
            {
                var generator = _pageGeneratorFactory.Create(doc);

                generator.Generate();
            }

            //if (!dryRun)
            //{
            //    foreach (var doc in projectInfo.Documents)
            //    {
            //        var generator = _pageGeneratorFactory.Create(doc, dryRun);

            //        if (doc.Kind != DocumentKind.MarkDown)
            //        {
            //            continue;
            //        }

            //        var file = GetFileInfo(projectInfo, Path.ChangeExtension(doc.Info.Name, ".html"));

            //        // TODO - only grab the innards - use layout to wrap everything

            //        using (var writer = file.CreateText())
            //        {
            //            using (var reader = doc.Info.OpenText())
            //            {
            //                CommonMark.CommonMarkConverter.Convert(reader, writer);
            //            }
            //        }
            //    }
            //}
        }



        //private FileInfo GetFileInfo(IProjectInfo projectInfo, string filePath)
        //{
        //    var path = Path.Combine(projectInfo.OutputDirectory.FullName, filePath);

        //    return new FileInfo(Path.GetFullPath(path));
        //}
    }
}
