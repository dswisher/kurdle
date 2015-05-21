using System;
using System.Collections.Generic;
using Kurdle.Misc;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace Kurdle.Generation
{
    public interface IPageGeneratorFactory
    {
        IFileProcessor Create(IProjectInfo projectInfo, DocumentEntry entry);
    }



    public class PageGeneratorFactory : IPageGeneratorFactory
    {
        private readonly IRazorEngineService _razorEngine;
        private readonly HashSet<string> _compiledTemplates = new HashSet<string>();


        public PageGeneratorFactory()
        {
            var config = new TemplateServiceConfiguration
            {
                CachingProvider = new DefaultCachingProvider(t => { }),
                DisableTempFileLocking = true,
                TemplateManager = new EmbeddedTemplateManager("Kurdle.Templates")
            };

            _razorEngine = RazorEngineService.Create(config);
        }



        public IFileProcessor Create(IProjectInfo projectInfo, DocumentEntry entry)
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
                    generator = new MarkDownPageGenerator(_razorEngine, projectInfo, entry);
                    break;

                case DocumentKind.AsciiDoc:
                    generator = new AsciiDocPageGenerator(_razorEngine, projectInfo, entry);
                    break;

                case DocumentKind.Image:
                case DocumentKind.Script:
                case DocumentKind.Style:
                    generator = new CopyFileProcessor(projectInfo, entry);
                    break;

                default:
                    throw new NotImplementedException("Pages of kind '" + entry.Kind + "' are not yet implemented.");
            }

            // Return what we hath wrought...
            return generator;
        }

    }
}
