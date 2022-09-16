using System;
using Kurdle.Config.Models;
using Kurdle.DepGraph.Models;
using Kurdle.Processors.Blog;
using Kurdle.Processors.Index;
using Serilog;

namespace Kurdle.Config
{
    public class ScopeUpdater : IScopeUpdater
    {
        private readonly IBlogMode blogMode;
        private readonly IIndexMode indexMode;
        private readonly ILogger logger;

        public ScopeUpdater(
            IBlogMode blogMode,
            IIndexMode indexMode,
            ILogger logger)
        {
            this.blogMode = blogMode;
            this.indexMode = indexMode;
            this.logger = logger;
        }


        public void Update(ScanScope scope, ScopeConfig config)
        {
            if (!string.IsNullOrEmpty(config.Layout))
            {
                scope.Layout = config.Layout;
            }

            // Set up the processing mode
            switch (config.ProcessingMode)
            {
                case ProcessingMode.Blog:
                    scope.ProcessingMode = blogMode;
                    break;

                case ProcessingMode.Index:
                    scope.ProcessingMode = indexMode;
                    break;

                default:
                    // TODO - custom exception
                    throw new NotImplementedException($"Processing mode {config.ProcessingMode} is not yet implemented!");
            }
        }
    }
}
