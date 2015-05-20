using System;


namespace Kurdle.Generation
{
    public interface ISiteGenerator
    {
        void Generate(IProjectInfo projectInfo, bool dryRun);
    }



    public class SiteGenerator : ISiteGenerator
    {
        private readonly IPageGeneratorFactory _pageGeneratorFactory;


        public SiteGenerator(IPageGeneratorFactory pageGeneratorFactory)
        {
            _pageGeneratorFactory = pageGeneratorFactory;
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

            // Generate all the documents...
            foreach (var doc in projectInfo.Documents)
            {
                var generator = _pageGeneratorFactory.Create(projectInfo, doc);

                generator.Process(dryRun);
            }
        }
    }
}
