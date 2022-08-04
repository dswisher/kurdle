using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Kurdle.DepGraph.Models;
using Markdig;
using Serilog;

namespace Kurdle.Processors.Index
{
    public class MarkdownProcessor : AbstractProcessorNode
    {
        private readonly FileInfo inputFile;
        private readonly FileInfo outputFile;
        private readonly MarkdownPipeline pipeline;
        private readonly ILogger logger;

        public MarkdownProcessor(
            FileInfo inputFile,
            FileInfo outputFile,
            MarkdownPipeline pipeline,
            ILogger logger)
        {
            this.inputFile = inputFile;
            this.outputFile = outputFile;
            this.pipeline = pipeline;
            this.logger = logger;
        }


        public override async Task ExecuteAsync(bool rebuild, CancellationToken cancellationToken)
        {
            // Convert the markdown to HTML
            var rawText = await File.ReadAllTextAsync(inputFile.FullName, cancellationToken);
            var htmlFragment = Markdown.ToHtml(rawText, pipeline);

            // Use the layout to render the final HTML
            // TODO - xyzzy - implement me!

            // Make sure the output directory exists
            if (outputFile.Directory != null && !outputFile.Directory.Exists)
            {
                logger.Debug("Creating directory: {Path}", outputFile.Directory.FullName);
                outputFile.Directory.Create();
            }

            // Write the result
            await File.WriteAllTextAsync(outputFile.FullName, htmlFragment, cancellationToken);
        }
    }
}
