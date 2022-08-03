using System;
using Kurdle.Commands;
using Kurdle.Config;
using Kurdle.DepGraph;
using Kurdle.Processors.Blog;
using Kurdle.Processors.Index;
using Markdig;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Kurdle
{
    public static class Container
    {
        public static void RegisterServices(this ServiceCollection services)
        {
            // Add the logger
            services.AddSingleton(Log.Logger);

            // Add the commands
            services.AddSingleton<BuildCommand>();
            services.AddSingleton<CreateCommand>();

            // Add dependency graph stuff
            services.AddSingleton<Func<string, string, IDirectoryScanner>>(s => (root, output) =>
            {
                var configScanner = s.GetRequiredService<IConfigScanner>();
                var scopeUpdater = s.GetRequiredService<IScopeUpdater>();
                var logger = s.GetRequiredService<ILogger>();

                return new DirectoryScanner(root, output, configScanner, scopeUpdater, logger);
            });

            // Add config stuff
            services.AddSingleton<IConfigScanner, ConfigScanner>();
            services.AddSingleton<IScopeUpdater, ScopeUpdater>();

            // Add processor stuff
            services.AddSingleton<IBlogMode, BlogMode>();
            services.AddSingleton<IIndexMode, IndexMode>();

            // Set up the MarkDig pipeline
            // TODO - register our custom pipeline extensions
            var pipeline = new MarkdownPipelineBuilder()
                .UseAdvancedExtensions()
                .Build();

            services.AddSingleton(pipeline);
        }
    }
}
