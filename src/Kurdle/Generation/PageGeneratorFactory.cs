using System;
using System.Collections.Generic;
using Kurdle.Misc;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Kurdle.Generation
{
    public interface IPageGeneratorFactory
    {
        IFileProcessor Create(DocumentEntry entry);
    }



    public class PageGeneratorFactory : IPageGeneratorFactory
    {
        private readonly IProjectInfo _projectInfo;
        private readonly IRazorEngineService _razorEngine;
        private readonly HashSet<string> _compiledTemplates = new HashSet<string>();


        public PageGeneratorFactory(IProjectInfo projectInfo)
        {
            _projectInfo = projectInfo;
            var config = new TemplateServiceConfiguration
            {
                CachingProvider = new DefaultCachingProvider(t => { }),
                DisableTempFileLocking = true,
                TemplateManager = new EmbeddedTemplateManager("Kurdle.Templates", projectInfo.TemplateDirectory)
            };

            _razorEngine = RazorEngineService.Create(config);
        }



        public IFileProcessor Create(DocumentEntry entry)
        {
            // Make sure the template is ready...
            if (!_compiledTemplates.Contains(entry.Template))
            {
                Console.WriteLine("...compiling '{0}' template...", entry.Template);
                _razorEngine.Compile(entry.Template, typeof(DocumentModel));
                _compiledTemplates.Add(entry.Template);
            }

            // Construct the page generator itself...
            IFileProcessor generator;
            switch (entry.Kind)
            {
                case DocumentKind.MarkDown:
                    generator = new MarkDownPageGenerator(_razorEngine, _projectInfo, entry);
                    break;

                case DocumentKind.AsciiDoc:
                    generator = new AsciiDocPageGenerator(_razorEngine, _projectInfo, entry);
                    break;

                case DocumentKind.Image:
                case DocumentKind.Script:
                case DocumentKind.Style:
                    generator = new CopyFileProcessor(_projectInfo, entry);
                    break;

                default:
                    throw new NotImplementedException("Pages of kind '" + entry.Kind + "' are not yet implemented.");
            }

            // Return what we hath wrought...
            return generator;
        }

    }
}
