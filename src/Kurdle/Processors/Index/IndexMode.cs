using System;
using System.Collections.Generic;
using System.IO;
using Kurdle.DepGraph.Models;
using Kurdle.Layout;
using Markdig;
using Serilog;

namespace Kurdle.Processors.Index
{
    public class IndexMode : IIndexMode
    {
        private readonly Dictionary<string, Action<DependencyGraph, ScanScope, FileInfo>> extensionMap = new();
        private readonly MarkdownPipeline pipeline;
        private readonly ILayoutManager layoutManager;
        private readonly ILogger logger;

        public IndexMode(
            MarkdownPipeline pipeline,
            ILayoutManager layoutManager,
            ILogger logger)
        {
            this.pipeline = pipeline;
            this.layoutManager = layoutManager;
            this.logger = logger;

            extensionMap.Add(".md", AddMarkdown);
        }


        public void AddFileToGraph(DependencyGraph graph, ScanScope scope, FileInfo file)
        {
            if (!extensionMap.ContainsKey(file.Extension))
            {
                // TODO - should this be a fatal error (aka exception)?
                logger.Warning("IndexMode - do not know how to handle files with extension {Extension}.", file.Extension);
                return;
            }

            var adder = extensionMap[file.Extension];

            adder(graph, scope, file);
        }


        private void AddMarkdown(DependencyGraph graph, ScanScope scope, FileInfo file)
        {
            // Determine the output file
            var outputName = Path.GetFileNameWithoutExtension(file.Name) + ".html";
            var outputPath = Path.Join(scope.OutputDirectory.FullName, outputName);
            var outputFile = new FileInfo(outputPath);

            // Create the processor and hook it into the graph
            var layoutTemplate = layoutManager.GetTemplate(scope.Layout);

            // TODO - xyzzy - do not pass layoutManager here! Instead, pass the template
            // TODO - the scope needs to include the current layout, pulled from the config, just like processing mode
            var processor = new MarkdownProcessor(file, outputFile, pipeline, layoutTemplate, logger);

            graph.AddProcessor(processor);

            // TODO - add a node for the input file
            // TODO - add a node for the output file
            // TODO - add a dependency from processor to input file
            // TODO - add a dependency from output file to processor
        }
    }
}
