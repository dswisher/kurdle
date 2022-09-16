using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Kurdle.DepGraph.Models;
using Kurdle.Layout;
using Kurdle.Layout.Models;
using Markdig;
using Serilog;

namespace Kurdle.Processors.Index
{
    public class MarkdownProcessor : AbstractProcessorNode
    {
        private readonly FileInfo inputFile;
        private readonly FileInfo outputFile;
        private readonly MarkdownPipeline pipeline;
        private readonly LayoutTemplate layoutTemplate;
        private readonly ILogger logger;

        public MarkdownProcessor(
            FileInfo inputFile,
            FileInfo outputFile,
            MarkdownPipeline pipeline,
            LayoutTemplate layoutTemplate,
            ILogger logger)
        {
            this.inputFile = inputFile;
            this.outputFile = outputFile;
            this.pipeline = pipeline;
            this.layoutTemplate = layoutTemplate;
            this.logger = logger;
        }


        public override async Task ExecuteAsync(bool rebuild, CancellationToken cancellationToken)
        {
            // Convert the markdown to HTML
            var rawText = await File.ReadAllTextAsync(inputFile.FullName, cancellationToken);
            var htmlFragment = Markdown.ToHtml(rawText, pipeline);

            // TODO - how to pull out the yaml/json/whatever front matter to get things like page title?

            // Use the layout to render the final HTML
            // TODO - set page title based on front matter
            // TODO - have constants for things like "title"
            layoutTemplate.SetValue("title", "My First Page");

            // TODO - pull the layout name (main) from config
            var html = layoutTemplate.Apply(htmlFragment);

            // Make sure the output directory exists
            if (outputFile.Directory != null && !outputFile.Directory.Exists)
            {
                logger.Debug("Creating directory: {Path}", outputFile.Directory.FullName);
                outputFile.Directory.Create();
            }

            // Write the result
            await File.WriteAllTextAsync(outputFile.FullName, html, cancellationToken);

            logger.Debug("Wrote {Path}", outputFile.FullName);
        }
    }
}
